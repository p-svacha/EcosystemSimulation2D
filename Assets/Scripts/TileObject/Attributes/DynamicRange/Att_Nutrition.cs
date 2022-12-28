using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Nutrition : DynamicRangeAttribute
{
    // Base
    public override string Name => "Nutrition";
    public override string Description => "Current and maximum amount of nutrition an animal has.";
    public override AttributeId Id => AttributeId.Nutrition;
    public override string Category => "Needs";

    protected override bool KeepValueRatio => false;

    // Individual
    private readonly AnimalBase Animal;

    public Att_Nutrition(AnimalBase animal)
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
