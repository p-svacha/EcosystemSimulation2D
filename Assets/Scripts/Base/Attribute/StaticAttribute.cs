using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Has a fixed value of a generic type that is not influenced by other attributes.
/// </summary>
public class StaticAttribute<T> : Attribute
{
    public T Value { get; protected set; }

    public StaticAttribute(IThing thing, AttributeId id, AttributeCategory category, string name, string description, T value) : base(thing)
    {
        Id = id;
        Category = category;
        Name = name;
        Description = description;
        Value = value;
        Type = AttributeType.Static;
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
