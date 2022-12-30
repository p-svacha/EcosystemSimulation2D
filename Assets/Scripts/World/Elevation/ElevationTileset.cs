using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ElevationTileset
{
    private Dictionary<TileElevationDirection, TileBase> Tiles;

    public ElevationTileset(string name, Texture2D texture, int tileSize)
    {
        Tiles = new Dictionary<TileElevationDirection, TileBase>();

        Tiles.Add(TileElevationDirection.Corner_SE, TileGenerator.CreateTileFromTexture(texture, 0, 0, tileSize, name + "_Corner_SE"));
        Tiles.Add(TileElevationDirection.Side_S, TileGenerator.CreateTileFromTexture(texture, 0, 1, tileSize, name + "_Side_S"));
        Tiles.Add(TileElevationDirection.Corner_SW, TileGenerator.CreateTileFromTexture(texture, 0, 3, tileSize, name + "_Corner_SW"));
        Tiles.Add(TileElevationDirection.Side_E, TileGenerator.CreateTileFromTexture(texture, 1, 0, tileSize, name + "_Side_E"));
        Tiles.Add(TileElevationDirection.Side_NW, TileGenerator.CreateTileFromTexture(texture, 1, 1, tileSize, name + "_Side_NW"));
        Tiles.Add(TileElevationDirection.Side_NE, TileGenerator.CreateTileFromTexture(texture, 1, 2, tileSize, name + "_Side_NE"));
        Tiles.Add(TileElevationDirection.Side_W, TileGenerator.CreateTileFromTexture(texture, 1, 3, tileSize, name + "_Side_W"));
        Tiles.Add(TileElevationDirection.Corners_NW_SE, TileGenerator.CreateTileFromTexture(texture, 2, 0, tileSize, name + "_Corners_NW_SE"));
        Tiles.Add(TileElevationDirection.Side_SW, TileGenerator.CreateTileFromTexture(texture, 2, 1, tileSize, name + "_Side_SW"));
        Tiles.Add(TileElevationDirection.Side_SE, TileGenerator.CreateTileFromTexture(texture, 2, 2, tileSize, name + "_Side_SE"));
        Tiles.Add(TileElevationDirection.Corners_NE_SW, TileGenerator.CreateTileFromTexture(texture, 2, 3, tileSize, name + "_Corners_NE_SW"));
        Tiles.Add(TileElevationDirection.Corner_NE, TileGenerator.CreateTileFromTexture(texture, 3, 0, tileSize, name + "_Corner_NE"));
        Tiles.Add(TileElevationDirection.Side_N, TileGenerator.CreateTileFromTexture(texture, 3, 1, tileSize, name + "_Side_N"));
        Tiles.Add(TileElevationDirection.Corner_NW, TileGenerator.CreateTileFromTexture(texture, 3, 3, tileSize, name + "_Corner_NW"));
    }

    public TileBase GetTile(TileElevationDirection dir)
    {
        return Tiles[dir];
    }
}
