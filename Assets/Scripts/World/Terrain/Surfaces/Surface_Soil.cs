using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface_Soil : Surface
{
    // Surface
    public override int Precedence => 50;
    public override SurfaceType Type => SurfaceType.Soil;
    protected override string SurfaceName => "Soil";
    protected override string SurfaceDescription => "Fertile terrain that will spawn plants quite often.";
    protected override float MOVEMENT_COST => 2f;
    protected override bool REQUIRES_SWIMMING => false;

    // Individual
    private const float TALL_GRASS_SPAWN_CHANCE = 0.003f;

    public Surface_Soil(World world) : base(world)
    {
        _Attributes.Add(AttributeId.TallGrassSpawnChance, new StaticAttribute<float>(this, AttributeId.TallGrassSpawnChance, AttributeCategory.Production, "Tall Grass Spawn Chance", "Chance to spawn tall grass on this tile per hour", TALL_GRASS_SPAWN_CHANCE));
    }
}
