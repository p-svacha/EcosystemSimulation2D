using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public abstract class PlantBase : OrganismBase
{
    #region Update

    // Performance Profilers
    static readonly ProfilerMarker pm_all = new ProfilerMarker("Update PlantBase");

    public override void Tick()
    {
        pm_all.Begin();
        base.Tick();

        pm_all.End();
    }

    #endregion
}
