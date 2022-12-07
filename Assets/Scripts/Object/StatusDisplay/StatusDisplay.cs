using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A StatusDisplay is an informative display that appears when a specified condition in an objects attributes are met.
/// </summary>
public abstract class StatusDisplay
{
    public abstract string Name { get; }
    public abstract Sprite DisplaySprite { get; }
    public abstract bool DoShowDisplayValue { get; }

    public StatusDisplayObject WorldDisplayObject { get; private set; }

    /// <summary>
    /// Requirement check if the status display should be displayed.
    /// </summary>
    public abstract bool ShouldShow();

    public abstract string GetDisplayValue();

    public StatusDisplayObject CreateWorldDisplay(Transform parent, int index, int numElements)
    {
        if (WorldDisplayObject != null) return WorldDisplayObject;

        WorldDisplayObject = GameObject.Instantiate(ResourceManager.Singleton.StatusDisplayPrefab, parent);
        WorldDisplayObject.Init(this, index, numElements);
        return WorldDisplayObject;
    }
}