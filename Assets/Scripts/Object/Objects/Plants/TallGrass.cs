using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallGrass : VisibleTileObject
{
    // TileObject Base
    protected override string ObjectName => "Tall Grass";
    protected override string ObjectDescription => "Edible Plants that grow randomly.";
    public override TileObjectType Type => TileObjectType.TallGrass;
    protected override int MAX_HEALTH => 10;

    protected override NutrientType NUTRIENT_TYPE => NutrientType.Plant;
    protected override float NUTRIENT_VALUE => 20;
    protected override float EATING_DIFFICULTY => 0.4f;
}
