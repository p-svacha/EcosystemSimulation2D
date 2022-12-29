using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Has a fixed value of a generic type that is not influenced by other attributes.
/// <br/> For Attributes storing a time use TimeAttribute.
/// </summary>
public class StaticAttribute<T> : Attribute
{
    // Attribute Base
    public override AttributeId Id => _Id;
    public override string Name => _Name;
    public override string Description => _Description;
    public override string Category => _Category;
    public override AttributeType Type => AttributeType.Base;

    // Static
    private readonly string _Name;
    private readonly string _Description;
    private readonly AttributeId _Id;
    private readonly string _Category;
    // Value
    public virtual T Value { get; private set; }

    public StaticAttribute(AttributeId id, string category, string name, string description, T value)
    {
        if (!(this is TimeAttribute) && value is SimulationTime) throw new System.Exception("Use TimeAttribute instead of StaticAttribute<SimulationTime>");

        _Id = id;
        _Category = category;
        _Name = name;
        _Description = description;

        Value = value;
    }

    public void SetValue(T newValue)
    {
        Value = newValue;
    }

    public override float GetValue() => (float)(object)Value;
    public T GetStaticValue() => Value;
    public override string GetValueString() => Value.ToString();
    public override string GetValueBreakdownText() => GetValueString();
}
