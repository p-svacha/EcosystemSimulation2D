using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_NutrientValue : DynamicAttribute
{
    // Attribute Base
    public override string Name => "Nutrient Value";
    public override string Description => "Actual amount of nutrition an object provides at when being eaten from full health to 0.";
    public override AttributeId Id => AttributeId.NutrientValue;
    public override string Category => "Nutrition";
    public override IThing Thing => Thing;

    // Individual
    private readonly TileObjectBase Object;

    public Att_NutrientValue(TileObjectBase obj)
    {
        Object = obj;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>
        {
            new AttributeModifier("Base Nutrient Value", Object.GetFloatAttribute(AttributeId.NutrientValueBase), AttributeModifierType.BaseValue)
        };

        if (Object.GetFloatAttribute(AttributeId.Size) != 1f)
            mods.Add(new AttributeModifier("Size", Object.GetFloatAttribute(AttributeId.Size), AttributeModifierType.Multiply));

        return mods;
    }
}

