using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A StatusEffect is a modifier that can affect attributes and logic of the TileObject it is attached to. Inspired by a Rimworld hediff.
/// </summary>
public abstract class StatusEffect
{
    public abstract StatusEffectId Id { get; }
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract StatusDisplay Display { get; }
    public abstract Dictionary<AttributeId, AttributeModifier> AttributeModifiers { get; }
    

    protected TileObjectBase TileObject;

    /// <summary>
    /// Initialize a StatusEffect
    /// </summary>
    public void Init(TileObjectBase obj)
    {
        TileObject = obj;
        Display.Init(obj);

        // Add attribute modifiers to attached thing
        foreach(KeyValuePair<AttributeId, AttributeModifier> modifiers in AttributeModifiers)
        {
            DynamicAttribute att = TileObject.Attributes[modifiers.Key] as DynamicAttribute;
            if (att == null) throw new System.Exception("StatusEffect " + Name + " is trying to modify the " + modifiers.Key + " attribute of " + TileObject.Name + " even though it is not a DynamicAttribute.");

            att.AddStatusEffectModifier(modifiers.Value);
        }

        OnAdd();
    }

    /// <summary>
    /// Gets executed each frame.
    /// </summary>
    public void Tick()
    {
        OnTick();

        if(IsEndConditionReached()) TileObject.RemoveStatusEffect(this);
    }

    /// <summary>
    /// Terminate a StatusEffect
    /// </summary>
    public void End()
    {
        // Remove attribute modifiers to attached thing
        foreach (KeyValuePair<AttributeId, AttributeModifier> modifiers in AttributeModifiers)
        {
            DynamicAttribute att = TileObject.Attributes[modifiers.Key] as DynamicAttribute;
            att.RemoveStatusEffectModifier(modifiers.Value);
        }

        OnEnd();
    }


    /// <summary>
    /// Logic that gets executed when the StatusEffect is applied to a thing.
    /// </summary>
    protected virtual void OnAdd() { }

    /// <summary>
    /// Logic that gets executed every frame while the StatusEffect is active.
    /// </summary>
    protected virtual void OnTick() { }

    /// <summary>
    /// Checks and returns if the StatusEffect should end.
    /// </summary>
    protected abstract bool IsEndConditionReached();

    /// <summary>
    /// Logic that gets exectued when the StatusEffect is removed from a thing.
    /// </summary>
    protected virtual void OnEnd() { }

}
