using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this to an object to test different noise types in the editor.
/// </summary>
public class NoiseTester : MonoBehaviour
{
    public GameObject TestPlane;

    [Header("General")]
    public NoiseType NoiseId;
    public float AreaSize = 300;

    [Header("Perlin")]
    public float PerlinScale = 0.01f;

    [Header("Layered Perlin")]
    public float LayeredPerlinScale = 0.01f;
    public int LayeredPerlinOctaves = 6;
    public float LayeredPerlinPersistance = 0.5f;
    public float LayeredPerlinLacunarity = 2f;

    [Header("Ridged Multifractal")]
    public float RmfFrequency = 0.01f;
    public float RmfLacunarity = 2f;
    public int RmfOctaves = 6;

    [Header("Voronoi")]
    public int VoronoiNumPoints = 100;
    public float VoronoiPValue = 2;


    public void DisplayNoise()
    {
        Noise noise = GetNoise();

        Dictionary<int, Color> distinctColors = new Dictionary<int, Color>();

        TestPlane.transform.localScale = new Vector3(AreaSize / 10, 1, AreaSize / 10);
        TestPlane.transform.position = new Vector3(-TestPlane.transform.localScale.x * 10, -TestPlane.transform.localScale.z * 10, 0f);

        Texture2D tex = new Texture2D((int)AreaSize, (int)AreaSize);
        tex.filterMode = FilterMode.Point;
        for(int y = 0; y < tex.height; y++)
        {
            for(int x = 0; x < tex.width; x++)
            {
                float noiseX = (((float)x / (float)tex.width) * (TestPlane.transform.localScale.x * 10));
                float noiseY = (((float)y / (float)tex.height) * (TestPlane.transform.localScale.z * 10));
                float value = (noise.GetValue(noiseX, noiseY));
                //Debug.Log("Value at " + noiseX + "/" + noiseY + "/" + 1f + ": " + rmfn.GetValue(noiseX, noiseY, 1f));

                NoiseTestDisplayType displayType = GetDisplayType();
                if(displayType == NoiseTestDisplayType.Linear) tex.SetPixel(x, y,  new Color(value, value, value));
                if(displayType == NoiseTestDisplayType.Distinct)
                {
                    int intValue = (int)value;
                    if(distinctColors.TryGetValue(intValue, out Color c)) tex.SetPixel(x, y, c);
                    else
                    {
                        //Color newColor = new Color(0f + Random.value * 0.5f, 0.5f + Random.value * 0.5f, 0.6f + Random.value * 0.4f); // blue-greenish colors
                        float grayScale = Random.value;
                        Color newColor = new Color(grayScale, grayScale, grayScale);
                        distinctColors.Add(intValue, newColor);
                        tex.SetPixel(x, y, newColor);
                    }
                }
            }
        }
        tex.Apply();

        TestPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = tex;
    }

    private Noise GetNoise()
    {
        switch(NoiseId)
        {
            case NoiseType.Perlin: 
                return new PerlinNoise(PerlinScale);

            case NoiseType.LayeredPerlin:
                return new LayeredPerlinNoise(LayeredPerlinScale, LayeredPerlinOctaves, LayeredPerlinPersistance, LayeredPerlinLacunarity);

            case NoiseType.MultifractalRidge:
                return new RidgedMultifractalNoise(RmfFrequency, RmfLacunarity, RmfOctaves);

            case NoiseType.Voronoi:
                return new VoronoiNoise(AreaSize, VoronoiNumPoints, VoronoiPValue);

            default:
                throw new System.Exception("Noise type not handled");
        }
    }

    private NoiseTestDisplayType GetDisplayType()
    {
        return NoiseId switch
        {
            NoiseType.Perlin => NoiseTestDisplayType.Linear,
            NoiseType.LayeredPerlin => NoiseTestDisplayType.Linear,
            NoiseType.MultifractalRidge => NoiseTestDisplayType.Linear,
            NoiseType.Voronoi => NoiseTestDisplayType.Distinct,
            _ => throw new System.Exception("Noise type not handled")
        };
    }

    public enum NoiseType
    {
        Perlin,
        LayeredPerlin,
        MultifractalRidge,
        Voronoi
    }

    public enum NoiseTestDisplayType
    {
        Linear, // Range (0,1) is shown in grayscale values, where 0 is black and 1 is white.
        Distinct // Different absolute int values (1,2,3,4,...) are shown as different colors.
    }
}
