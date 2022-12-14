using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour
{
    // Tiles are stored in a dictionary, where the key is their coordinates
    public Dictionary<Vector2Int, WorldTile> Tiles = new Dictionary<Vector2Int, WorldTile>();

    /// <summary>
    /// After world generation each tile gets assigned a pot.
    /// <br/> Per Frame only one put gets updated for performance reasons.
    /// </summary>
    public Dictionary<int, List<WorldTile>> TileUpdatePots = new Dictionary<int, List<WorldTile>>();
    private int CurrentTilePot = 0;

    public TerrainLayer TerrainLayer;
    public ElevationLayer ElevationLayer;

    #region Initialization

    private void Start()
    {
        _Singleton = GameObject.Find("Grid").GetComponent<World>();
    }

    /// <summary>
    /// Draws all tiles of the world.
    /// </summary>
    public void DrawTiles()
    {
        ClearGrid();
        TileUpdatePots.Clear();
        for (int i = 0; i < Simulation.NUM_TILE_UPDATE_POTS; i++) TileUpdatePots.Add(i, new List<WorldTile>());

        foreach (WorldTile tile in Tiles.Values)
        {
            TileUpdatePots[CurrentTilePot++].Add(tile);
            if (CurrentTilePot >= Simulation.NUM_TILE_UPDATE_POTS) CurrentTilePot = 0;

            // Surface Tiles
            TerrainLayer.DrawSurface(tile.Coordinates, tile.Surface, refreshAdjacentTransitions: false);
            ElevationLayer.DrawElevation(tile);
        }
    }

    /// <summary>
    /// Removes everything from the current battle map from the grid.
    /// </summary>
    private void ClearGrid()
    {
        TerrainLayer.ClearGrid();
    }

    #endregion

    #region Setters

    public void SetTerrain(WorldTile tile, SurfaceBase surface)
    {
        if (tile == null) return;
        tile.SetSurface(surface);
        TerrainLayer.DrawSurface(tile.Coordinates, surface, refreshAdjacentTransitions: true);
    }

    /// <summary>
    /// Spawns a new TileObject of a specified type on a tile. If isNew is false, the object will already have a certain age and random properties set.
    /// </summary>
    public void SpawnTileObject(WorldTile tile, TileObjectId tileObjectType, bool isNew = true)
    {
        VisibleTileObjectBase newObject = TileObjectFactory.CreateObject(tileObjectType, isNew);
        newObject.transform.position = tile.WorldPosition3;
        tile.AddObject(newObject);
        newObject.SetTile(tile);

        // Register to the simulation
        Simulation.Singleton.RegisterObject(newObject);
    }

    public void RemoveObjects(WorldTile tile)
    {
        foreach (TileObjectBase tileObject in tile.TileObjects)
        {
            if (tileObject is VisibleTileObjectBase visibleObject) Destroy(visibleObject.gameObject);

            UIHandler.Singleton.CloseSelectionWindow(tileObject);
            UIHandler.Singleton.CloseThingInfoWindow(tileObject);

            Simulation.Singleton.UnregisterObject(tileObject);
        }
        tile.TileObjects.Clear();
    }

    /// <summary>
    /// Destroys and removes a tileobject from the world.
    /// </summary>
    public void RemoveObject(TileObjectBase tileObject)
    {
        tileObject.Tile.RemoveObject(tileObject);
        if(tileObject is VisibleTileObjectBase visibleObject) Destroy(visibleObject.gameObject);

        // Close UI windows showing this object
        UIHandler.Singleton.CloseSelectionWindow(tileObject);
        UIHandler.Singleton.CloseThingInfoWindow(tileObject);

        // Unregister from simulation
        Simulation.Singleton.UnregisterObject(tileObject);
    }

    #endregion

    #region Getters

    public float MinWorldX => Tiles.Min(x => x.Key.x);
    public float MaxWorldX => Tiles.Max(x => x.Key.x);
    public float MinWorldY => Tiles.Min(x => x.Key.y);
    public float MaxWorldY => Tiles.Max(x => x.Key.y);
    public float CenterWorldX => (MinWorldX + MaxWorldX) / 2f;
    public float CenterWorldY => (MinWorldY + MaxWorldY) / 2f;

    public WorldTile GetTile(Vector2Int coordinates)
    {
        Tiles.TryGetValue(coordinates, out WorldTile tile);
        return tile;
    }
    public WorldTile GetTile(int x, int y)
    {
        return GetTile(new Vector2Int(x, y));
    }
    public WorldTile GetTile(Vector3 worldPosition)
    {
        Vector3Int tileCoords = GetComponent<Grid>().LocalToCell(worldPosition);
        return GetTile(tileCoords.x, tileCoords.y);
    }
    public WorldTile GetTileInDirection(Vector2Int pos, Direction dir)
    {
        return GetTile(HelperFunctions.GetPositionInDirection(pos, dir));
    }
    public List<WorldTile> GetAllTilesInRange(Vector2Int pos, int range)
    {
        List<WorldTile> tiles = new List<WorldTile>();
        foreach (Vector2Int v in HelperFunctions.GetAllPositionsWithinRange(pos, range)) tiles.Add(GetTile(v));
        return tiles;
    }
    public WorldTile GetRandomTileInRange(Vector2Int pos, int range)
    {
        return GetTile(HelperFunctions.GetRandomPositionWithinRange(pos, range));
    }


    public SurfaceBase GetSurface(int x, int y)
    {
        return GetSurface(new Vector2Int(x, y));
    }
    public SurfaceBase GetSurface(Vector2Int position)
    {
        if (!Tiles.ContainsKey(position)) return null;
        else return Tiles[position].Surface;
    }
    public SurfaceBase GetSurfaceInDirection(Vector2Int pos, Direction dir)
    {
        return GetSurface(HelperFunctions.GetPositionInDirection(pos, dir));
    }

    private static World _Singleton;
    public static World Singleton => _Singleton;
    #endregion

}
