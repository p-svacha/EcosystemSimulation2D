using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A DynamicBarAttribute is an attribute consisting of a current value and a max value (i.e. 84/100).
/// <br/> AttributeModifiers affect the max value of the attribute. 
/// <br/> The current value can be directly changed by discrete values but is also affected by changes of the max value, meaning the ratio (CurrentValue / MaxValue) will stay the same when the MaxValue is changed.
/// </summary>
public abstract class DynamicBarAttribute : DynamicAttribute
{
    public DynamicBarAttribute(AttributeId id, string name, string category, AttributeType type) : base(id, name, category, type) { }

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
    /// Calculates the new max value from all modifiers and derives the new current value from that and the previous ratio.
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
    public override string GetValueString() => Value.ToString("F1") + " / " + MaxValue.ToString("F1");
    public override string GetValueBreakdownText()
    {
        string text = "Calculation of max value:\n\n";

        // Overwrite active
        AttributeModifier overwriteModifier = GetActiveOverwriteModifier();
        if (overwriteModifier != null)
        {
            text += overwriteModifier.Source + ":\t" + overwriteModifier.Value + " (Forced)\n";
        }
        // Default calculation
        else
        {
            List<AttributeModifier> modifiers = GetAllModifiers();

            AttributeModifier baseMod = modifiers.First(x => x.Type == AttributeModifierType.BaseValue);
            text += baseMod.Source + ":\t" + baseMod.Value + "\n";

            foreach (AttributeModifier mod in modifiers.Where(x => x.Type == AttributeModifierType.Add))
                text += "\n" + mod.Source + ":\t" + (mod.Value >= 0 ? "+" : "") + mod.Value;
            foreach (AttributeModifier mod in modifiers.Where(x => x.Type == AttributeModifierType.Multiply))
                text += "\n" + mod.Source + ":\tx" + mod.Value;
            text += "\n\nMax Value:\t" + MaxValue;
        }

        text += "\nCurrent Value:\t" + Value + "\n\n" + Value + " / " + MaxValue;

        return text;
    }
}
