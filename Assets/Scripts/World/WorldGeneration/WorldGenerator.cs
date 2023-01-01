using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WorldGenerator
{
    // Increase Simulation.TILE_UPDATE_POTS with bigger map sizes and performance keeps being good (16 for 200x200) (128 for 400x400)
    public static int MAP_SIZE => 300;

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
        for (int y = 0; y < MAP_SIZE; y++)
        {
            for (int x = 0; x < MAP_SIZE; x++)
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
        LayeredPerlinNoise heightNoise = new LayeredPerlinNoise(scale: 0.03f, numOctaves: 2);

        Noise voronoi = new VoronoiNoise(MAP_SIZE, 10, 3);
        Dictionary<int, int> voronoiCellElevations = new Dictionary<int, int>();

        int[,] heightmap = new int[MAP_SIZE + 1, MAP_SIZE + 1];
        for (int y = 0; y < MAP_SIZE + 1; y++)
        {
            for (int x = 0; x < MAP_SIZE + 1; x++)
            {
                float heightValue = heightNoise.GetValue(x, y);

                heightmap[x, y] = ApplyHeightOperation(heightValue);

                int cellId = (int)voronoi.GetValue(x, y);
                if (voronoiCellElevations.TryGetValue(cellId, out int elev))
                {
                    if (elev == 2) heightmap[x, y] = elev;
                    else heightmap[x, y] += elev;
                }
                else
                {
                    float rng = Random.value;
                    int newElev = 0;
                    if (rng < 0.6f) newElev = 0;
                    else if (rng < 0.9f) newElev = 1;
                    else if (rng < 2f) newElev = 2;

                    voronoiCellElevations.Add(cellId, newElev);

                    if (elev == 2) heightmap[x, y] = newElev;
                    else heightmap[x, y] += newElev;
                }
            }
        }

        // Store elevation values in tiles
        for (int y = 0; y < MAP_SIZE; y++)
        {
            for (int x = 0; x < MAP_SIZE; x++)
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
        LayeredPerlinNoise biomeNoise = new LayeredPerlinNoise(scale: 0.03f, numOctaves: 5);

        for (int y = 0; y < MAP_SIZE; y++)
        {
            for (int x = 0; x < MAP_SIZE; x++)
            {
                SurfaceBase surface = null;
                Vector2Int pos = new Vector2Int(x, y);
                WorldTile tile = World.Tiles[pos];

                if(tile.MaxElevation == 0)
                {
                    if (biomeNoise.GetValue(pos) < 0.35f && tile.MaxElevation == 0) surface = World.TerrainLayer.Surfaces[SurfaceId.Water];
                    else if (biomeNoise.GetValue(pos) < 0.45f) surface = World.TerrainLayer.Surfaces[SurfaceId.Sand];
                    else if (biomeNoise.GetValue(pos) < 2f) surface = World.TerrainLayer.Surfaces[SurfaceId.Soil];
                }
                else
                {
                    surface = World.TerrainLayer.Surfaces[SurfaceId.Soil];
                }

                

                World.Tiles[pos].SetSurface(surface);
            }
        }
    }

    private static void PopulateWorld()
    {
        for (int y = 0; y < MAP_SIZE; y++)
        {
            for (int x = 0; x < MAP_SIZE; x++)
            {
                Vector2Int pos = new(x, y);
                World.Tiles[pos].OnWorldGeneration();
            }
        }
    }
}
