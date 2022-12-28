using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface_Sand : SurfaceBase
{
    public override int Precedence => 30;
    public override SurfaceId SurfaceId => SurfaceId.Sand;
    protected override string SurfaceName => "Sand";
    protected override string SurfaceDescription => "Soft and slow to walk on.";
    protected override float MOVEMENT_COST => 4f;
    protected override bool REQUIRES_SWIMMING => false;
    protected override float PLANT_GROW_CHANCE => 0f;
    protected override float PLANT_SPAWN_CHANCE => 0f;
    protected override float ANIMAL_SPAWN_CHANCE => 0f;
}
