using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Represents an attribute displayed as a row within the ThingInfoWindow.
/// Can be clicked on for further information.
/// </summary>
public class UI_ThingInfoAttribute : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI ValueText;

    private Attribute Attribute;

    /// <summary>
    /// Initialize by giving an attribute.
    /// </summary>
    public void Init(Attribute att, UI_ThingInfoWindow window)
    {
        Attribute = att;
        NameText.text = att.Name;
        ValueText.text = att.GetValueString();
        GetComponent<Button>().onClick.AddListener(() => window.SetSelectedAttribute(att));
    }

    /// <summary>
    /// Update the value display
    /// </summary>
    public void UpdateValueDisplay()
    {
        ValueText.text = Attribute.GetValueString();
    }
}
