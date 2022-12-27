using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Special kind of DynamicAttribute that always has 1 (=100%) as a base value and represents a percentage that can act as a multiplier modifier for other attributes.
/// </summary>
public abstract class PercentageAttribute : DynamicAttribute
{
    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>
        {
            new AttributeModifier("Base Value", 1f, AttributeModifierType.BaseValue)
        };

        return mods;
    }

    public override string GetValueString()
    {
        return (GetValue() * 100).ToString("F0") + "%";
    }
}
