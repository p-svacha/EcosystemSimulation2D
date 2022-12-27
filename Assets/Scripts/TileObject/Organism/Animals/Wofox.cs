using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wofox : AnimalBase
{
    // TileObject
    public override TileObjectId ObjectId => TileObjectId.Wofox;
    protected override string ObjectName => "Wofox";
    protected override string ObjectDescription => "A wooly descendant of the wox that will eat anything and shed wool.";
    protected override string ObjectCategory => "Animals";
    

    // Animal Base
    protected override float VISION_RANGE => 20f;
    protected override float LAND_MOVEMENT_SPEED_BASE => 0.5f;
    protected override float WATER_MOVEMENT_SPEED_BASE => 0.1f;
    protected override int HEALTH_BASE => 90;
    protected override SimulationTime MATURITY_AGE => new SimulationTime(0, 3, 0, 0);

    protected override float HUNGER_RATE_BASE => 1f;
    protected override float NUTRITION_BASE => 90;
    protected override List<NutrientType> DIET => new List<NutrientType>() { NutrientType.Plant, NutrientType.Meat };
    protected override float EATING_SPEED => 2f;

    protected override SimulationTime PREGNANCY_MAX_AGE => new SimulationTime(20, 0, 0, 0);
    protected override float PREGNANCY_CHANCE_BASE => 0.01f;
    protected override SimulationTime PREGNANCY_DURATION => new SimulationTime(0, 0, 5, 0);
    protected override int MIN_NUM_OFFSPRING => 1;
    protected override int MAX_NUM_OFFSPRING => 3;

    // Individual


}
