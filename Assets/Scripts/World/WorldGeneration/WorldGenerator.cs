using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WorldGenerator
{
    // Increase Simulation.TILE_UPDATE_POTS with bigger map sizes and performance keeps being good (16 for 200x200) (128 for 400x400)
    public static int MAP_WIDTH => 300;
    public static int MAP_HEIGHT => 300;

    private static WorldGenerationInfo Info;
    private static World World;

    public static Dictionary<Vector2Int, WorldTile> GenerateMap(World world, WorldGenerationInfo info)
    {
        World = world;
        Info = info;

        World.Tiles = new Dictionary<Vector2Int, WorldTile>();

        CreateTileInstances();
        CreateSurfaces();
        PopulateWorld();

        Debug.Log("Generated world with " + World.Tiles.Count + " tiles.");

        return World.Tiles;
    }

    private static void CreateTileInstances()
    {
        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                WorldTile tile = new WorldTile(World, pos);
                World.Tiles.Add(pos, tile);
            }
        }
    }

    private static void CreateSurfaces()
    {
        LayeredPerlinNoise noise = new LayeredPerlinNoise(scale: 0.03f, numOctaves: 5);

        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                SurfaceBase surface = null;
                Vector2Int pos = new Vector2Int(x, y);
                if (noise.GetValue(pos) < 0.3f) surface = World.TerrainLayer.Surfaces[SurfaceId.Water];
                else if (noise.GetValue(pos) < 0.4f) surface = World.TerrainLayer.Surfaces[SurfaceId.Sand];
                else if (noise.GetValue(pos) < 2f) surface = World.TerrainLayer.Surfaces[SurfaceId.Soil];

                World.Tiles[pos].SetSurface(surface);
            }
        }
    }

    private static void PopulateWorld()
    {
        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                Vector2Int pos = new(x, y);
                World.Tiles[pos].OnWorldGeneration();
            }
        }
    }
}
