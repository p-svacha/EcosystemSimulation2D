using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface_Soil : SurfaceBase
{
    // Surface Base Required
    public override int Precedence => 50;
    public override SurfaceType Type => SurfaceType.Soil;
    protected override string SurfaceName => "Soil";
    protected override string SurfaceDescription => "Fertile terrain that will spawn plants quite often.";
    protected override float MOVEMENT_COST => 2f;
    protected override bool REQUIRES_SWIMMING => false;

    // Surface Base Optional
    protected override float TALL_GRASS_SPAWN_CHANCE => 0.0005f;
}
