using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainLayer : MonoBehaviour
{
    public World World;
    public Tilemap TerrainBaseLayer;
    public TilemapBlendLayer[] TerrainBlendLayers;

    /// <summary>
    /// Dictionary containing the unique instances of each surface.
    /// </summary>
    public Dictionary<SurfaceId, SurfaceBase> Surfaces;

    #region Initialization

    private void Awake()
    {
        // Initilaize surfaces
        Surfaces = new Dictionary<SurfaceId, SurfaceBase>();
        Surfaces.Add(SurfaceId.Soil, new Surface_Soil());
        Surfaces.Add(SurfaceId.Water, new Surface_Water());
        Surfaces.Add(SurfaceId.Sand, new Surface_Sand());
    }

    #endregion

    #region Drawing

    /// <summary>
    /// Removes everything from the current battle map from the grid.
    /// </summary>
    public void ClearGrid()
    {
        TerrainBaseLayer.ClearAllTiles();
        foreach (TilemapBlendLayer layer in TerrainBlendLayers) layer.ClearAllTiles();
    }

    private void ClearSurfaceBlendTiles(Vector2Int pos)
    {
        foreach (TilemapBlendLayer layer in TerrainBlendLayers) layer.ClearTiles(pos);
    }

    /// <summary>
    /// Removes and recalculates and redraws the surface transition tiles for a position.
    /// </summary>
    private void RefreshSurfaceTransitionTiles(Vector2Int pos)
    {
        int thisPrecedence = World.GetSurface(pos).Precedence;

        // Remove tiles on all overlay maps
        ClearSurfaceBlendTiles(pos);

        // Refresh tiles
        // Make a dictionary that contains all surfaces that will overlay our tile with a list of adjacent tiles with that surface
        Dictionary<SurfaceBase, List<Direction>> overlayTiles = new Dictionary<SurfaceBase, List<Direction>>();
        foreach (Direction dir in HelperFunctions.GetAdjacentDirections())
        {
            SurfaceBase surface = World.GetSurfaceInDirection(pos, dir);
            if (surface == null) continue; // Out of bounds
            if (surface.Precedence <= thisPrecedence) continue; // We ignore adjacent tiles with lower precedence, as they don't overlay this tile
            else
            {
                if (overlayTiles.ContainsKey(surface)) overlayTiles[surface].Add(dir);
                else overlayTiles.Add(surface, new List<Direction>() { dir });
            }
        }

        // Order our dictionary by precedence so we can overlay the ones with the lowest precedence first
        overlayTiles = overlayTiles.OrderBy(x => x.Key.Precedence).ToDictionary(x => x.Key, x => x.Value);

        // Apply overlays for each surface
        int blendLayerIndex = 0;
        foreach (KeyValuePair<SurfaceBase, List<Direction>> surfaceOverlays in overlayTiles)
        {
            SurfaceBase surface = surfaceOverlays.Key;
            //string overlayString = TilemapFunctions.GetOverlayString(surfaceOverlays.Value);
            List<TilemapBlendType> blendTypes = DirectionsToBlendType(surfaceOverlays.Value);
            foreach (TilemapBlendType type in blendTypes) DrawBlendTile(surface.SurfaceId, pos, type, blendLayerIndex);
            blendLayerIndex++;
        }
    }

    /// <summary>
    /// This function takes a list of directions and converts them to a a list of TilemapBlendTypes.
    /// <br/>It is used as that the list of directions contains all directions that a specific surface is located around a position and then calculates which blend types needs to be filled for blending that surface.
    /// <br/>For example when the surface is occuring in the north, northeast and east of the position, the blend type Side_NE is returned.
    /// </summary>
    private List<TilemapBlendType> DirectionsToBlendType(List<Direction> directions)
    {
        List<TilemapBlendType> blendTypes = new List<TilemapBlendType>();

        // SIDES

        // 4 Sides
        if (directions.Contains(Direction.N) && directions.Contains(Direction.E) && directions.Contains(Direction.S) && directions.Contains(Direction.W))
            blendTypes.Add(TilemapBlendType.Side_NESW);

        // 3 Sides
        else if (directions.Contains(Direction.N) && directions.Contains(Direction.E) && directions.Contains(Direction.S))
            blendTypes.Add(TilemapBlendType.Side_NES);
        else if (directions.Contains(Direction.W) && directions.Contains(Direction.N) && directions.Contains(Direction.E))
            blendTypes.Add(TilemapBlendType.Side_WNE);
        else if (directions.Contains(Direction.S) && directions.Contains(Direction.W) && directions.Contains(Direction.N))
            blendTypes.Add(TilemapBlendType.Side_SWN);
        else if (directions.Contains(Direction.E) && directions.Contains(Direction.S) && directions.Contains(Direction.W))
            blendTypes.Add(TilemapBlendType.Side_ESW);

        // 2 Sides corner
        else if (directions.Contains(Direction.N) && directions.Contains(Direction.E))
        {
            blendTypes.Add(TilemapBlendType.Side_NE);
            if (directions.Contains(Direction.SW)) blendTypes.Add(TilemapBlendType.Corner_SW);
        }
        else if (directions.Contains(Direction.W) && directions.Contains(Direction.N))
        {
            blendTypes.Add(TilemapBlendType.Side_NW);
            if (directions.Contains(Direction.SE)) blendTypes.Add(TilemapBlendType.Corner_SE);
        }
        else if (directions.Contains(Direction.S) && directions.Contains(Direction.W))
        {
            blendTypes.Add(TilemapBlendType.Side_SW);
            if (directions.Contains(Direction.NE)) blendTypes.Add(TilemapBlendType.Corner_NE);
        }
        else if (directions.Contains(Direction.E) && directions.Contains(Direction.S))
        {
            blendTypes.Add(TilemapBlendType.Side_SE);
            if (directions.Contains(Direction.NW)) blendTypes.Add(TilemapBlendType.Corner_NW);
        }

        // 2 Sides parallel
        else if (directions.Contains(Direction.N) && directions.Contains(Direction.S))
        {
            blendTypes.Add(TilemapBlendType.Side_N);
            blendTypes.Add(TilemapBlendType.Side_S);
        }
        else if (directions.Contains(Direction.E) && directions.Contains(Direction.W))
        {
            blendTypes.Add(TilemapBlendType.Side_E);
            blendTypes.Add(TilemapBlendType.Side_W);
        }

        // 1 Side
        else if (directions.Contains(Direction.N))
        {
            blendTypes.Add(TilemapBlendType.Side_N);
            if (directions.Contains(Direction.SE)) blendTypes.Add(TilemapBlendType.Corner_SE);
            if (directions.Contains(Direction.SW)) blendTypes.Add(TilemapBlendType.Corner_SW);
        }
        else if (directions.Contains(Direction.E))
        {
            blendTypes.Add(TilemapBlendType.Side_E);
            if (directions.Contains(Direction.NW)) blendTypes.Add(TilemapBlendType.Corner_NW);
            if (directions.Contains(Direction.SW)) blendTypes.Add(TilemapBlendType.Corner_SW);
        }
        else if (directions.Contains(Direction.S))
        {
            blendTypes.Add(TilemapBlendType.Side_S);
            if (directions.Contains(Direction.NW)) blendTypes.Add(TilemapBlendType.Corner_NW);
            if (directions.Contains(Direction.NE)) blendTypes.Add(TilemapBlendType.Corner_NE);
        }
        else if (directions.Contains(Direction.W))
        {
            blendTypes.Add(TilemapBlendType.Side_W);
            if (directions.Contains(Direction.NE)) blendTypes.Add(TilemapBlendType.Corner_NE);
            if (directions.Contains(Direction.SE)) blendTypes.Add(TilemapBlendType.Corner_SE);
        }

        // No sides
        else
        {
            if (directions.Contains(Direction.NE)) blendTypes.Add(TilemapBlendType.Corner_NE);
            if (directions.Contains(Direction.SE)) blendTypes.Add(TilemapBlendType.Corner_SE);
            if (directions.Contains(Direction.SW)) blendTypes.Add(TilemapBlendType.Corner_SW);
            if (directions.Contains(Direction.NW)) blendTypes.Add(TilemapBlendType.Corner_NW);
        }

        return blendTypes;
    }

    /// <summary>
    /// Places a surface tile of the given surface on the correct tilemap according to the given layer and blend type.
    /// </summary>
    private void DrawBlendTile(SurfaceId surfaceType, Vector2Int pos, TilemapBlendType blendType, int layer)
    {
        Vector3Int pos3 = new Vector3Int(pos.x, pos.y, 0);
        TerrainBlendLayers[layer].DrawTile(pos, blendType, ResourceManager.Singleton.GetSurfaceTile(surfaceType));
    }

    #endregion

    #region Setters

    public void DrawSurface(Vector2Int coordinates, SurfaceBase surface, bool refreshAdjacentTransitions)
    {
        Vector3Int pos = new Vector3Int(coordinates.x, coordinates.y, 0);
        TerrainBaseLayer.SetTile(pos, ResourceManager.Singleton.GetSurfaceTile(surface.SurfaceId));
        RefreshSurfaceTransitionTiles(coordinates);

        if(refreshAdjacentTransitions)
        {
            foreach(Vector2Int adjCoordinates in HelperFunctions.GetAdjacentCoordinates(coordinates))
            {
                RefreshSurfaceTransitionTiles(adjCoordinates);
            }
        }
    }

    #endregion
}

public enum TilemapBlendType
{
    Corner_NE,
    Corner_SE,
    Corner_SW,
    Corner_NW,
    Side_N,
    Side_E,
    Side_S,
    Side_W,
    Side_NE,
    Side_SE,
    Side_SW,
    Side_NW,
    Side_ESW,
    Side_SWN,
    Side_WNE,
    Side_NES,
    Side_NESW,
}
