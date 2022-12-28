using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act_WanderAround : Activity
{
    // Activity Base
    public override ActivityId Id => ActivityId.WanderAround;
    public override string DisplayString => "Wandering around aimlessly";

    // Individual
    public Act_WanderAround(AnimalBase animal) : base(animal) { }

    protected override void OnActivityStart()
    {
        SourceAnimal.SetMovementPath(Pathfinder.GetRandomPath(SourceAnimal, SourceAnimal.Tile, maxRange: 8));
    }

    public override void OnTick()
    {
        if (!SourceAnimal.IsMoving) End();
    }

    public override float GetUrgency()
    {
        return Random.value;
    }
}
