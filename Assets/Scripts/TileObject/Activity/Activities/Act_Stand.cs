using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act_Stand : Activity
{
    // Activity Base
    public override ActivityId Id => ActivityId.Standing;
    public override string DisplayString => "Standing";

    // Individual
    private const float MIN_STAND_HOURS = 0.2f;
    private const float MAX_STAND_HOURS = 2f;
    private float StandDelay;
    private float StandTime;

    public Act_Stand(AnimalBase animal) : base(animal) { }

    protected override void OnActivityStart()
    {
        StandTime = Random.Range(MIN_STAND_HOURS, MAX_STAND_HOURS);
        StandDelay = 0f;
    }

    public override void OnTick()
    {
        StandDelay += Simulation.Singleton.TickTime;
        if (StandDelay >= StandTime) End();
    }

    public override float GetUrgency()
    {
        return Random.value;
    }
}
