using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_NutrientValue : DynamicAttribute
{
    // Individual
    private readonly TileObjectBase Object;

    public Att_NutrientValue(TileObjectBase obj) : base(AttributeId.NutrientValue, "Nutrient Value", "Nutrition", AttributeType.Stat)
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

