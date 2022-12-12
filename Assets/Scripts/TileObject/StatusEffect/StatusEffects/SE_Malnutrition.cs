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
    private StatusDisplay _Display;
    private Dictionary<AttributeId, AttributeModifier> _Modifiers;

    public SE_Malnutrition()
    {
        _Display = new SD_Malnutrition();
        _Modifiers = new Dictionary<AttributeId, AttributeModifier>()
        {
            { AttributeId.HungerRate, new AttributeModifier(Name, 1.2f, AttributeModifierType.Multiply) }
        };
    }

    protected override bool IsEndConditionReached() => (TileObject as AnimalBase).Malnutrition == 0;
    protected override void OnTick()
    {
        TileObject.ChangeAttribute(AttributeId.Health, -((TileObject as AnimalBase).Malnutrition * Simulation.Singleton.TickTime), 0f, TileObject.MaxHealth);
    }
}
