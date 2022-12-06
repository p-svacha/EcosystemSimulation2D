using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Has a fixed value of a generic type that is not influenced by other attributes.
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
    public T Value { get; protected set; }

    private readonly string _Name;
    private readonly string _Description;
    private readonly AttributeId _Id;
    private readonly AttributeType _Type;
    private readonly string _Category;
    private readonly IThing _Thing;

    public StaticAttribute(IThing thing, AttributeId id, string category, string name, string description, T value)
    {
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
        if (Value is int) return (int)(object)Value;
        if (Value is float) return (float)(object)Value;
        throw new System.Exception("GetValue is not supported for StaticAttributes with type " + typeof(T).ToString() + ".");
    }

    public override string GetValueString()
    {
        return Value.ToString();
    }
}
