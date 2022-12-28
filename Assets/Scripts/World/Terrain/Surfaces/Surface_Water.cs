using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface_Water : SurfaceBase
{
    public override int Precedence => 10;
    public override SurfaceId SurfaceId => SurfaceId.Water;
    protected override string SurfaceName => "Water";
    protected override string SurfaceDescription => "Impassable for most things.";
    protected override float MOVEMENT_COST => 10f;
    protected override bool REQUIRES_SWIMMING => true;
    protected override float PLANT_GROW_CHANCE => 0f;
    protected override float PLANT_SPAWN_CHANCE => 0f;
    protected override float ANIMAL_SPAWN_CHANCE => 0f;
}
