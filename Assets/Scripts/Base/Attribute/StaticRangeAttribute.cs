using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An attribute consisting of a minimum and maximum integer value that can return a random value within the range.
/// </summary>
public class StaticRangeAttribute : Attribute
{
    // Attribute Base
    public override AttributeId Id => _Id;
    public override string Name => _Name;
    public override string Description => _Description;
    public override string Category => _Category;
    public override AttributeType Type => AttributeType.Base;

    // Static
    private readonly AttributeId _Id;
    private readonly string _Name;
    private readonly string _Description;
    private readonly string _Category;

    // Value
    public int MinValue { get; private set; }
    public int MaxValue { get; private set; }
    public int RandomValue => Random.Range(MinValue, MaxValue + 1);

    public StaticRangeAttribute(AttributeId id, string name, string category, string description, int min, int max)
    {
        _Id = id;
        _Name = name;
        _Description = description;
        _Category = category;

        MinValue = min;
        MaxValue = max;
    }

    public override float GetValue() => throw new System.Exception("GetValue is not supported for StaticRangeAttribute. Use MinValue, MaxValue or RandomValue");
    public override string GetValueString() => MinValue + " - " + MaxValue;
    public override string GetValueBreakdownText() => GetValueString();
}
