using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Health : DynamicBarAttribute
{
    // Attribute Base
    public override AttributeId Id => AttributeId.Health;
    public override string Name => "Health";
    public override string Description => "Current and maximum amount of HP an object has.";
    public override string Category => "General";
    public override AttributeType Type => AttributeType.Stat;
    protected override bool KeepValueRatio => true;

    // Individual
    private readonly TileObjectBase Object;

    public Att_Health(TileObjectBase obj)
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
