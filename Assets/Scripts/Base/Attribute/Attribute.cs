using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// An Attribute that defines a certain aspect of a thing. Contains all information and calculations of its values.
/// Attributes can be dynamic (see NumberAttribute) or static (see StaticAttribute).
/// </summary>
public abstract class Attribute
{
    /// <summary>
    /// Display name of the attribute.
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Description of the attribute.
    /// </summary>
    public string Description { get; protected set; }

    /// <summary>
    /// Identifier of the attribute. Is unique per thing.
    /// </summary>
    public AttributeId Id { get; protected set; }

    /// <summary>
    /// Type of the attribute used to know how to handle it. Can be static or dynamic number.
    /// </summary>
    public AttributeType Type { get; protected set; }

    /// <summary>
    /// Category is solely used to better sort and organize different attributes within a thing. Has no effect of how the attribute is calculated.
    /// </summary>
    public AttributeCategory Category { get; protected set; }

    /// <summary>
    /// Thing that this attribute belongs to.
    /// </summary>
    public IThing Thing { get; private set; }


    public Attribute(IThing thing)
    {
        Thing = thing;
    }

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

/// <summary>
/// Dynamic attributes have its value calculated based on other attributes.
/// <br/>Static attributes have fixed values that are not influenced by other attributes.
/// </summary>
public enum AttributeType
{
    Dynamic,
    Static
}

/// <summary>
/// The category is solely used to better sort and organize attributes by what they do.
/// </summary>
public enum AttributeCategory
{
    Base, // only used for id, name and description
    General,
    Production
}
