using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SWC_AnimalBase : UI_SWC_TileObjectBase
{
    private AnimalBase Animal;

    [Header("Elements")]
    public UI_ValueBar FoodBar;
    public TextMeshProUGUI CurrentActivityText;
    public GameObject StatusDisplayContainer;

    public override void Init(IThing thing)
    {
        base.Init(thing);
        Animal = (AnimalBase)thing;
        FoodBar.Init("Nutrition");

        UpdateStatusDisplays();
    }

    protected override void Update()
    {
        base.Update();
        FoodBar.SetValue(Animal.Nutrition.Value, Animal.Nutrition.MaxValue);
        CurrentActivityText.text = Animal.CurrentActivity;

        UpdateStatusDisplays();
    }

    private void UpdateStatusDisplays()
    {
        // Collect all displays
        List<StatusDisplay> statusDisplaysToShow = new List<StatusDisplay>();

        // from StatusEffect
        foreach (StatusEffect statusEffect in Animal.StatusEffects)
            statusDisplaysToShow.Add(statusEffect.Display);

        // Conditional
        foreach (ConditionalStatusDisplay csd in Animal.ConditionalStatusDisplays)
            if (csd.ShouldShow())
                statusDisplaysToShow.Add(csd);

        // Display them
        HelperFunctions.DestroyAllChildredImmediately(StatusDisplayContainer);

        foreach (StatusDisplay display in statusDisplaysToShow)
            display.CreateUIDisplay(StatusDisplayContainer.transform);
    }
}
