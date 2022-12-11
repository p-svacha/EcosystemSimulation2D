using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_Malnutrition : StatusEffect
{
    // StatusEffect Base
    public override StatusEffectId Id => StatusEffectId.Malnutrition;
    public override string Name => "Malnutrition";
    public override string Description => "Animal needs more food for a while.";
    public override StatusDisplay Display => _Display;
    public override Dictionary<AttributeId, AttributeModifier> AttributeModifiers => new Dictionary<AttributeId, AttributeModifier>()
    {
        { AttributeId.HungerRate, new AttributeModifier(Name, 1.2f, AttributeModifierType.Multiply) }
    };

    // Individual
    private StatusDisplay _Display;

    public SE_Malnutrition()
    {
        _Display = new SD_Malnutrition(TileObject);
    }

    protected override bool IsEndConditionReached() => TileObject.Attributes[AttributeId.Malnutrition].GetValue() > 0f;
}
