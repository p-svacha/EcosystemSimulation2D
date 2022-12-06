using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach a child of this class to a panel that displays stuff about a specific thing.
/// </summary>
public abstract class UI_SelectionWindowContent : MonoBehaviour
{
    public UI_SelectionWindow ParentWindow;

    /// <summary>
    /// Initialize the Dynamic Content.
    /// </summary>
    public abstract void Init(IThing thing);

    /// <summary>
    /// Flag if the camera can be focussed on this thing.
    /// </summary>
    public abstract bool CanFocusCamera();
    /// <summary>
    /// Action that happens when the focus camera button is clicked.
    /// </summary>
    public virtual void FocusCamera() { }

    /// <summary>
    /// Flag if this thing can be removed.
    /// </summary>
    public abstract bool CanRemoveObject();
    /// <summary>
    /// Action that happens when the remove object button is clicked.
    /// </summary>
    public virtual void RemoveObject() { }

    /// <summary>
    /// Flag if the next layer can be selected.
    /// </summary>
    public abstract bool CanSelectNextLayer();
    /// <summary>
    /// Action that happens when the select next layer button is clicked.
    /// </summary>
    public virtual void SelectNextLayer() { }
}
