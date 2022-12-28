using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public abstract class OrganismBase : VisibleTileObjectBase
{
    // Required Attributes
    protected abstract SimulationTime MATURITY_AGE { get; }
    protected abstract List<SurfaceId> SPAWN_SURFACES { get; }

    #region Initialize

    public override void Init()
    {
        base.Init();

        // Attributes
        _Attributes.Add(AttributeId.MaturityAge, new TimeAttribute(AttributeId.MaturityAge, "General", "Maturity Age", "The age at which an organism reaches full size. Animals are able to get pregnant at that point.", MATURITY_AGE));
        _Attributes.Add(AttributeId.Size, new Att_Size(this));
        _Attributes.Add(AttributeId.SpawnSurfaces, new Att_SpawnSurfaces(SPAWN_SURFACES));

        // Set size correctly immediately
        UpdateSizeDisplay();
    }

    
    public override void InitExisting()
    {
        base.InitExisting();

        int numYears = Random.Range(0, 4);
        int numMonths = Random.Range(0, SimulationTime.MonthsPerYear);
        int numDays = Random.Range(0, SimulationTime.DaysPerMonth);
        int numHours = Random.Range(0, SimulationTime.HoursPerDay);
        Age.SetTime(numYears, numMonths, numDays, numHours);

        Health.Init(initialRatio: 1f);
    }

    #endregion

    #region Update

    // Performance Profilers
    static readonly ProfilerMarker pm_all = new ProfilerMarker("Update Organism");
    static readonly ProfilerMarker pm_sizeDisplay = new ProfilerMarker("Update Size Display");

    public override void Tick()
    {
        pm_all.Begin();
        base.Tick();

        pm_sizeDisplay.Begin();
        if (NumTicks % 60 == 0) UpdateSizeDisplay();
        pm_sizeDisplay.End();

        pm_all.End();
    }

    private void UpdateSizeDisplay()
    {
        float renderSize = GetFloatAttribute(AttributeId.Size);
        Renderer.size = new Vector2(renderSize, renderSize);
    }

    #endregion


    #region Getters

    public float Size => GetFloatAttribute(AttributeId.Size);
    public float MaturityAge => GetFloatAttribute(AttributeId.MaturityAge);

    #endregion
}
