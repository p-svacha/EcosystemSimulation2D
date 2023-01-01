using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_MovementCost : DynamicAttribute
{
    // Individual
    private readonly WorldTile Tile;

    public Att_MovementCost(WorldTile tile) : base(AttributeId.MovementCost, "Movement Cost", "Movement", AttributeType.Stat)
    {
        Tile = tile;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = new List<AttributeModifier>();

        mods.Add(new AttributeModifier("Base Value", 1f, AttributeModifierType.BaseValue));
        mods.Add(new AttributeModifier(Tile.Surface.Name + " Surface", Tile.Surface.MovementCost, AttributeModifierType.Multiply));

        if (Tile.ElevationType == TileElevationType.Slope)
            mods.Add(new AttributeModifier("Slope", 1.5f, AttributeModifierType.Multiply));

        return mods;
    }

    public override float GetValue()
    {
        float value = base.GetValue();
        if (value < 1) Debug.LogWarning("Movement Cost of a WorldTile should never be smaller than 1 because it breaks Pathfinding.");
        return value;
    }
}
