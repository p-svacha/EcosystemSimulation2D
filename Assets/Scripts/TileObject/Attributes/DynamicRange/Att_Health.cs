using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Health : DynamicRangeAttribute
{
    // Base
    public override string Name => "Health";
    public override string Description => "Current and maximum amount of HP an object has.";
    public override AttributeId Id => AttributeId.Health;
    public override string Category => "General";
    public override IThing Thing => Object;

    protected override bool KeepValueRatio => true;

    // Individual
    private readonly TileObjectBase Object;

    public Att_Health(TileObjectBase obj)
    {
        Object = obj;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>();

        mods.Add(new AttributeModifier("Base Health", Object.GetFloatAttribute(AttributeId.HealthBase), AttributeModifierType.BaseValue));

        if(Object.GetFloatAttribute(AttributeId.Size) != 1f)
            mods.Add(new AttributeModifier("Size", Object.GetFloatAttribute(AttributeId.Size), AttributeModifierType.Multiply));

        return mods;
    }
}
