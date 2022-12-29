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
    /// <summary>
    /// List of all modifiers that come from StatusEffects on the parent thing. Elements get added and removed from this list by the StatusEffects themselves.
    /// <br/> Does not get operated on each frame and is therefore more performant than dynamic modifiers.
    /// </summary>
    private List<AttributeModifier> StatusEffectModifiers = new List<AttributeModifier>();

    public override float GetValue()
    {
        return CalculateCurrentValue();
    }

    /// <summary>
    /// Returns the current value of this attribute, calculated from all modifiers (dynamic modifiers + StatusEffect modifiers).
    /// <br/> Code is not beautiful but as performant as possible.
    /// </summary>
    protected float CalculateCurrentValue()
    {
        List<AttributeModifier> overwriteMods = new List<AttributeModifier>();
        List<AttributeModifier> addMods = new List<AttributeModifier>();
        List<AttributeModifier> multiplyMods = new List<AttributeModifier>();
        AttributeModifier baseValueMod = null;

        foreach (AttributeModifier mod in GetDynamicValueModifiers())
        {
            switch(mod.Type)
            {
                case AttributeModifierType.Overwrite: overwriteMods.Add(mod); break;
                case AttributeModifierType.Add: addMods.Add(mod); break;
                case AttributeModifierType.Multiply: multiplyMods.Add(mod); break;
                case AttributeModifierType.BaseValue: baseValueMod = mod; break;
            };
        }
        foreach (AttributeModifier mod in StatusEffectModifiers)
        {
            switch (mod.Type)
            {
                case AttributeModifierType.Overwrite: overwriteMods.Add(mod); break;
                case AttributeModifierType.Add: addMods.Add(mod); break;
                case AttributeModifierType.Multiply: multiplyMods.Add(mod); break;
                case AttributeModifierType.BaseValue: baseValueMod = mod; break;
            };
        }

        // Check for overwrite
        if(overwriteMods.Count == 1) return overwriteMods[0].Value;
        if (overwriteMods.Count > 1) return overwriteMods.OrderBy(x => x.Priority).First().Value;

        // Default calculation
        List<AttributeModifier> modifiers = GetAllModifiers();

        float value = baseValueMod.Value;
        foreach (AttributeModifier mod in addMods) value += mod.Value;
        foreach (AttributeModifier mod in multiplyMods) value *= mod.Value;

        return value;
    }

    public override string GetValueString() => GetValue().ToString();

    public override string GetValueBreakdownText()
    {
        string text = "";

        // Overwrite active
        AttributeModifier overwriteModifier = GetActiveOverwriteModifier();
        if (overwriteModifier != null)
        {
            text += overwriteModifier.Source + ":\t" + overwriteModifier.Value + " (Forced)\n";
        }

        // Default calculation
        else
        {
            List<AttributeModifier> modifiers = GetAllModifiers();

            AttributeModifier baseMod = modifiers.First(x => x.Type == AttributeModifierType.BaseValue);
            text += baseMod.Source + ":\t" + baseMod.Value + "\n";

            foreach (AttributeModifier mod in modifiers.Where(x => x.Type == AttributeModifierType.Add))
                text += "\n" + mod.Source + ":\t" + (mod.Value >= 0 ? "+" : "") + mod.Value;
            foreach (AttributeModifier mod in modifiers.Where(x => x.Type == AttributeModifierType.Multiply))
                text += "\n" + mod.Source + ":\tx" + mod.Value;
            text += "\n\nFinal Value:\t" + GetValue();
        }

        return text;
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
