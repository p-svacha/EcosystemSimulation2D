using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_LandMovementSpeed : DynamicAttribute
{
    // Attribute Base
    public override string Name => "Land Movement Speed";
    public override string Description => "Actual speed at which an animal moves on land.";
    public override AttributeId Id => AttributeId.LandMovementSpeed;
    public override string Category => "Movement";

    // Individual
    private readonly AnimalBase Animal;

    public Att_LandMovementSpeed(AnimalBase animal)
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
