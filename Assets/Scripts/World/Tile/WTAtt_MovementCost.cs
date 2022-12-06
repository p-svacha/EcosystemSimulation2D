using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WTAtt_MovementCost : DynamicAttribute
{
    // Attribute Base
    public override string Name => "Movement Cost";
    public override string Description => "How hard it is to traverse this tile.";
    public override AttributeId Id => AttributeId.MovementCost;
    public override string Category => "Movement";
    public override IThing Thing => Tile;

    // Individual
    private readonly WorldTile Tile;

    public WTAtt_MovementCost(WorldTile tile)
    {
        Tile = tile;
    }

    public override List<DynamicAttributeModifier> GetValueModifiers()
    {
        List<DynamicAttributeModifier> mods = new List<DynamicAttributeModifier>();

        mods.Add(new DynamicAttributeModifier("Base Value", 1f, AttributeModifierType.BaseValue));
        mods.Add(new DynamicAttributeModifier(Tile.Surface.Name + " Surface", Tile.Surface.MovementCost, AttributeModifierType.Multiply));

        return mods;
    }

    public override float GetValue()
    {
        float value = base.GetValue();
        if (value < 1) Debug.LogWarning("Movement Cost of a WorldTile should never be smaller than 1 because it breaks Pathfinding.");
        return value;
    }
}
