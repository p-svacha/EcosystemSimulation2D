using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_PregnancyChance : DynamicAttribute
{
    // Attribute Base
    public override string Name => "Pregnancy Chance";
    public override string Description => "Actual chance per hour that an animal gets pregnant.";
    public override AttributeId Id => AttributeId.PregnancyChance;
    public override string Category => "Reproduction";
    public override IThing Thing => Animal;

    // Individual
    private readonly Animal Animal;

    public Att_PregnancyChance(Animal animal)
    {
        Animal = animal;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        if(Animal.Age < Animal.PregnancyMinAge)
            return new List<AttributeModifier>() { new AttributeModifier("Below minimum pregnancy age", 0, AttributeModifierType.BaseValue) };

        if (Animal.Age > Animal.PregnancyMaxAge)
            return new List<AttributeModifier>() { new AttributeModifier("Above maximum pregnancy age", 0, AttributeModifierType.BaseValue) };

        List<AttributeModifier> mods = new List<AttributeModifier>();
        mods.Add(new AttributeModifier("Base Chance", Animal.Attributes[AttributeId.PregnancyChanceBase].GetValue(), AttributeModifierType.BaseValue));
        if(Animal.HealthRatio < 1f)
            mods.Add(new AttributeModifier("Injured", Animal.HealthRatio, AttributeModifierType.Multiply));

        return mods;
    }
}
