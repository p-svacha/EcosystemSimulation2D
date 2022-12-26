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
    public override string Name => _Name;
    public override string Description => _Description;
    public override AttributeId Id => _Id;
    public override AttributeType Type => AttributeType.Static;
    public override string Category => _Category;
    public override IThing Thing => _Thing;

    // Static
    private readonly string _Name;
    private readonly string _Description;
    private readonly AttributeId _Id;
    private readonly string _Category;
    private readonly IThing _Thing;

    // Value
    public virtual T Value { get; private set; }

    public StaticAttribute(IThing thing, AttributeId id, string category, string name, string description, T value)
    {
        if (!(this is TimeAttribute) && value is SimulationTime) throw new System.Exception("Use TimeAttribute instead of StaticAttribute<SimulationTime>");

        _Thing = thing;
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

    public override float GetValue()
    {
        return (float)(object)Value;
    }

    public T GetStaticValue()
    {
        return Value;
    }

    public override string GetValueString()
    {
        return Value.ToString();
    }
}
