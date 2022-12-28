using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Static class responsible for creating instances for specified TileObject types.
/// </summary>
public static class TileObjectFactory
{
    /// <summary>
    /// Contains one non-simulated instance of each tile object. Used for easy access of base attributes of tile objects.
    /// </summary>
    public static Dictionary<TileObjectId, TileObjectBase> DummyObjects { get; private set; }

    /// <summary>
    /// Initializes the TileObjectFactory by creating one instance of each tile object as a dummy object.
    /// </summary>
    public static void Init()
    {
        DummyObjects = new Dictionary<TileObjectId, TileObjectBase>();
        foreach (TileObjectId id in System.Enum.GetValues(typeof(TileObjectId)))
        {
            TileObjectBase dummyObject = CreateObject(id);
            dummyObject.transform.position = new Vector3(-666, -666, 0);
            DummyObjects.Add(id, dummyObject);
        }
    }

    public static VisibleTileObjectBase CreateObject(TileObjectId type, bool isNew = true)
    {
        GameObject newObject = new GameObject(type.ToString());

        VisibleTileObjectBase tileObject = type switch
        {
            TileObjectId.TallGrass => newObject.AddComponent<TallGrass>(),
            TileObjectId.Creer => newObject.AddComponent<Creer>(),
            TileObjectId.Wofox => newObject.AddComponent<Wofox>(),
            _ => throw new System.Exception("TileObjectType " + type.ToString() + " not handled in TileObjectFactory."),
        };

        SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();
        renderer.sprite = ResourceManager.Singleton.GetTileObjectSprite(type);
        renderer.drawMode = SpriteDrawMode.Sliced; // Used to be able to set size without changing transform.scale
        renderer.sortingLayerName = "Object";
        renderer.material = ResourceManager.Singleton.DefaultSpriteRenderMaterial;

        newObject.AddComponent<BoxCollider2D>();

        tileObject.Init();
        if (isNew) tileObject.InitNew();
        else tileObject.InitExisting();
        return tileObject;
    }
}
