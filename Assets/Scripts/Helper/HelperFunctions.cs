using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class HelperFunctions
{
    #region Math

    public static int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    #endregion

    #region Strings

    public static string ListToString<T>(List<T> list)
    {
        string txt = "";
        foreach (T elem in list) txt += elem.ToString() + ", ";
        txt = txt.TrimEnd(' ');
        txt = txt.TrimEnd(',');
        return txt;
    }

    #endregion

    #region Random

    public static T GetWeightedRandomElement<T>(Dictionary<T, int> weightDictionary)
    {
        int probabilitySum = weightDictionary.Sum(x => x.Value);
        int rng = Random.Range(0, probabilitySum);
        int tmpSum = 0;
        foreach (KeyValuePair<T, int> kvp in weightDictionary)
        {
            tmpSum += kvp.Value;
            if (rng < tmpSum) return kvp.Key;
        }
        throw new System.Exception();
    }

    /// <summary>
    /// Returns a random number in a gaussian distribution. About 2/3 of generated numbers are within the standard deviation of the mean.
    /// </summary>
    public static float NextGaussian(float mean, float standard_deviation)
    {
        return mean + NextGaussian() * standard_deviation;
    }

    private static float NextGaussian()
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);
        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }

    /// <summary>
    /// Returns a random direction used for adjacency.
    /// </summary>
    public static Direction GetRandomAdjacentDirection()
    {
        List<Direction> dirs = GetAdjacentDirections();
        return dirs[Random.Range(0, dirs.Count)];
    }

    public static Vector2Int GetRandomPositionInWorld(World world, int mapEdgeMargin)
    {
        int x = Random.Range(mapEdgeMargin, WorldGenerator.MAP_WIDTH - mapEdgeMargin);
        int y = Random.Range(mapEdgeMargin, WorldGenerator.MAP_HEIGHT - mapEdgeMargin);
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// Returns a random position around a point within a given range.
    /// </summary>
    public static Vector2Int GetRandomPositionInRange(Vector2Int center, int range)
    {
        List<Vector2Int> allPositionsInRange = GetAllPositionsInRange(center, range);
        return allPositionsInRange[Random.Range(0, allPositionsInRange.Count)];
    }

    #endregion

    #region Tilemap

    /// <summary>
    /// Returns a vector in a given direction with a given distance. Used for other helper functions.
    /// </summary>
    private static Vector2Int GetDirectionVector(Direction dir, int distance = 1)
    {
        switch (dir)
        {
            case Direction.N: return new Vector2Int(0, 1) * distance;
            case Direction.NE: return new Vector2Int(1, 1) * distance;
            case Direction.E: return new Vector2Int(1, 0) * distance;
            case Direction.SE: return new Vector2Int(1, -1) * distance;
            case Direction.S: return new Vector2Int(0, -1) * distance;
            case Direction.SW: return new Vector2Int(-1, -1) * distance;
            case Direction.W: return new Vector2Int(-1, 0) * distance;
            case Direction.NW: return new Vector2Int(-1, 1) * distance;
        }
        throw new System.Exception("Direction not handled");
    }

    /// <summary>
    /// Returns a list of coordinates for all adjacent positions from a source position. Does not include source position.
    /// </summary>
    public static List<Vector2Int> GetAdjacentCoordinates(Vector2Int source, int distance = 1)
    {
        List<Vector2Int> coordinates = new List<Vector2Int>();
        foreach(Direction dir in GetAdjacentDirections()) coordinates.Add(GetPositionInDirection(source, dir, distance));
        return coordinates;
    }

    /// <summary>
    /// Returns coordinates that are in a given direction and distance from a source position.
    /// </summary>
    public static Vector2Int GetPositionInDirection(Vector2Int pos, Direction dir, int distance = 1)
    {
        return pos + GetDirectionVector(dir, distance);
    }

    /// <summary>
    /// Returns the direction of which the position lies from the source. Must be adjacent, else returns Direction.None
    /// </summary>
    public static Direction GetDirectionTo(Vector2Int source, Vector2Int pos)
    {
        foreach (Direction dir in GetAdjacentDirections())
        {
            if (GetPositionInDirection(source, dir) == pos) return dir;
        }
        return Direction.None;
    }

    /// <summary>
    /// Returns a list of all directions used for adjacency.
    /// </summary>
    /// <returns></returns>
    public static List<Direction> GetAdjacentDirections()
    {
        return new List<Direction>() { Direction.N, Direction.NE, Direction.E, Direction.SE, Direction.S, Direction.SW, Direction.W, Direction.NW };
    }

    /// <summary>
    /// Returns all coordinates around a center point within a maximum range. Source position is included.
    /// </summary>
    public static List<Vector2Int> GetAllPositionsInRange(Vector2Int center, int range)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        for(int x = -range; x < range + 1; x++)
            for(int y = -range; y < range + 1; y++)
                positions.Add(center + new Vector2Int(x, y)); 

        return positions;
    }

    /// <summary>
    /// Returns all coordinates that have an exact distance to a source position.
    /// </summary>
    public static List<Vector2Int> GetAllPositionsWithRange(Vector2Int center, int range)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        for (int x = -range; x < range + 1; x++)
        {
            for (int y = -range; y < range + 1; y++)
            {
                if (x > -range && x < range && y > -range && y < range) continue;
                positions.Add(center + new Vector2Int(x, y));
            }
        }

        return positions;
    }

    #endregion

    #region UI

    /// <summary>
    /// Destroys all children of a GameObject immediately.
    /// </summary>
    public static void DestroyAllChildredImmediately(GameObject obj)
    {
        int numChildren = obj.transform.childCount;
        for (int i = 0; i < numChildren; i++) GameObject.DestroyImmediate(obj.transform.GetChild(0).gameObject);
    }

    public static Sprite Texture2DToSprite(Texture2D tex)
    {
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    #endregion

}
