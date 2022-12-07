using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_Malnutrition : StatusDisplay
{
    // StatusDisplay Base
    public override string Name => "Malnutrition";
    public override Sprite DisplaySprite => ResourceManager.Singleton.SD_Malnutrition;
    public override bool DoShowDisplayValue => true;

    // Individual
    private Animal Animal;

    public SD_Malnutrition(Animal animal)
    {
        Animal = animal;
    }

    public override string GetDisplayValue() => Animal.Malnutrition.ToString("F0");
    public override bool ShouldShow() => Animal.Malnutrition > 0;
}
