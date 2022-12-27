using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_WaterMovementSpeed : DynamicAttribute
{
    // Attribute Base
    public override string Name => "Water Movement Speed";
    public override string Description => "Actual speed at which an animal moves on water.";
    public override AttributeId Id => AttributeId.WaterMovementSpeed;
    public override string Category => "Movement";
    public override IThing Thing => Animal;

    // Individual
    private readonly AnimalBase Animal;

    public Att_WaterMovementSpeed(AnimalBase animal)
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
