using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A StatusDisplay is an informative display to show conditions on a TileObject.
/// <br/> It can appear as a World_StatusDisplay in world space or a UI_StatusDisplay in UI space.
/// </summary>
public abstract class StatusDisplay
{
    protected TileObject TileObject;

    public abstract string Name { get; }
    public abstract Sprite DisplaySprite { get; }
    public abstract bool DoShowDisplayValue { get; }

    public World_StatusDisplay WorldDisplayObject { get; private set; }

    public virtual string DisplayValue => "";

    public StatusDisplay(TileObject obj)
    {
        TileObject = obj;
    }

    public World_StatusDisplay CreateWorldDisplay(Transform parent, int index, int numElements)
    {
        if (WorldDisplayObject != null) return WorldDisplayObject;

        WorldDisplayObject = GameObject.Instantiate(ResourceManager.Singleton.StatusDisplayWorldPrefab, parent);
        WorldDisplayObject.Init(this, index, numElements);
        return WorldDisplayObject;
    }

    public UI_StatusDisplay CreateUIDisplay(Transform parent)
    {
        UI_StatusDisplay uiDisplay = GameObject.Instantiate(ResourceManager.Singleton.StatusDisplayUIPrefab, parent);
        uiDisplay.Init(this);
        return uiDisplay;
    }
}
