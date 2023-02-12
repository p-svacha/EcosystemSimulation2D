using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Script needs to be attached to the exact object that should react to the drag (usually the header)
/// </summary>
public class UI_DraggableWindow : MonoBehaviour, IDragHandler
{
    /// <summary>
    /// Window that should be dragged around.
    /// </summary>
    public RectTransform DragTarget;
    public bool IsDraggable;

    public void OnDrag(PointerEventData eventData)
    {
        if (IsDraggable)
        {
            DragTarget.anchoredPosition += eventData.delta / GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
            DragTarget.transform.SetAsLastSibling();
        }
    }
}