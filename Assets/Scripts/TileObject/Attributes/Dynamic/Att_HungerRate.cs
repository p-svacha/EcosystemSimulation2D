using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_HungerRate : DynamicAttribute
{
    // Attribute Base
    public override string Name => "Hunger Rate";
    public override string Description => "Actual amount at which the nutrition of an animal drops per hour.";
    public override AttributeId Id => AttributeId.HungerRate;
    public override string Category => "Needs";

    // Individual
    private readonly AnimalBase Animal;

    public Att_HungerRate(AnimalBase animal)
    {
        Animal = animal;
    }


    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>();

        mods.Add(new AttributeModifier("Base Hunger Rate", Animal.GetFloatAttribute(AttributeId.HungerRateBase), AttributeModifierType.BaseValue));

        if (Animal.GetFloatAttribute(AttributeId.Size) != 1f)
            mods.Add(new AttributeModifier("Size", Animal.GetFloatAttribute(AttributeId.Size), AttributeModifierType.Multiply));

        return mods;
    }
}
