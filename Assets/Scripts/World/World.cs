using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour
{
    public Simulation Simulation;

    // Tiles are stored in a dictionary, where the key is their coordinates
    public Dictionary<Vector2Int, WorldTile> Tiles = new Dictionary<Vector2Int, WorldTile>();

    /// <summary>
    /// After world generation each tile gets assigned a pot.
    /// <br/> Per Frame only one put gets updated for performance reasons.
    /// </summary>
    public Dictionary<int, List<WorldTile>> TileUpdatePots = new Dictionary<int, List<WorldTile>>();
    private int CurrentTilePot = 0;

    public TerrainLayer TerrainLayer;

    #region Initialization

    private void Start()
    {
        _Singleton = GameObject.Find("Grid").GetComponent<World>();
    }

    /// <summary>
    /// Draws all tiles of the world.
    /// </summary>
    public void DrawTiles(Dictionary<Vector2Int, WorldTile> tiles)
    {
        ClearGrid();
        TileUpdatePots.Clear();
        for (int i = 0; i < Simulation.TILE_UPDATE_POTS; i++) TileUpdatePots.Add(i, new List<WorldTile>());

        Tiles = tiles;

        foreach (WorldTile tile in Tiles.Values)
        {
            TileUpdatePots[CurrentTilePot++].Add(tile);
            if (CurrentTilePot >= Simulation.TILE_UPDATE_POTS) CurrentTilePot = 0;

            // Surface Tiles
            TerrainLayer.DrawSurface(tile.Coordinates, tile.Surface, refreshAdjacentTransitions: false);
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

    #region Update

    private Dictionary<int, SimulationTime> LastTileUpdateTime = new Dictionary<int, SimulationTime>();

    private void Update()
    {
        if (Simulation.IsPaused) return;

        float hoursSinceLastUpdate;
        if (!LastTileUpdateTime.ContainsKey(CurrentTilePot)) hoursSinceLastUpdate = Simulation.LastFrameHoursPassed;
        else hoursSinceLastUpdate = Simulation.CurrentTime.ExactTime - LastTileUpdateTime[CurrentTilePot].ExactTime;
        LastTileUpdateTime[CurrentTilePot] = new SimulationTime(Simulation.CurrentTime);

        foreach (WorldTile tile in TileUpdatePots[CurrentTilePot++]) tile.Update(hoursSinceLastUpdate);
        if (CurrentTilePot >= Simulation.TILE_UPDATE_POTS) CurrentTilePot = 0;
    }


    #endregion

    #region Setters

    public void SetTerrain(WorldTile tile, Surface surface)
    {
        if (tile == null) return;
        tile.SetSurface(surface);
        TerrainLayer.DrawSurface(tile.Coordinates, surface, refreshAdjacentTransitions: true);
    }

    public void SpawnTileObject(WorldTile tile, TileObjectType tileObjectType)
    {
        if (tile.TileObjects.Any(x => x.Type == tileObjectType)) return;
        VisibleTileObject newObject = TileObjectFactory.CreateObject(tileObjectType);
        newObject.transform.position = tile.WorldPosition3;
        tile.AddObject(newObject);
        newObject.SetTile(tile);
        newObject.SetIsSimulated(true);
    }

    public void RemoveObjects(WorldTile tile)
    {
        foreach(VisibleTileObject obj in tile.TileObjects) Destroy(obj.gameObject);
        tile.TileObjects.Clear();
    }

    public void RemoveObject(VisibleTileObject tileObject)
    {
        tileObject.Tile.RemoveObject(tileObject);
        Destroy(tileObject.gameObject);

        // Close UI windows showing this object
        UIHandler.Singleton.CloseSelectionWindow(tileObject);
        UIHandler.Singleton.CloseThingInfoWindow(tileObject);
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
        WorldTile tile;
        Tiles.TryGetValue(coordinates, out tile);
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
        foreach (Vector2Int v in HelperFunctions.GetAllPositionsAround(pos, range)) tiles.Add(GetTile(v));
        return tiles;
    }
    public WorldTile GetRandomTileInRange(Vector2Int pos, int range)
    {
        return GetTile(HelperFunctions.GetRandomPositionInRange(pos, range));
    }


    public Surface GetSurface(int x, int y)
    {
        return GetSurface(new Vector2Int(x, y));
    }
    public Surface GetSurface(Vector2Int position)
    {
        if (!Tiles.ContainsKey(position)) return null;
        else return Tiles[position].Surface;
    }
    public Surface GetSurfaceInDirection(Vector2Int pos, Direction dir)
    {
        return GetSurface(HelperFunctions.GetPositionInDirection(pos, dir));
    }

    private static World _Singleton;
    public static World Singleton => _Singleton;
    #endregion

}
