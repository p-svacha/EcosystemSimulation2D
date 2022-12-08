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

    public override void Init(IThing thing)
    {
        base.Init(thing);
        Animal = (Animal)thing;
        FoodBar.Init("Nutrition");
    }

    protected override void Update()
    {
        base.Update();
        FoodBar.SetValue(Animal.Nutrition, Animal.MaxNutrition);
        CurrentActivityText.text = Animal.CurrentActivity;
    }
}
