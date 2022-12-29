using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Movement : PercentageAttribute
{
    // Individual
    private readonly OrganismBase Organism;

    public Att_Movement(OrganismBase organism) : base(AttributeId.Movement, "Movement", "Movement", AttributeType.Intermediary)
    {
        Organism = organism;
    }
}
