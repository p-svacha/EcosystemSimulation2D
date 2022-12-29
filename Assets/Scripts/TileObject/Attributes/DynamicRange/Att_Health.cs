using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Health : DynamicBarAttribute
{
    // Attribute Base
    protected override bool KeepValueRatio => true;

    // Individual
    private readonly TileObjectBase Object;

    public Att_Health(TileObjectBase obj) : base(AttributeId.Health, "Health", "General", AttributeType.Stat)
    {
        Object = obj;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>
        {
            new AttributeModifier("Base Health", Object.GetFloatAttribute(AttributeId.HealthBase), AttributeModifierType.BaseValue)
        };

        if (Object.GetFloatAttribute(AttributeId.Size) != 1f)
            mods.Add(new AttributeModifier("Size", Object.GetFloatAttribute(AttributeId.Size), AttributeModifierType.Multiply));

        return mods;
    }
}
