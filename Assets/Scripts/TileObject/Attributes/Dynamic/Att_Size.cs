using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Size : PercentageAttribute
{
    // Attribute Base
    public override string Name => "Size";
    public override string Description => "How big an organism is compared to its default size.";
    public override AttributeId Id => AttributeId.Size;
    public override string Category => "General";

    // Individual
    private readonly OrganismBase Organism;

    public Att_Size(OrganismBase organism)
    {
        Organism = organism;
    }

    public override List<AttributeModifier> GetDynamicValueModifiers()
    {
        List<AttributeModifier> mods = base.GetDynamicValueModifiers();

        if (Organism.Age < Organism.MaturityAge) 
        {
            float ageRatio = Organism.Age.AbsoluteTime / Organism.MaturityAge;
            float sizeRatio = 0.35f + (0.65f * ageRatio); // 35%-100%
            mods.Add(new AttributeModifier("Age", sizeRatio, AttributeModifierType.Multiply));
        }

        return mods;
    }
}
