using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// The visualization of a StatusDisplay in world space.
/// </summary>
public class StatusDisplayObject : MonoBehaviour
{
    [Header("Elements")]
    public SpriteRenderer SpriteDisplay;
    public TextMeshPro ValueDisplay;

    private StatusDisplay StatusDisplay;

    public void Init(StatusDisplay statusDisplay, int index, int numElements)
    {
        StatusDisplay = statusDisplay;
        SpriteDisplay.sprite = statusDisplay.DisplaySprite;
        if (!statusDisplay.DoShowDisplayValue) ValueDisplay.gameObject.SetActive(false);
        else ValueDisplay.text = statusDisplay.GetDisplayValue();

        UpdateDisplay(index, numElements);
    }

    public void UpdateDisplay(int index, int numElements)
    {
        // Update Value
        if (StatusDisplay.DoShowDisplayValue) ValueDisplay.text = StatusDisplay.GetDisplayValue();

        // Calculate world position based on how many status displays there are
        float yPos = 1f;
        float xGap = 1f;

        float xStart = -(xGap * 0.5f * (numElements - 1));
        float xPos = xStart + (index * xGap);
        Vector3 pos = new Vector3(xPos, yPos, 0f);

        transform.localPosition = pos;
        transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        
    }
}
