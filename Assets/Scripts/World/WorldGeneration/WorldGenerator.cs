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
        CreateElevation();
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

    private static void CreateElevation()
    {
        // Create a heightmap
        LayeredPerlinNoise heightNoise = new LayeredPerlinNoise(scale: 0.05f, numOctaves: 2);
        LayeredPerlinNoise mesaNoise = new LayeredPerlinNoise(scale: 0.01f, numOctaves: 2);

        int[,] heightmap = new int[MAP_WIDTH + 1, MAP_HEIGHT + 1];
        for (int y = 0; y < MAP_HEIGHT + 1; y++)
        {
            for (int x = 0; x < MAP_WIDTH + 1; x++)
            {
                float heightValue = heightNoise.GetValue(x, y);

                heightmap[x, y] = ApplyHeightOperation(heightValue);
                if (mesaNoise.GetValue(x, y) > 0.7f) heightmap[x, y]++;
            }
        }

        // Validate height map so there is no elevation step > 2
        bool isValid = false;
        while(!isValid)
        {
            isValid = true;
            for (int y = 1; y < MAP_HEIGHT; y++)
            {
                for (int x = 1; x < MAP_WIDTH; x++)
                {
                    int value = heightmap[x, y];
                    foreach (Vector2Int v in HelperFunctions.GetAdjacentCoordinates(new Vector2Int(x, y)))
                    {
                        if (heightmap[v.x, v.y] < value - 2)
                        {
                            heightmap[v.x, v.y] = value - 2;
                            isValid = false;
                        }
                        if(heightmap[v.x,v.y] > value + 2)
                        {
                            heightmap[v.x, v.y] = value + 2;
                            isValid = false;
                        }
                    }
                }
            }
        }

        // Store elevation values in tiles
        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                World.Tiles[pos].SetElevation(new int[] { heightmap[x, y + 1], heightmap[x + 1, y + 1], heightmap[x + 1, y], heightmap[x, y] });
            }
        }
    }

    /// <summary>
    /// Returns the elevation of a corner derived from the heightmap value (0-1)
    /// </summary>
    private static int ApplyHeightOperation(float initialHeightValue)
    {
        float value = Mathf.Clamp(initialHeightValue, 0f, 1f);

        // Skew it linearly to range
        float minValue = 0;
        float maxValue = 1.6f;
        value = value * (maxValue - minValue) + minValue;

        // Don't change negative
        if (value <= 0) return (int)value;

        // Apply operation to positive
        value = value * value;
        int intValue =  (int)value;
        return intValue;
    }

    private static void CreateSurfaces()
    {
        LayeredPerlinNoise waterNoise = new LayeredPerlinNoise(scale: 0.03f, numOctaves: 5);
        LayeredPerlinNoise biomeNoise = new LayeredPerlinNoise(scale: 0.03f, numOctaves: 5);

        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                SurfaceBase surface = null;
                Vector2Int pos = new Vector2Int(x, y);
                WorldTile tile = World.Tiles[pos];

                if (waterNoise.GetValue(pos) < 0.3f && tile.MaxElevation == 0) surface = World.TerrainLayer.Surfaces[SurfaceId.Water];
                else if (biomeNoise.GetValue(pos) < 0.2f) surface = World.TerrainLayer.Surfaces[SurfaceId.Sand];
                else if (biomeNoise.GetValue(pos) < 2f) surface = World.TerrainLayer.Surfaces[SurfaceId.Soil];

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
