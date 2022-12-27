using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SimpleAttributeDisplay : MonoBehaviour
{
    public TextMeshProUGUI LabelText;
    public TextMeshProUGUI ValueText;
    public TooltipTarget TooltipTarget;

    private Attribute Attribute;

    public void Init(Attribute attribute)
    {
        Attribute = attribute;
        LabelText.text = attribute.Name;
        ValueText.text = attribute.GetValueString();
        TooltipTarget.Text = attribute.Description;
    }

    public void UpdateValue()
    {
        ValueText.text = Attribute.GetValueString();
    }
}
