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
    private readonly Animal Animal;

    public Att_MovementSpeed(Animal animal)
    {
        Animal = animal;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>();

        mods.Add(new AttributeModifier("Base Land Movement Speed", Animal.Attributes[AttributeId.LandMovementSpeedBase].GetValue(), AttributeModifierType.BaseValue));

        return mods;
    }
}
