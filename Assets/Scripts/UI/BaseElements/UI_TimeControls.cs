using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_TimeControls : MonoBehaviour
{
    [Header("Elements")]
    public TextMeshProUGUI TimeText;
    public Button Speed0Button; // Pause
    public Button Speed1Button;
    public Button Speed2Button;
    public Button Speed3Button;

    private void Start()
    {
        Speed0Button.onClick.AddListener(Speed0Button_OnClick);
        Speed1Button.onClick.AddListener(Speed1Button_OnClick);
        Speed2Button.onClick.AddListener(Speed2Button_OnClick);
        Speed3Button.onClick.AddListener(Speed3Button_OnClick);
    }

    private void Speed0Button_OnClick() { Simulation.Singleton.SetSpeed(Simulation.SPEED0_MODIFIER); }
    private void Speed1Button_OnClick() { Simulation.Singleton.SetSpeed(Simulation.SPEED1_MODIFIER); }
    private void Speed2Button_OnClick() { Simulation.Singleton.SetSpeed(Simulation.SPEED2_MODIFIER); }
    private void Speed3Button_OnClick() { Simulation.Singleton.SetSpeed(Simulation.SPEED3_MODIFIER); }

    public void SetPauseDisplay(bool value)
    {
        Speed0Button.GetComponent<Image>().color = value ? ResourceManager.Singleton.ButtonSelectedColor : ResourceManager.Singleton.IconButtonDefaultColor;
    }
    public void SetSpeedDisplay(int speedModifier)
    {
        Speed0Button.GetComponent<Image>().color = speedModifier == Simulation.SPEED0_MODIFIER ? ResourceManager.Singleton.ButtonSelectedColor : ResourceManager.Singleton.IconButtonDefaultColor;
        Speed1Button.GetComponent<Image>().color = speedModifier == Simulation.SPEED1_MODIFIER ? ResourceManager.Singleton.ButtonSelectedColor : ResourceManager.Singleton.IconButtonDefaultColor;
        Speed2Button.GetComponent<Image>().color = speedModifier == Simulation.SPEED2_MODIFIER ? ResourceManager.Singleton.ButtonSelectedColor : ResourceManager.Singleton.IconButtonDefaultColor;
        Speed3Button.GetComponent<Image>().color = speedModifier == Simulation.SPEED3_MODIFIER ? ResourceManager.Singleton.ButtonSelectedColor : ResourceManager.Singleton.IconButtonDefaultColor;
    }

    public void SetTimeDisplay(SimulationTime time)
    {
        TimeText.text = time.DateString;
    }

    public static UI_TimeControls Singleton { get { return GameObject.Find("TimeControls").GetComponent<UI_TimeControls>(); } }
}
