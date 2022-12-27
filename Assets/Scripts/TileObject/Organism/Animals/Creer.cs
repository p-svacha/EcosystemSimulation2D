using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creer : AnimalBase
{
    // TileObject
    protected override string ObjectName => "Creer";
    protected override string ObjectDescription => "A peaceful animal that eats plants.";
    public override TileObjectType Type => TileObjectType.Creer;

    // Animal Base
    protected override float VISION_RANGE => 10f;
    protected override float LAND_MOVEMENT_SPEED_BASE => 0.2f;
    protected override float WATER_MOVEMENT_SPEED_BASE => 0f;
    protected override int HEALTH_BASE => 120;
    protected override SimulationTime MATURITY_AGE => new SimulationTime(0, 2, 0, 0);
    
    protected override float HUNGER_RATE_BASE => 2f;
    protected override float NUTRITION_BASE => 80;
    protected override List<NutrientType> DIET => new List<NutrientType>() { NutrientType.Plant };
    protected override float EATING_SPEED => 0.9f;

    protected override SimulationTime PREGNANCY_MAX_AGE => new SimulationTime(20, 0, 0, 0);
    protected override float PREGNANCY_CHANCE_BASE => 0.01f;
    protected override SimulationTime PREGNANCY_DURATION => new SimulationTime(0, 0, 3, 0);
    protected override int MIN_NUM_OFFSPRING => 1;
    protected override int MAX_NUM_OFFSPRING => 2;

    // Individual


}