using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_WaterMovementSpeed : DynamicAttribute
{
    // Individual
    private readonly AnimalBase Animal;

    public Att_WaterMovementSpeed(AnimalBase animal) : base(AttributeId.WaterMovementSpeed, "Water Movement Speed", "Movement", AttributeType.Stat)
    {
        Animal = animal;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>
        {
            new AttributeModifier("Base Water Movement Speed", Animal.GetFloatAttribute(AttributeId.WaterMovementSpeedBase), AttributeModifierType.BaseValue)
        };

        if (Animal.GetFloatAttribute(AttributeId.Movement) != 1f)
            mods.Add(new AttributeModifier("Movement", Animal.GetFloatAttribute(AttributeId.Movement), AttributeModifierType.Multiply));

        return mods;
    }
}
