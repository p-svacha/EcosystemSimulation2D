using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StatusDisplay : MonoBehaviour
{
    [Header("Elements")]
    public Image Icon;
    public TextMeshProUGUI Text;

    private StatusDisplay StatusDisplay;

    public void Init(StatusDisplay statusDisplay)
    {
        StatusDisplay = statusDisplay;
        Icon.sprite = statusDisplay.DisplaySprite;
        if (!statusDisplay.DoShowDisplayValue) Text.gameObject.SetActive(false);
        else Text.text = statusDisplay.GetDisplayValue();
    }

    public void UpdateDisplay()
    {
        if (StatusDisplay.DoShowDisplayValue) Text.text = StatusDisplay.GetDisplayValue();
    }
}
