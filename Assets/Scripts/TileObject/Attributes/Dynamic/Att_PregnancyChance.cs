using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_PregnancyChance : DynamicAttribute
{
    // Individual
    private readonly AnimalBase Animal;

    public Att_PregnancyChance(AnimalBase animal) : base(AttributeId.PregnancyChance, "Pregnancy Chance", "Reproduction", AttributeType.Stat)
    {
        Animal = animal;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        if(Animal.Age < Animal.MaturityAge)
            return new List<AttributeModifier>() { new AttributeModifier("Below maturity age", 0, AttributeModifierType.BaseValue) };

        if (Animal.Age > Animal.PregnancyMaxAge)
            return new List<AttributeModifier>() { new AttributeModifier("Above maximum pregnancy age", 0, AttributeModifierType.BaseValue) };

        List<AttributeModifier> mods = new List<AttributeModifier>
        {
            new AttributeModifier("Base Chance", Animal.GetFloatAttribute(AttributeId.PregnancyChanceBase), AttributeModifierType.BaseValue)
        };

        if (Animal.Health.Ratio < 1f)
            mods.Add(new AttributeModifier("Injured", Animal.Health.Ratio, AttributeModifierType.Multiply));

        return mods;
    }
}
