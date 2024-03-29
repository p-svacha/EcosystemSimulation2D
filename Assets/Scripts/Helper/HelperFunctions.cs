using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public static string GetOrdinalSuffix(int num)
    {
        string number = num.ToString();
        if (number.EndsWith("11")) return "th";
        if (number.EndsWith("12")) return "th";
        if (number.EndsWith("13")) return "th";
        if (number.EndsWith("1")) return "st";
        if (number.EndsWith("2")) return "nd";
        if (number.EndsWith("3")) return "rd";
        return "th";
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

    public static T GetWeightedRandomElement<T>(Dictionary<T, float> weightDictionary)
    {
        float probabilitySum = weightDictionary.Sum(x => x.Value);
        float rng = Random.Range(0, probabilitySum);
        float tmpSum = 0;
        foreach (KeyValuePair<T, float> kvp in weightDictionary)
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
        int x = Random.Range(mapEdgeMargin, WorldGenerator.MAP_SIZE - mapEdgeMargin);
        int y = Random.Range(mapEdgeMargin, WorldGenerator.MAP_SIZE - mapEdgeMargin);
        return new Vector2Int(x, y);
    }

    public static TileObjectId GetRandomPlantForSurface(SurfaceId surface) => GetRandomObjectForSurface("Plants", surface);
    public static TileObjectId GetRandomAnimalForSurface(SurfaceId surface) => GetRandomObjectForSurface("Animals", surface);

    private static TileObjectId GetRandomObjectForSurface(string category, SurfaceId surface)
    {
        Dictionary<TileObjectId, float> weightedCandidates = new Dictionary<TileObjectId, float>();
        foreach(TileObjectBase obj in TileObjectFactory.DummyObjects.Values.Where(x => x.Category == category && x.SpawnSurfaces.Contains(surface)))
        {
            weightedCandidates.Add(obj.ObjectId, obj.Commonness);
        }
        return GetWeightedRandomElement(weightedCandidates);
    }

    #endregion

    #region Tilemap

    /// <summary>
    /// Returns a vector in a given direction with a given distance. Used for other helper functions.
    /// </summary>
    public static Vector2Int GetDirectionVector(Direction dir, int distance = 1)
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
    public static List<Vector2Int> GetAllPositionsWithinRange(Vector2Int source, int maxRange)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        for(int x = -maxRange; x < maxRange + 1; x++)
            for(int y = -maxRange; y < maxRange + 1; y++)
                positions.Add(source + new Vector2Int(x, y)); 

        return positions;
    }
    /// <summary>
    /// Returns a random position around a center point within a maximum range.
    /// </summary>
    public static Vector2Int GetRandomPositionWithinRange(Vector2Int source, int maxRange)
    {
        int x = -1;
        while (x < 0 || x >= WorldGenerator.MAP_SIZE) x = Random.Range(source.x - maxRange, source.x + maxRange + 1);

        int y = -1;
        while (y < 0 || y >= WorldGenerator.MAP_SIZE) y = Random.Range(source.y - maxRange, source.y + maxRange + 1);

        return new Vector2Int(x, y);
    }


    /// <summary>
    /// Returns all coordinates that have an exact distance to a source position.
    /// </summary>
    public static List<Vector2Int> GetAllPositionsWithRange(Vector2Int center, int range)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        for (int x = -range; x < range + 1; x++)
        {
            positions.Add(center + new Vector2Int(x, -range));
            positions.Add(center + new Vector2Int(x, range));
        }
        for (int y = -range; y < range + 1; y++)
        {
            positions.Add(center + new Vector2Int(-range + 1, y));
            positions.Add(center + new Vector2Int(range - 1, y));
        }
        return positions;
    }
    /// <summary>
    /// Returns a random position that has the exact range to a source position.
    /// </summary>
    public static Vector2Int GetRandomPositionWithRange(Vector2Int source, int range)
    {
        List<Vector2Int> allPositionsWithRange = GetAllPositionsWithRange(source, range);
        return allPositionsWithRange[Random.Range(0, allPositionsWithRange.Count)];
    }

    public static SurfaceBase GetRandomSurface()
    {
        return World.Singleton.TerrainLayer.Surfaces.Values.ToList()[Random.Range(0, World.Singleton.TerrainLayer.Surfaces.Values.Count)];
    }

    public static Vector2Int GetRandomPositionOnMap(int mapEdgeMargin = 5)
    {
        int x = Random.Range(mapEdgeMargin, WorldGenerator.MAP_SIZE - mapEdgeMargin);
        int y = Random.Range(mapEdgeMargin, WorldGenerator.MAP_SIZE - mapEdgeMargin);
        return new Vector2Int(x, y);
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

    /// <summary>
    /// Sets the Left, Right, Top and Bottom attribute of a RectTransform
    /// </summary>
    public static void SetRectTransformMargins(RectTransform rt, float left, float right, float top, float bottom)
    {
        rt.offsetMin = new Vector2(left, bottom);
        rt.offsetMax = new Vector2(-right, -top);
    }

    public static void SetLeft(RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    /// <summary>
    /// Creates a rectangular Image on the UI displaying a line from point A to point B
    /// </summary>
    public static GameObject CreateLine(Transform parent, Vector2 dotPositionA, Vector2 dotPositionB, Color color, float thickness)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(parent, false);
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchoredPosition = new Vector2((dotPositionB.x + dotPositionA.x) / 2, (dotPositionB.y + dotPositionA.y) / 2);
        rectTransform.sizeDelta = new Vector2(distance, thickness);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        //rectTransform.anchoredPosition = (dotPositionA - new Vector2(graphPositionX, graphPositionY)) + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        gameObject.transform.SetSiblingIndex(0);

        return gameObject;
    }

    private static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    #endregion

    #region Textures / Images

    /// <summary>
    /// Create a sprite from a file path to a .jpg or .png
    /// </summary>
    public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        Texture2D SpriteTexture = LoadTexture(FilePath);
        return Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);
    }

    /// <summary>
    /// Create a Texture2D from a file path to a .jpg or .png
    /// </summary>
    public static Texture2D LoadTexture(string FilePath)
    {
        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }

    #endregion

}
