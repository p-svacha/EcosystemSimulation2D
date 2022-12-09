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

    public override List<DynamicAttributeModifier> GetValueModifiers()
    {
        if(Animal.Age < Animal.PregnancyMinAge)
            return new List<DynamicAttributeModifier>() { new DynamicAttributeModifier("Below minimum pregnancy age", 0, AttributeModifierType.BaseValue) };

        if (Animal.Age > Animal.PregnancyMaxAge)
            return new List<DynamicAttributeModifier>() { new DynamicAttributeModifier("Above maximum pregnancy age", 0, AttributeModifierType.BaseValue) };

        if (Animal.IsPregnant)
            return new List<DynamicAttributeModifier>() { new DynamicAttributeModifier("Already pregnant", 0, AttributeModifierType.BaseValue) };

        List<DynamicAttributeModifier> mods = new List<DynamicAttributeModifier>();
        mods.Add(new DynamicAttributeModifier("Base Chance", Animal.Attributes[AttributeId.BasePregnancyChance].GetValue(), AttributeModifierType.BaseValue));
        if(Animal.HealthRatio < 1f)
            mods.Add(new DynamicAttributeModifier("Injured", Animal.HealthRatio, AttributeModifierType.Multiply));

        return mods;
    }
}
