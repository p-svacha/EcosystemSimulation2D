using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A DynamicAttribute is a float attribute that is calculated at runtime and fully derived by modifiers rather than a static value.
/// <br/> The calculated value is cached for performance reasons. By default the cache is cleared at the start of each frame.
/// <br/> These modifiers are often attributes so a DynamicAttribute can is often dependent on other things and their attributes.
/// <br/> This class is abstract because all DynamicAttributes need to derive from this class to include their logic (how the value of the attribute is calculated) in GetValueModifiers.
/// </summary>
public abstract class DynamicAttribute : Attribute
{
    public override AttributeType Type => AttributeType.Dynamic;

    /// <summary>
    /// List of all modifiers that come from StatusEffects on the parent thing. Elements get added and removed from this list by the StatusEffects themselves.
    /// <br/> Does not get operated on each frame and is therefore more performant than dynamic modifiers.
    /// </summary>
    private List<AttributeModifier> StatusEffectModifiers = new List<AttributeModifier>();

    /// <summary>
    /// Returns the current value of this attribute, calculated from all modifiers (dynamic modifiers + StatusEffect modifiers).
    /// </summary>
    public override float GetValue()
    {
        // Check for overwrite
        AttributeModifier overwriteModifier = GetActiveOverwriteModifier();
        if (overwriteModifier != null) return overwriteModifier.Value;

        // Default calculation
        List<AttributeModifier> modifiers = GetAllModifiers();

        if (modifiers.Where(x => x.Type == AttributeModifierType.BaseValue).Count() != 1) throw new System.Exception("A numeric attribute must have exactly 1 BaseValue modifier!");

        float value = modifiers.First(x => x.Type == AttributeModifierType.BaseValue).Value;
        foreach (AttributeModifier mod in modifiers.Where(x => x.Type == AttributeModifierType.Add)) value += mod.Value;
        foreach (AttributeModifier mod in modifiers.Where(x => x.Type == AttributeModifierType.Multiply)) value *= mod.Value;

        return value;
    }

    public override string GetValueString()
    {
        return GetValue().ToString();
    }

    /// <summary>
    /// Returns all modifiers of this attribute.
    /// </summary>
    /// <returns></returns>
    public List<AttributeModifier> GetAllModifiers()
    {
        List<AttributeModifier> modifiers = new List<AttributeModifier>();
        modifiers.AddRange(GetDynamicValueModifiers());
        modifiers.AddRange(StatusEffectModifiers);
        return modifiers;
    }

    /// <summary>
    /// Returns a list of all modifiers for this specific attribute need to be calculated each frame / don't come from StatusEffects. Must contain a BaseValue modifier.
    /// </summary>
    public abstract List<AttributeModifier> GetDynamicValueModifiers();

    /// <summary>
    /// Returns the active overwrite modifier if there is one. Else returns null.
    /// </summary>
    public AttributeModifier GetActiveOverwriteModifier()
    {
        List<AttributeModifier> overwriteModifiers = GetAllModifiers().Where(x => x.Type == AttributeModifierType.Overwrite).ToList();
        if (overwriteModifiers.Count > 0)
        {
            AttributeModifier highestModifier = overwriteModifiers.OrderBy(x => x.Priority).First();
            return highestModifier;
        }
        return null;
    }

    public void AddStatusEffectModifier(AttributeModifier modifier)
    {
        StatusEffectModifiers.Add(modifier);
    }

    public void RemoveStatusEffectModifier(AttributeModifier modifier)
    {
        StatusEffectModifiers.Remove(modifier);
    }
}
