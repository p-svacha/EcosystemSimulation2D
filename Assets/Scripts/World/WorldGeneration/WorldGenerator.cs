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

    public static Dictionary<Vector2Int, WorldTile> GenerateBattleMap(World world, WorldGenerationInfo info)
    {
        World = world;
        Info = info;

        Tiles = new Dictionary<Vector2Int, WorldTile>();

        CreateTileInstances();
        CreateSurfaces();

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
        PerlinNoise noise = new PerlinNoise(scale: 0.05f);

        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                Surface surface = null;
                Vector2Int pos = new Vector2Int(x, y);
                if (noise.GetValue(pos) < 0.3f) surface = World.TerrainLayer.Surfaces[SurfaceType.Water];
                else if (noise.GetValue(pos) < 0.35f) surface = World.TerrainLayer.Surfaces[SurfaceType.Sand];
                else if (noise.GetValue(pos) < 2f) surface = World.TerrainLayer.Surfaces[SurfaceType.Soil];

                Tiles[pos].SetSurface(surface);
            }
        }
    }

    private static Surface GetRandomSurface()
    {
        return World.TerrainLayer.Surfaces.Values.ToList()[Random.Range(0, World.TerrainLayer.Surfaces.Values.Count)];
    }

    private static Vector2Int GetRandomPositionOnMap(int mapEdgeMargin = 5)
    {
        int x = Random.Range(mapEdgeMargin, MAP_WIDTH - mapEdgeMargin);
        int y = Random.Range(mapEdgeMargin, MAP_HEIGHT - mapEdgeMargin);
        return new Vector2Int(x, y);
    }

    private static Vector2Int GetRandomPositionAround(Vector2Int center, int maxDistance)
    {
        int x = -1;
        while (x < 0 || x >= MAP_WIDTH) x = Random.Range(center.x - maxDistance, center.x + maxDistance + 1);

        int y = -1;
        while (y < 0 || y >= MAP_HEIGHT) y = Random.Range(center.y - maxDistance, center.y + maxDistance + 1);

        return new Vector2Int(x, y);
    }
}
