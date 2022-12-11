using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An AttributeModifier is a building block of a DynamicAttribute.
/// <br/>A DynamicAttribute can consist of different AttributeModifiers, one of which always has to have the Type BaseValue.
/// <br/>Other Modifiers will overwrite, be added or multiplied to the BaseValue Modifier.
/// </summary>
public class AttributeModifier
{
    public string Source { get; private set; }
    public float Value { get; private set; }
    public AttributeModifierType Type { get; private set; }
    /// <summary>
    /// Priority is only used for overwrite modifiers. The Overwrite Modifier with the highest priority value will overwrite all others.
    /// </summary>
    public int Priority { get; private set; }

    public AttributeModifier(string source, float value, AttributeModifierType type, int priority = 0)
    {
        Source = source;
        Value = value;
        Type = type;
        Priority = priority;
    }
}

public enum AttributeModifierType
{
    BaseValue, // only one per attribute allowed
    Add,
    Multiply,
    Overwrite
}
