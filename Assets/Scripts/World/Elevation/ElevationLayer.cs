using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ElevationLayer : MonoBehaviour
{
    public Tilemap CliffTilemap;
    public Tilemap SlopeTilemap;
    public Tilemap[] ElevationBrightnessTilemaps;

    private ElevationTileset CliffTileset;
    private ElevationTileset SlopeTileset;
    

    // Start is called before the first frame update
    void Start()
    {
        CliffTileset = new ElevationTileset("Cliff", ResourceManager.Singleton.CliffTileset, 128);
        SlopeTileset = new ElevationTileset("Slope", ResourceManager.Singleton.SlopeTileset, 128);
    }

    public void DrawElevation(WorldTile tile)
    {
        for (int i = 0; i < tile.MinElevation; i++)
        {
            ElevationBrightnessTilemaps[i].SetTile(tile.Coordinates3, ResourceManager.Singleton.WhiteTile);
        }

        if (tile.ElevationType == TileElevationType.Flat || tile.MaxElevation <= 0) return;
        else if (tile.ElevationType == TileElevationType.Slope) SlopeTilemap.SetTile(tile.Coordinates3, SlopeTileset.GetTile(tile.ElevationDirection));
        else if (tile.ElevationType == TileElevationType.Cliff) CliffTilemap.SetTile(tile.Coordinates3, CliffTileset.GetTile(tile.ElevationDirection));
    }
}
