using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WorldGenerator
{
    // Increase Simulation.TILE_UPDATE_POTS with bigger map sizes and performance keeps being good (16 for 200x200) (128 for 400x400)
    public static int MAP_WIDTH => 250;
    public static int MAP_HEIGHT => 250;

    private static WorldGenerationInfo Info;
    private static World World;

    private static Dictionary<Vector2Int, WorldTile> Tiles;

    public static Dictionary<Vector2Int, WorldTile> GenerateMap(World world, WorldGenerationInfo info)
    {
        World = world;
        Info = info;

        Tiles = new Dictionary<Vector2Int, WorldTile>();

        CreateTileInstances();
        CreateSurfaces();
        PopulateWorld();

        Debug.Log("Generated world with " + Tiles.Count + " tiles.");

        return Tiles;
    }

    private static void CreateTileInstances()
    {
        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                WorldTile tile = new WorldTile(World, pos);
                Tiles.Add(pos, tile);
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
                if (noise.GetValue(pos) < 0.3f) surface = World.TerrainLayer.Surfaces[SurfaceType.Water];
                else if (noise.GetValue(pos) < 0.4f) surface = World.TerrainLayer.Surfaces[SurfaceType.Sand];
                else if (noise.GetValue(pos) < 2f) surface = World.TerrainLayer.Surfaces[SurfaceType.Soil];

                Tiles[pos].SetSurface(surface);
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
                Tiles[pos].OnWorldGeneration();
            }
        }

        // create animal herds with age according to new attributes attached to animals (spawn_commonness, spawn_surfaces, spawn_group_size)
    }
}
