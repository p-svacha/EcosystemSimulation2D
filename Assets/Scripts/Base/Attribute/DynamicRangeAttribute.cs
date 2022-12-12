using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A DynamicRangeAttribute is an attribute consisting of a current value and a max value (i.e. 84/100).
/// <br/> AttributeModifiers affect the max value of the attribute. 
/// <br/> The current value can be directly changed by discrete values but is also affected by changes of the max value, meaning the ratio (CurrentValue / MaxValue) will stay the same when the MaxValue is changed.
/// </summary>
public abstract class DynamicRangeAttribute : DynamicAttribute
{
    // Attribute Base
    public override AttributeType Type => AttributeType.DynamicRange;

    // Required setting
    /// <summary>
    /// If true, the value will also change whenever the max value changes to keep the ratio the same.
    /// </summary>
    protected abstract bool KeepValueRatio { get; }

    // Value
    public float Value { get; private set; }
    public float MaxValue { get; private set; }
    public float Ratio => Value / MaxValue; // [0-1]
    
    /// <summary>
    /// Needs to be called in LateInit of a TileObject.
    /// </summary>
    public void Init(float initialRatio)
    {
        MaxValue = CalculateCurrentValue();
        Value = MaxValue * initialRatio;
    }

    /// <summary>
    /// Should be called at the beginning of each frame. Calculates the new max value from all modifiers and derives the new current value from that and the previous ratio.
    /// </summary>
    public void CalculateNewValues()
    {
        float currentRatio = Ratio;
        MaxValue = CalculateCurrentValue();
        if(KeepValueRatio) Value = MaxValue * currentRatio;
    }

    /// <summary>
    /// Changes the current value according to a value.
    /// </summary>
    public void ChangeValue(float delta)
    {
        Value += delta;
        Value = Mathf.Clamp(Value, 0f, MaxValue);
    }

    public override float GetValue() => throw new System.Exception("GetValue should not be used for DynamicRangeAttributes. Use Value or MaxValue instead.");
    public override string GetValueString() => Value + " / " + MaxValue;
}
