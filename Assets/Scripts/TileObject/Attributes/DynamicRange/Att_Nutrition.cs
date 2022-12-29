using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Nutrition : DynamicBarAttribute
{
    // Attribute Base
    protected override bool KeepValueRatio => false;

    // Individual
    private readonly AnimalBase Animal;

    public Att_Nutrition(AnimalBase animal) : base(AttributeId.Nutrition, "Nutrition", "Needs", AttributeType.Stat)
    {
        Animal = animal;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>
        {
            new AttributeModifier("Base Nutrition", Animal.GetFloatAttribute(AttributeId.NutritionBase), AttributeModifierType.BaseValue)
        };

        if (Animal.GetFloatAttribute(AttributeId.Size) != 1f)
            mods.Add(new AttributeModifier("Size", Animal.GetFloatAttribute(AttributeId.Size), AttributeModifierType.Multiply));

        return mods;
    }
}
