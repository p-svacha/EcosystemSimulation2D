using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_MovementSpeed : DynamicAttribute
{
    // Attribute Base
    public override string Name => "Land Movement Speed";
    public override string Description => "Actual speed at which an animal moves on land.";
    public override AttributeId Id => AttributeId.LandMovementSpeed;
    public override string Category => "General";
    public override IThing Thing => Animal;

    // Individual
    private readonly AnimalBase Animal;

    public Att_MovementSpeed(AnimalBase animal)
    {
        Animal = animal;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>
        {
            new AttributeModifier("Base Land Movement Speed", Animal.GetFloatAttribute(AttributeId.LandMovementSpeedBase), AttributeModifierType.BaseValue)
        };

        return mods;
    }
}
