using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface_Soil : SurfaceBase
{
    // Surface Base Required
    public override int Precedence => 50;
    public override SurfaceId SurfaceId => SurfaceId.Soil;
    protected override string SurfaceName => "Soil";
    protected override string SurfaceDescription => "Fertile terrain that will spawn plants quite often.";
    protected override float MOVEMENT_COST => 2f;
    protected override bool REQUIRES_SWIMMING => false;
    protected override float PLANT_GROW_CHANCE => 0.0001f;
    protected override float PLANT_SPAWN_CHANCE => 0.01f;
    protected override float ANIMAL_SPAWN_CHANCE => 0.0006f;
}
