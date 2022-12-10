using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SWC_AnimalBase : UI_SWC_TileObjectBase
{
    private Animal Animal;

    [Header("Elements")]
    public UI_ValueBar FoodBar;
    public TextMeshProUGUI CurrentActivityText;
    public GameObject StatusDisplayContainer;

    public override void Init(IThing thing)
    {
        base.Init(thing);
        Animal = (Animal)thing;
        FoodBar.Init("Nutrition");

        UpdateStatusDisplays();
    }

    protected override void Update()
    {
        base.Update();
        FoodBar.SetValue(Animal.Nutrition, Animal.MaxNutrition);
        CurrentActivityText.text = Animal.CurrentActivity;

        UpdateStatusDisplays();
    }

    private void UpdateStatusDisplays()
    {
        if (Animal.StatusDisplays.Count == 0) return;

        foreach (StatusDisplay sd in Animal.StatusDisplays)
        {
            if (sd.ShouldShow())
            {
                if (sd.SelectionWindowObject != null) sd.SelectionWindowObject.UpdateDisplay();
                else sd.CreateUIDisplay(StatusDisplayContainer.transform);
            }
            else
            {
                if (sd.SelectionWindowObject != null) Destroy(sd.SelectionWindowObject.gameObject);
            }
        }
    }
}
