using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A type of noise that splits a set area into distinct regions.
/// </summary>
public class VoronoiNoise : Noise
{
    private List<Vector2> PointLocations;
    /// <summary>
    /// The p-Value determines the shape of the voronoi cells.
    /// <br/> p < 1 will result in very flat cells.
    /// <br/> p = 1 (Manhattan distance) will result in cells consisting only of straight and diagonal (45°) lines.
    /// <br/> p = 2 (Euclidian distance) will result in a standard voronoi.
    /// <br/> p = 3 (Minkowski distance) will result in cells having a some curves in it.
    /// <br/> p > 3 will result in elongated diamond shapes.
    /// </summary>
    private float PValue;

    public VoronoiNoise(float areaSize, int numPoints, float pValue)
    {
        PValue = pValue;
        PointLocations = new List<Vector2>();
        for(int i = 0; i < numPoints; i++)
        {
            PointLocations.Add(new Vector2(Random.value * areaSize, Random.value * areaSize));
        }
    }


    public override float GetValue(float x, float y)
    {
        Vector2 pos = new Vector2(x, y);
        int regionIndex = -1;
        float shortestDistance = float.MaxValue;

        for(int i = 0; i < PointLocations.Count; i++)
        {
            float distance = GetDistance(pos, PointLocations[i]);
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                regionIndex = i;
            }
        }
        return regionIndex;
    }

    private float GetDistance(Vector2 point1, Vector2 point2)
    {
        float diffX = Mathf.Abs(point1.x - point2.x);
        float diffY = Mathf.Abs(point1.y - point2.y);
        return (float)Mathf.Pow(Mathf.Pow(diffX, PValue) + Mathf.Pow(diffY, PValue), 1 / PValue);
    }
}
