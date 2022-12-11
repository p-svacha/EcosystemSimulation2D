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
    public override IThing Thing => Thing;

    // Individual
    private readonly Animal Animal;

    public Att_HungerRate(Animal animal)
    {
        Animal = animal;
    }


    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>();

        mods.Add(new AttributeModifier("Base Hunger Rate", Animal.Attributes[AttributeId.HungerRateBase].GetValue(), AttributeModifierType.BaseValue));

        return mods;
    }
}
