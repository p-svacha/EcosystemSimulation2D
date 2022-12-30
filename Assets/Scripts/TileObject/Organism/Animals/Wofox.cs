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
    protected override float PREGNANCY_CHANCE_BASE => 0.001f;
    protected override SimulationTime PREGNANCY_DURATION => new SimulationTime(0, 0, 5, 0);
    protected override int NUM_OFFSPRING_MIN => 1;
    protected override int NUM_OFFSPRING_MAX => 3;

    protected override List<SurfaceId> SPAWN_SURFACES => new List<SurfaceId>() { SurfaceId.Soil };
    protected override int SPAWN_GROUP_SIZE_MIN => 1;
    protected override int SPAWN_GROUP_SIZE_MAX => 1;
    protected override float COMMONNESS => 6f;

    // Individual


}
