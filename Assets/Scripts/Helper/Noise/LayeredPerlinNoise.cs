using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredPerlinNoise : Noise
{
    public float Scale { get; private set; }
    public int NumOctaves { get; private set; }
    public float Persistance { get; private set; }
    public float Lacunarity { get; private set; }

    private List<PerlinNoise> Layers;

    public LayeredPerlinNoise(float scale = 0.5f, int numOctaves = 6, float persistance = 0.5f, float lacunarity = 2f)
    {
        Scale = scale;
        NumOctaves = numOctaves;
        Persistance = persistance;
        Lacunarity = lacunarity;

        Layers = new List<PerlinNoise>();
        float frequency = 1f;
        for(int i = 0; i < NumOctaves; i++)
        {
            Layers.Add(new PerlinNoise(frequency * Scale));
            frequency *= Lacunarity;
        }
    }

    public override float GetValue(float x, float y)
    {
        float value = 0f;
        float amplitude = 1f;
        for(int i = 0; i < NumOctaves; i++)
        {
            float layerValue = Layers[i].GetValue(x, y) * 2 - 1; // range (-1,1)
            value += amplitude * layerValue;
            amplitude *= Persistance;
        }
        value = value / 2 + 0.5f; // map back to range (0,1)
        return value;
    }
}
