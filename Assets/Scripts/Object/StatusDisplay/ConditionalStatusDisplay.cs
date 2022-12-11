using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A ConditionalStatusDisplay is a special kind of StatusDisplay that is constantly attached to a TileObject.
/// <br/> It has a condition that gets checked each frame and if met, the StatusDisplay will be shown.
/// </summary>
public abstract class ConditionalStatusDisplay : StatusDisplay
{
    /// <summary>
    /// Requirement check if the status display should be displayed.
    /// </summary>
    public abstract bool ShouldShow();

    public ConditionalStatusDisplay(TileObject obj) : base(obj) { }
}
