using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Size : PercentageAttribute
{
    // Individual
    private readonly OrganismBase Organism;

    public Att_Size(OrganismBase organism) : base(AttributeId.Size, "Size", "General", AttributeType.Intermediary)
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
