using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A RangeAttribute is a static float attribute within a minimum value of 0 and a maximum value.
/// </summary>
public class RangeAttribute : StaticAttribute<float>
{
    // Attribute Base
    public override AttributeType Type => AttributeType.Range;

    // Value
    public float MaxValue { get; private set; }
    public float Ratio => Value / MaxValue; // [0-1]

    public RangeAttribute(IThing thing, AttributeId id, string category, string name, string description, float value, float maxValue) : base(thing, id, category, name, description, value)
    {
        MaxValue = maxValue;
    }

    public override string GetValueString()
    {
        return Value + " / " + MaxValue;
    }
}
