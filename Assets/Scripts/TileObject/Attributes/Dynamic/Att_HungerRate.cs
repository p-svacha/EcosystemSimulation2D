using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_HungerRate : DynamicAttribute
{
    // Individual
    private readonly AnimalBase Animal;

    public Att_HungerRate(AnimalBase animal) : base(AttributeId.HungerRate, "Hunger Rate", "Needs", AttributeType.Stat)
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
