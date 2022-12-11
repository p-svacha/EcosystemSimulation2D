using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pregnancy is a reproduction status effect. It lasts as long as the pregancyduration of an animal.
/// While active, it increases hunger and reduces movement speed.
/// When done the animal will produce offspring.
/// </summary>
public class SE_Pregnancy : StatusEffect
{
    // StatusEffect Base
    public override StatusEffectId Id => StatusEffectId.Pregnancy;
    public override string Name => "Pregnant";
    public override string Description => "Animal will produce offspring soon.";
    public override StatusDisplay Display => _Display;
    public override Dictionary<AttributeId, AttributeModifier> AttributeModifiers => _Modifiers;

    // Individual
    private StatusDisplay _Display;
    private Dictionary<AttributeId, AttributeModifier> _Modifiers;

    public SE_Pregnancy()
    {
        _Display = new SD_Pregnancy();
        _Modifiers = new Dictionary<AttributeId, AttributeModifier>()
        {
            { AttributeId.HungerRate, new AttributeModifier(Name, 1.5f, AttributeModifierType.Multiply) },
            { AttributeId.LandMovementSpeed, new AttributeModifier(Name, 0.5f, AttributeModifierType.Multiply) },
            { AttributeId.PregnancyChance, new AttributeModifier(Name, 0f, AttributeModifierType.Overwrite, 100) }
        };
    }

    protected override bool IsEndConditionReached() => (TileObject as Animal).PregnancyProgress.ExactTime >= (TileObject as Animal).PregnancyDuration.ExactTime;
    protected override void OnTick()
    {
        (TileObject as Animal).PregnancyProgress.IncreaseTime(Simulation.Singleton.TickTime);
    }

    protected override void OnEnd()
    {
        (TileObject as Animal).GiveBirth();
        (TileObject as Animal).PregnancyProgress.Reset();
    }
}
