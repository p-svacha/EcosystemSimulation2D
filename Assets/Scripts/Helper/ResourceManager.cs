using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager _Singleton;
    public static ResourceManager Singleton => _Singleton;

    [Header("Surface Textures")]
    public Texture2D SoilTexture;
    public Texture2D SandTexture;
    public Texture2D WaterTexture;

    private Dictionary<SurfaceType, Texture2D> TerrainTextures;
    private Dictionary<SurfaceType, TileBase> TerrainTiles;

    [Header("Object Sprites")]
    public Sprite TallGrassSprite;
    public Sprite CreerSprite;

    private Dictionary<TileObjectType, Sprite> TileObjectSprites;

    [Header("Status Display")]
    public StatusDisplayObject StatusDisplayWorldPrefab;
    public UI_StatusDisplay StatusDisplayUIPrefab;
    public Sprite SD_Malnutrition;
    public Sprite SD_Dehydration;
    public Sprite SD_Pregnancy;

    [Header("Materials")]
    public Material DefaultSpriteRenderMaterial;

    [Header("Special Tiles")]
    public TileBase WhiteTile;

    [Header("UI Icons")]
    public Sprite RemoveSprite;

    [Header("UI Colors")]
    public Color TextButtonDefaultColor;
    public Color IconButtonDefaultColor;
    public Color IconButtonDisabledColor;
    public Color ButtonSelectedColor;

    [Header("Selection Window Content")]
    public UI_SWC_TileObjectBase SWC_TileObjectBase;
    public UI_SWC_AnimalBase SWC_AnimalBase;
    

    void Awake()
    {
        // Terrain
        TerrainTextures = new Dictionary<SurfaceType, Texture2D>();
        TerrainTiles = new Dictionary<SurfaceType, TileBase>();

        TerrainTextures.Add(SurfaceType.Soil, SoilTexture);
        TerrainTextures.Add(SurfaceType.Sand, SandTexture);
        TerrainTextures.Add(SurfaceType.Water, WaterTexture);

        foreach (KeyValuePair<SurfaceType, Texture2D> kvp in TerrainTextures)
            TerrainTiles.Add(kvp.Key, TileGenerator.CreateTileFromTexture(kvp.Value));

        // Objects
        TileObjectSprites = new Dictionary<TileObjectType, Sprite>();
        TileObjectSprites.Add(TileObjectType.TallGrass, TallGrassSprite);
        TileObjectSprites.Add(TileObjectType.Creer, CreerSprite);
    }

    private void Start()
    {
        _Singleton = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
    }

    public Texture2D GetSurfaceTexture(SurfaceType type) => TerrainTextures[type];
    public TileBase GetSurfaceTile(SurfaceType type) => TerrainTiles[type];
    public Sprite GetTileObjectSprite(TileObjectType type) => TileObjectSprites[type];

}