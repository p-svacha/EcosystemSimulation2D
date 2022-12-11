using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_Pregnancy : StatusEffect
{
    // StatusEffect Base
    public override StatusEffectId Id => StatusEffectId.Pregnancy;
    public override string Name => "Pregnant";
    public override string Description => "Animal will produce offspring soon.";
    public override StatusDisplay Display => _Display;
    public override Dictionary<AttributeId, AttributeModifier> AttributeModifiers => new Dictionary<AttributeId, AttributeModifier>()
    {
        { AttributeId.HungerRate, new AttributeModifier(Name, 1.5f, AttributeModifierType.Multiply) },
        { AttributeId.LandMovementSpeed, new AttributeModifier(Name, 0.5f, AttributeModifierType.Multiply) },
        { AttributeId.PregnancyChance, new AttributeModifier(Name, 0f, AttributeModifierType.Overwrite, 100) }
    };

    // Individual
    private StatusDisplay _Display;

    public SE_Pregnancy()
    {
        _Display = new SD_Pregnancy(TileObject);
    }

    protected override bool IsEndConditionReached() => ((StaticAttribute<SimulationTime>)TileObject.Attributes[AttributeId.PregnancyProgress]).GetStaticValue().ExactTime > 0;
}
