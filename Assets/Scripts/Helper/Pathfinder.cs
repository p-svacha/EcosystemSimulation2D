using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinder
{
    #region Public Methods

    // A* algorithm implementation. https://pavcreations.com/tilemap-based-a-star-algorithm-implementation-in-unity-game/
    /// <summary>
    /// Returns the optimal path for the given animal from a source tile to a target tile.
    /// <br/> Returned path contains both source and target tile.
    /// <br/> Returns null if no path is found.
    /// </summary>
    public static List<WorldTile> GetPath(AnimalBase animal, WorldTile from, WorldTile to)
    {
        if (to == null) return null;
        if (from == to) return null;
        if (!to.IsPassable(animal)) return null;

        List<WorldTile> openList = new List<WorldTile>() { from }; // tiles that are queued for searching
        List<WorldTile> closedList = new List<WorldTile>(); // tiles that have already been searched

        Dictionary<WorldTile, float> gCosts = new Dictionary<WorldTile, float>();
        Dictionary<WorldTile, float> fCosts = new Dictionary<WorldTile, float>();
        Dictionary<WorldTile, WorldTile> previousTiles = new Dictionary<WorldTile, WorldTile>();

        gCosts.Add(from, 0);
        fCosts.Add(from, gCosts[from] + GetHCost(from, to));

        while (openList.Count > 0)
        {
            WorldTile currentTile = GetLowestFCostTile(openList, fCosts);
            if (currentTile == to) // Reached goal
            {
                return GetFinalPath(to, previousTiles);
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach (WorldTile neighbour in currentTile.GetAdjacentTiles())
            {
                if (closedList.Contains(neighbour)) continue;
                if (!neighbour.IsPassable(animal)) continue;

                float tentativeGCost = gCosts[currentTile] + GetCCost(animal, currentTile, neighbour);
                if (!gCosts.ContainsKey(neighbour) || tentativeGCost < gCosts[neighbour])
                {
                    previousTiles[neighbour] = currentTile;
                    gCosts[neighbour] = tentativeGCost;
                    fCosts[neighbour] = tentativeGCost + GetHCost(neighbour, to);

                    if (!openList.Contains(neighbour)) openList.Add(neighbour);
                }
            }
        }

        // Out of tiles -> no path
        return null;
    }

    /// <summary>
    /// Returns a random path that is no longer than the given range.
    /// <br/> The path is not optimized towards a tile, it's just randomly wandering.
    /// <br/> Returns null if no path is found.
    /// </summary>
    public static List<WorldTile> GetRandomPath(AnimalBase animal, WorldTile source, int maxRange)
    {
        List<WorldTile> path = new List<WorldTile> { source };
        int chosenRange = Random.Range(1, maxRange + 1);

        WorldTile currentTile = source;
        
        while(path.Count < chosenRange + 1)
        {
            List<WorldTile> nextTileCandidates = new List<WorldTile>();
            foreach (WorldTile neighbour in currentTile.GetAdjacentTiles())
            {
                if (path.Contains(neighbour)) continue;
                if (!neighbour.IsPassable(animal)) continue;

                nextTileCandidates.Add(neighbour);
            }
            if (nextTileCandidates.Count == 0) return path;

            WorldTile chosenNextTile = nextTileCandidates[Random.Range(0, nextTileCandidates.Count)];
            path.Add(chosenNextTile);
            currentTile = chosenNextTile;
        }

        return path;
    }

    /// <summary>
    /// Returns all tiles can be reached by traversing an exact amount of tiles (range) from a source position.
    /// <br/> Checks shortest path and tiles that can reached earlier than range are not included.
    /// </summary>
    public static List<WorldTile> GetAllReachablePositionsWithRange(AnimalBase animal, WorldTile center, int range)
    {
        int currentRange = 0;
        List<WorldTile> currentRangeTiles = new List<WorldTile>() { center };
        List<WorldTile> allCheckedTiles = new List<WorldTile>() { center };
        while (currentRange < range)
        {
            List<WorldTile> newRangeTiles = new List<WorldTile>();
            foreach (WorldTile tile in currentRangeTiles)
            {
                foreach (WorldTile neighbour in tile.GetAdjacentTiles())
                {
                    if (allCheckedTiles.Contains(neighbour)) continue;
                    if (!neighbour.IsPassable(animal)) continue;

                    newRangeTiles.Add(neighbour);
                    allCheckedTiles.Add(neighbour);
                }
            }

            if (newRangeTiles.Count == 0) return null; // No tiles reachable with exactly this range.

            currentRangeTiles = newRangeTiles;

            currentRange++;
        }

        return currentRangeTiles;
    }

    #endregion

    /// <summary>
    /// Assumed cost of that path. This function is not allowed to overestimate the cost. The real cost must be >= this cost.
    /// </summary>
    private static float GetHCost(WorldTile from, WorldTile to)
    {
        return Vector2.Distance(from.WorldPosition, to.WorldPosition);
    }

    /// <summary>
    /// Real cost of going from one node to another. Must be greater or equal to assumed cost.
    /// </summary>
    private static float GetCCost(AnimalBase animal, WorldTile from, WorldTile to)
    {
        float value = (0.5f * (from.GetMovementCost(animal))) + (0.5f * (to.GetMovementCost(animal))) * Vector2.Distance(from.WorldPosition, to.WorldPosition);
        return value;
    }

    private static WorldTile GetLowestFCostTile(List<WorldTile> list, Dictionary<WorldTile, float> fCosts)
    {
        float lowestCost = float.MaxValue;
        WorldTile lowestCostTile = list[0];
        foreach (WorldTile tile in list)
        {
            if (fCosts[tile] < lowestCost)
            {
                lowestCostTile = tile;
                lowestCost = fCosts[tile];
            }
        }
        return lowestCostTile;
    }

    private static List<WorldTile> GetFinalPath(WorldTile to, Dictionary<WorldTile, WorldTile> previousTiles)
    {
        List<WorldTile> path = new List<WorldTile>();
        path.Add(to);
        WorldTile currentTile = to;
        while (previousTiles.ContainsKey(currentTile))
        {
            path.Add(previousTiles[currentTile]);
            currentTile = previousTiles[currentTile];
        }
        path.Reverse();
        return path;
    }


    #region Visualization

    private static Color PathVisualizationColor = new Color(1f, 1f, 1f, 0.5f);
    private static float PathVisualizationWidth = 0.05f;
    public static GameObject GetPathVisualization(List<WorldTile> path)
    {
        GameObject pathVisualizer = new GameObject("PathVisualizer");

        LineRenderer line = pathVisualizer.AddComponent<LineRenderer>();
        line.sortingLayerName = "BM_CharacterLayer";
        line.material = ResourceManager.Singleton.DefaultSpriteRenderMaterial;
        line.startWidth = PathVisualizationWidth;
        line.endWidth = PathVisualizationWidth;
        line.startColor = PathVisualizationColor;
        line.endColor = PathVisualizationColor;
        line.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++) line.SetPosition(i, path[i].WorldPosition);

        return pathVisualizer;
    }

    #endregion
}
