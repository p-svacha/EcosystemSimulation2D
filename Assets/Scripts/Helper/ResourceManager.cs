using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager _Singleton;
    public static ResourceManager Singleton => _Singleton;

    [Header("Terrain Textures")]
    public Texture2D SoilTexture;
    public Texture2D SandTexture;
    public Texture2D WaterTexture;

    private Dictionary<SurfaceId, Texture2D> TerrainTextures;
    private Dictionary<SurfaceId, TileBase> TerrainTiles;

    [Header("Tilesets")]
    public Texture2D CliffTileset;
    public Texture2D SlopeTileset;

    [Header("Object Sprites")]
    public Sprite TallGrassSprite;
    public Sprite CreerSprite;
    public Sprite WofoxSprite;

    private Dictionary<TileObjectId, Sprite> TileObjectSprites;

    [Header("Status Display")]
    public World_StatusDisplay StatusDisplayWorldPrefab;
    public UI_StatusDisplay StatusDisplayUIPrefab;
    public Sprite SD_Malnutrition;
    public Sprite SD_Dehydration;
    public Sprite SD_Pregnancy;
    public Sprite SD_LowHealth;

    [Header("Materials")]
    public Material DefaultSpriteRenderMaterial;

    [Header("Special Tiles")]
    public TileBase WhiteTile;

    [Header("UI Basics")]
    public Sprite RemoveSprite;
    public Color TextButtonDefaultColor;
    public Color IconButtonDefaultColor;
    public Color IconButtonDisabledColor;
    public Color ButtonSelectedColor;
    public Tooltip Tooltip;
    public GameObject UiOverlaysContainer;

    [Header("Selection Window Content")]
    public UI_SWC_TileObjectBase SWC_TileObjectBase;
    public UI_SWC_AnimalBase SWC_AnimalBase;
    public UI_SimpleAttributeDisplay SimpleAttributeDisplay;
    

    void Awake()
    {
        _Singleton = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();

        // Terrain
        TerrainTextures = new Dictionary<SurfaceId, Texture2D>();
        TerrainTiles = new Dictionary<SurfaceId, TileBase>();

        TerrainTextures.Add(SurfaceId.Soil, SoilTexture);
        TerrainTextures.Add(SurfaceId.Sand, SandTexture);
        TerrainTextures.Add(SurfaceId.Water, WaterTexture);

        foreach (KeyValuePair<SurfaceId, Texture2D> kvp in TerrainTextures)
            TerrainTiles.Add(kvp.Key, TileGenerator.CreateTileFromTexture(kvp.Value));

        // Objects
        TileObjectSprites = new Dictionary<TileObjectId, Sprite>();
        TileObjectSprites.Add(TileObjectId.TallGrass, TallGrassSprite);
        TileObjectSprites.Add(TileObjectId.Creer, CreerSprite);
        TileObjectSprites.Add(TileObjectId.Wofox, WofoxSprite);
    }

    public Texture2D GetSurfaceTexture(SurfaceId type) => TerrainTextures[type];
    public TileBase GetSurfaceTile(SurfaceId type) => TerrainTiles[type];
    public Sprite GetTileObjectSprite(TileObjectId type) => TileObjectSprites[type];

}