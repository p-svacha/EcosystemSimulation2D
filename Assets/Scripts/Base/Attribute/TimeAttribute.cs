using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Special kind of static attribute that stores a SimulationTime as its value.
/// </summary>
public class TimeAttribute : StaticAttribute<SimulationTime>
{
    public TimeAttribute(AttributeId id, string category, string name, string description, SimulationTime value) : base(id, category, name, description, value) { }

    public override float GetValue() => Value.AbsoluteTime;
}
