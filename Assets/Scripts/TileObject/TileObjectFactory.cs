using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Static class responsible for creating instances for specified TileObject types.
/// </summary>
public static class TileObjectFactory
{

    public static VisibleTileObjectBase CreateObject(TileObjectType type)
    {
        GameObject newObject = new GameObject(type.ToString());
        VisibleTileObjectBase tileObject = type switch
        {
            TileObjectType.TallGrass => newObject.AddComponent<TallGrass>(),
            TileObjectType.Creer => newObject.AddComponent<Creer>(),
            _ => throw new System.Exception("TileObjectType " + type.ToString() + " not handled in TileObjectFactory."),
        };

#pragma 
        SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();
        renderer.sprite = ResourceManager.Singleton.GetTileObjectSprite(type);
        renderer.drawMode = SpriteDrawMode.Sliced; // Used to be able to set size without changing transform.scale
        renderer.sortingLayerName = "Object";
        renderer.material = ResourceManager.Singleton.DefaultSpriteRenderMaterial;

        newObject.AddComponent<BoxCollider2D>();

        tileObject.Init();
        tileObject.LateInit();
        return tileObject;
    }


    public static List<TileObjectType> GetAllObjectTypes()
    {
        return System.Enum.GetValues(typeof(TileObjectType)).Cast<TileObjectType>().ToList();
    }
}