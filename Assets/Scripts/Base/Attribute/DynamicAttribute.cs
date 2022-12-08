using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A DynamicAttribute is a float attribute that is calculated at runtime and fully derived by modifiers rather than a static value.
/// <br/>These modifiers are often attributes so a DynamicAttribute can is often dependent on other things and their attributes.
/// <br/>This class is abstract because all DynamicAttributes need to derive from this class to include their logic (how the value of the attribute is calculated) in GetValueModifiers.
/// </summary>
public abstract class DynamicAttribute : Attribute
{
    public override AttributeType Type => AttributeType.Dynamic;

    /// <summary>
    /// Returns the current value of this attribute, calculated from the base value and all modifiers.
    /// </summary>
    public override float GetValue()
    {
        List<DynamicAttributeModifier> modifiers = GetValueModifiers();
        if (modifiers.Where(x => x.Type == AttributeModifierType.BaseValue).Count() != 1) throw new System.Exception("A numeric attribute must have exactly 1 BaseValue modifier!");

        float value = modifiers.First(x => x.Type == AttributeModifierType.BaseValue).Value;
        foreach (DynamicAttributeModifier mod in modifiers.Where(x => x.Type == AttributeModifierType.Add)) value += mod.Value;
        foreach (DynamicAttributeModifier mod in modifiers.Where(x => x.Type == AttributeModifierType.Multiply)) value *= mod.Value;

        return value;
    }

    public override string GetValueString()
    {
        return GetValue().ToString();
    }

    /// <summary>
    /// Returns a list of all modifiers that skew the base value.
    /// Add modifiers are added first, then the multiply modifiers.
    /// </summary>
    public abstract List<DynamicAttributeModifier> GetValueModifiers();
}
