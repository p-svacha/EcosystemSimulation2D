using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Movement : PercentageAttribute
{
    // Attribute Base
    public override string Name => "Movement";
    public override string Description => "Modifier of how capable an animal is at moving.";
    public override AttributeId Id => AttributeId.Movement;
    public override string Category => "Movement";

    // Individual
    private readonly OrganismBase Organism;

    public Att_Movement(OrganismBase organism)
    {
        Organism = organism;
    }
}
