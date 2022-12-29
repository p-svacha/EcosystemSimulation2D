using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Special kind of static attribute that stores a SimulationTime as its value.
/// </summary>
public class TimeAttribute : StaticAttribute<SimulationTime>
{
    public TimeAttribute(AttributeId id, string name, string category, SimulationTime value) : base(id, name, category, value) { }

    public override float GetValue() => Value.AbsoluteTime;
}
