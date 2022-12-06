using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// One blend layer consinst of 17 tilemaps, representing the different possible blend types of bordering tiles.
/// All tilemaps within one blend layer have the same sorting order.
/// Per tile only non-overlapping tilemaps should be filled!
/// </summary>
public class TilemapBlendLayer : MonoBehaviour
{
    public Tilemap BlendTilemap_Corner_NE;
    public Tilemap BlendTilemap_Corner_SE;
    public Tilemap BlendTilemap_Corner_SW;
    public Tilemap BlendTilemap_Corner_NW;
    public Tilemap BlendTilemap_Side_N;
    public Tilemap BlendTilemap_Side_E;
    public Tilemap BlendTilemap_Side_S;
    public Tilemap BlendTilemap_Side_W;
    public Tilemap BlendTilemap_Side_NE;
    public Tilemap BlendTilemap_Side_SE;
    public Tilemap BlendTilemap_Side_SW;
    public Tilemap BlendTilemap_Side_NW;
    public Tilemap BlendTilemap_Side_ESW;
    public Tilemap BlendTilemap_Side_SWN;
    public Tilemap BlendTilemap_Side_WNE;
    public Tilemap BlendTilemap_Side_NES;
    public Tilemap BlendTilemap_Side_NESW;

    private Dictionary<TilemapBlendType, Tilemap> Tilemaps;

    private void Awake()
    {
        Tilemaps = new Dictionary<TilemapBlendType, Tilemap>();

        Tilemaps.Add(TilemapBlendType.Corner_NE, BlendTilemap_Corner_NE);
        Tilemaps.Add(TilemapBlendType.Corner_NW, BlendTilemap_Corner_NW);
        Tilemaps.Add(TilemapBlendType.Corner_SW, BlendTilemap_Corner_SW);
        Tilemaps.Add(TilemapBlendType.Corner_SE, BlendTilemap_Corner_SE);
        Tilemaps.Add(TilemapBlendType.Side_N, BlendTilemap_Side_N);
        Tilemaps.Add(TilemapBlendType.Side_E, BlendTilemap_Side_E);
        Tilemaps.Add(TilemapBlendType.Side_S, BlendTilemap_Side_S);
        Tilemaps.Add(TilemapBlendType.Side_W, BlendTilemap_Side_W);
        Tilemaps.Add(TilemapBlendType.Side_NE, BlendTilemap_Side_NE);
        Tilemaps.Add(TilemapBlendType.Side_SE, BlendTilemap_Side_SE);
        Tilemaps.Add(TilemapBlendType.Side_SW, BlendTilemap_Side_SW);
        Tilemaps.Add(TilemapBlendType.Side_NW, BlendTilemap_Side_NW);
        Tilemaps.Add(TilemapBlendType.Side_ESW, BlendTilemap_Side_ESW);
        Tilemaps.Add(TilemapBlendType.Side_SWN, BlendTilemap_Side_SWN);
        Tilemaps.Add(TilemapBlendType.Side_WNE, BlendTilemap_Side_WNE);
        Tilemaps.Add(TilemapBlendType.Side_NES, BlendTilemap_Side_NES);
        Tilemaps.Add(TilemapBlendType.Side_NESW, BlendTilemap_Side_NESW);
    }

    public void ClearAllTiles()
    {
        foreach (Tilemap tm in Tilemaps.Values) tm.ClearAllTiles();
    }

    public void ClearTiles(Vector2Int coordinates)
    {
        Vector3Int pos = new Vector3Int(coordinates.x, coordinates.y, 0);
        foreach (Tilemap tm in Tilemaps.Values) tm.SetTile(pos, null);
    }

    public void DrawTile(Vector2Int coordinates, TilemapBlendType blendType, TileBase tile)
    {
        Vector3Int pos = new Vector3Int(coordinates.x, coordinates.y, 0);
        Tilemaps[blendType].SetTile(pos, tile);
    }
}
