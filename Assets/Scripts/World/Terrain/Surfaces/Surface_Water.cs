using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface_Water : Surface
{
    public override int Precedence => 10;
    public override SurfaceType Type => SurfaceType.Water;
    protected override string SurfaceName => "Water";
    protected override string SurfaceDescription => "Impassable for most things.";
    protected override float MOVEMENT_COST => 10f;
    protected override bool REQUIRES_SWIMMING => true;

    public Surface_Water(World world) : base(world) { }
}
