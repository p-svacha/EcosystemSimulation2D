using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Everything in the game should implement this class. It is used to have a generic and modular structure over all objects.
/// </summary>
public interface IThing
{
    /// <summary>
    /// Unique key to identify what kind of thing it is.
    /// </summary>
    public ThingId ThingId {get;}

    /// <summary>
    /// Name of the thing.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Description of the thing.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Attributes describing the behaviour of the thing.
    /// </summary>
    public Dictionary<AttributeId, Attribute> Attributes { get; }

    /// <summary>
    /// The UI Panel that will be loaded into a SelectionWindow when this thing is selected. Can be null.
    /// </summary>
    public UI_SelectionWindowContent SelectionWindowContent { get; }
}

public enum ThingId
{
    Tile,
    Surface,
    Object
}
