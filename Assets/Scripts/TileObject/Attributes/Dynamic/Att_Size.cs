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
    public override IThing Thing => Organism;

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
            float ageRatio = Organism.Age / Organism.MaturityAge;
            float sizeRatio = 0.3f + (0.7f * ageRatio); // 30%-100%
            mods.Add(new AttributeModifier("Age", sizeRatio, AttributeModifierType.Multiply));
        }

        return mods;
    }
}