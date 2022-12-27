using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallGrass : PlantBase
{
    // TileObject Base
    public override TileObjectId ObjectId => TileObjectId.TallGrass;
    protected override string ObjectName => "Tall Grass";
    protected override string ObjectDescription => "Edible Plants that grow randomly.";
    protected override string ObjectCategory => "Plants";


    protected override int HEALTH_BASE => 10;
    protected override SimulationTime MATURITY_AGE => new SimulationTime(0, 1, 0, 0);

    protected override NutrientType NUTRIENT_TYPE => NutrientType.Plant;
    protected override float NUTRIENT_VALUE_BASE => 20;
    protected override float EATING_DIFFICULTY => 0.4f;
}
