using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_SpawnSurfaces : StaticAttribute<List<SurfaceId>>
{
    // Attribute Base
    private const AttributeId ID = AttributeId.SpawnSurfaces;
    private const string CATEGORY = "Spawn";
    private const string NAME = "Spawn Surfaces";

    // Individual
    public List<SurfaceId> Surfaces { get; private set; }

    public Att_SpawnSurfaces(List<SurfaceId> surfaces) : base(ID, NAME, CATEGORY, surfaces)
    {
        Surfaces = surfaces;
    }

    public override string GetValueString() => HelperFunctions.ListToString(Surfaces);
}
