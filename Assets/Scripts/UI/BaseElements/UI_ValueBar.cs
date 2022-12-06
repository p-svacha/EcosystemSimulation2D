using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ValueBar : MonoBehaviour
{
    [Header("Elements")]
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI BarText;
    public RectTransform ProgressBar;

    public void Init(string title, bool hideBarText = false)
    {
        TitleText.text = title;
        if(hideBarText) BarText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Set the value how full the bar is. Value needs to be in [0,1] Range.
    /// </summary>
    public void SetValue(float currentValue, float maxValue)
    {
        float ratio = currentValue / maxValue;
        ProgressBar.anchorMax = new Vector2(ratio, 1f);
        BarText.text = (int)currentValue + " / " + (int)maxValue;
    }

}
