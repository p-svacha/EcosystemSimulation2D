using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// An Attribute that defines a certain aspect of a thing. Contains all information and calculations of its values.
/// </summary>
public abstract class Attribute
{
    /// <summary>
    /// Identifier of the attribute. Has to be unique per thing.
    /// </summary>
    public abstract AttributeId Id { get; }

    /// <summary>
    /// Display name of the attribute.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Description of the attribute.
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// Type of the attribute used to know how to handle it. Can be static or dynamic number.
    /// </summary>
    public abstract AttributeType Type { get; }

    /// <summary>
    /// Category is solely used to better sort and organize different attributes within a thing. Has no effect of how the attribute is calculated.
    /// </summary>
    public abstract string Category { get; }

    /// <summary>
    /// Returns the numeric value of the attribute as a float.
    /// This is only supported by numeric attributes! (but it's in the base class for easier access that doesn't require casting)
    /// </summary>
    public abstract float GetValue();

    /// <summary>
    /// Returns the display value of this attribute.
    /// </summary>
    public abstract string GetValueString();
}

public enum AttributeType
{
    Static,
    Dynamic,
    DynamicRange,
}