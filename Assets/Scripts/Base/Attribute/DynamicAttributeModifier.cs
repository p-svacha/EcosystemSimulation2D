using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A DynamicAttributeModifier is a building block of a DynamicAttribute.
/// <br/>A DynamicAttribute can consist of different DynamicAttributeModifiers, one of which always has to have the Type BaseValue.
/// <br/>Other Modifiers will be added or multiplied to the BaseValue Modifier.
/// </summary>
public class DynamicAttributeModifier
{
    public string Description { get; private set; }
    public float Value { get; private set; }
    public AttributeModifierType Type { get; private set; }

    public DynamicAttributeModifier(string description, float value, AttributeModifierType type)
    {
        Description = description;
        Value = value;
        Type = type;
    }
}

public enum AttributeModifierType
{
    BaseValue, // only one per attribute allowed
    Add,
    Multiply
}
