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
    private AnimalBase Animal;
    private StatusDisplay _Display;
    private Dictionary<AttributeId, AttributeModifier> _Modifiers;
    public SimulationTime PregnancyProgress { get; private set; }

    public SE_Pregnancy()
    {
        _Display = new SD_Pregnancy(this);
        _Modifiers = new Dictionary<AttributeId, AttributeModifier>()
        {
            { AttributeId.HungerRate, new AttributeModifier(Name, 1.5f, AttributeModifierType.Multiply) },
            { AttributeId.Movement, new AttributeModifier(Name, 0.6f, AttributeModifierType.Multiply) },
            { AttributeId.Size, new AttributeModifier(Name, 1.1f, AttributeModifierType.Multiply) },
            { AttributeId.PregnancyChance, new AttributeModifier(Name, 0f, AttributeModifierType.Overwrite, priority: 100) }
        };
    }

    protected override void OnAdd()
    {
        Animal = TileObject as AnimalBase;
        PregnancyProgress = new SimulationTime();
    }
    protected override bool IsEndConditionReached() => PregnancyProgress >= (TileObject as AnimalBase).PregnancyDuration;
    protected override void OnTick()
    {
        PregnancyProgress.IncreaseTime(Simulation.Singleton.TickTime);
    }

    protected override void OnEnd()
    {
        Animal.GiveBirth();
        PregnancyProgress.Reset();
    }
}
