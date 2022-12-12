using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public abstract class OrganismBase : VisibleTileObjectBase
{
    // Required Attributes
    protected abstract SimulationTime MATURITY_AGE { get; }

    #region Initialize

    public override void Init()
    {
        base.Init();

        // Attributes
        _Attributes.Add(AttributeId.MaturityAge, new StaticAttribute<SimulationTime>(this, AttributeId.MaturityAge, "General", "Maturity Age", "The age at which an organism reaches full size. Animals are able to get pregnant at that point.", MATURITY_AGE));
    }

    #endregion

    #region Update

    // Performance Profilers
    static readonly ProfilerMarker pm_all = new ProfilerMarker("Update Organism");
    static readonly ProfilerMarker pm_growth = new ProfilerMarker("Update Growth");

    public override void Tick()
    {
        pm_all.Begin();
        base.Tick();

        pm_growth.Begin();
        UpdateGrowth();
        pm_growth.End();

        pm_all.End();
    }

    private void UpdateGrowth()
    {

    }

    #endregion


    #region Getters

    public float Size => GetFloatAttribute(AttributeId.Size);
    public SimulationTime MaturityAge => ((StaticAttribute<SimulationTime>)Attributes[AttributeId.MaturityAge]).GetStaticValue();

    #endregion
}
