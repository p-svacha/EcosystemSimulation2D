using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface_Sand : SurfaceBase
{
    public override int Precedence => 30;
    public override SurfaceType Type => SurfaceType.Sand;
    protected override string SurfaceName => "Sand";
    protected override string SurfaceDescription => "Soft and slow to walk on.";
    protected override float MOVEMENT_COST => 4f;
    protected override bool REQUIRES_SWIMMING => false;
}
