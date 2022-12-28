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
    protected override SimulationTime MATURITY_AGE => new SimulationTime(1, 0, 0, 0);

    protected override NutrientType NUTRIENT_TYPE => NutrientType.Plant;
    protected override float NUTRIENT_VALUE_BASE => 20;
    protected override float EATING_DIFFICULTY => 0.4f;

    // Individual
    public override void InitExisting()
    {
        int numYears = Random.Range(0, 4);
        int numMonths = Random.Range(0, SimulationTime.MonthsPerYear);
        int numDays = Random.Range(0, SimulationTime.DaysPerMonth);
        int numHours = Random.Range(0, SimulationTime.HoursPerDay);
        Age.SetTime(numYears, numMonths, numDays, numHours);
    }
}
