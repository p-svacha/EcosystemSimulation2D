using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_LandMovementSpeed : DynamicAttribute
{
    // Individual
    private readonly AnimalBase Animal;

    public Att_LandMovementSpeed(AnimalBase animal) : base(AttributeId.LandMovementSpeed, "Land Movement Speed", "Movement", AttributeType.Stat)
    {
        Animal = animal;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>
        {
            new AttributeModifier("Base Land Movement Speed", Animal.GetFloatAttribute(AttributeId.LandMovementSpeedBase), AttributeModifierType.BaseValue)
        };

        if (Animal.GetFloatAttribute(AttributeId.Movement) != 1f)
            mods.Add(new AttributeModifier("Movement", Animal.GetFloatAttribute(AttributeId.Movement), AttributeModifierType.Multiply));

        return mods;
    }
}
