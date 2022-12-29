using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Malnutrition is a status effect that increases hunger and decreases health while active.
/// </summary>
public class SE_Malnutrition : StatusEffect
{
    // StatusEffect Base
    public override StatusEffectId Id => StatusEffectId.Malnutrition;
    public override string Name => "Malnutrition";
    public override string Description => "Animal needs more food for a while.";
    public override StatusDisplay Display => _Display;
    public override Dictionary<AttributeId, AttributeModifier> AttributeModifiers => _Modifiers;

    // Individual
    private AnimalBase Animal;
    private StatusDisplay _Display;
    private Dictionary<AttributeId, AttributeModifier> _Modifiers;
    public float MalnutritionAdvancement; // Animal loses this much health per hour

    public SE_Malnutrition()
    {
        _Display = new SD_Malnutrition(this);
        _Modifiers = new Dictionary<AttributeId, AttributeModifier>()
        {
            { AttributeId.HungerRate, new AttributeModifier(Name, 1.2f, AttributeModifierType.Multiply) }
        };
    }

    protected override void OnAdd()
    {
        Animal = TileObject as AnimalBase;
    }
    protected override bool IsEndConditionReached() => MalnutritionAdvancement <= 0;
    protected override void OnTick()
    {
        if (Animal.Nutrition.Value == 0) MalnutritionAdvancement += Animal.HungerRate * Simulation.Singleton.TickTime; // Increase malnutrition
        else MalnutritionAdvancement -= Animal.HungerRate * Simulation.Singleton.TickTime; // Decrease malnutrition

        if (MalnutritionAdvancement < 0) MalnutritionAdvancement = 0;

        Animal.Health.ChangeValue(-(MalnutritionAdvancement * Simulation.Singleton.TickTime));
    }
}
