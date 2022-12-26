using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Activity represents a specific action an animal can do.
/// </summary>
public abstract class Activity
{
    protected AnimalBase Animal;
    public abstract ActivityId Id { get; }
    public abstract string DisplayString { get; }
    public bool IsActive { get; protected set; }

    public Activity(AnimalBase animal)
    {
        Animal = animal;
        IsActive = false;
    }

    /// <summary>
    /// Returns a float value representing how urgently this activity should be done.
    /// </summary>
    public abstract float GetUrgency();

    /// <summary>
    /// Starts the activity.
    /// </summary>
    public void Start()
    {
        IsActive = true;
        OnActivityStart();
    }

    /// <summary>
    /// Gets triggered when the activity is started.
    /// </summary>
    protected virtual void OnActivityStart() { }

    /// <summary>
    /// Gets triggered every frame while the activity is active.
    /// </summary>
    public virtual void OnTick() { }

    /// <summary>
    /// Ends the activity.
    /// </summary>
    public void End()
    {
        IsActive = false;
        OnActivityEnd();
    }

    /// <summary>
    /// Gets triggered when the activity is ended.
    /// </summary>
    protected virtual void OnActivityEnd() { }
}

public enum ActivityId
{
    Standing,
    WanderAround,
    Eat
}
