using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_Pregnancy : StatusDisplay
{
    // StatusDisplay Base
    public override string Name => "Pregnant";
    public override Sprite DisplaySprite => ResourceManager.Singleton.SD_Pregnancy;
    public override bool DoShowDisplayValue => true;

    // Individual
    private Animal Animal;

    public SD_Pregnancy(Animal animal)
    {
        Animal = animal;
    }

    public override string GetDisplayValue() => Animal.PregnancyProgress.AbsoluteDay + " / " + Animal.PregnancyDuration.AbsoluteDay;
    public override bool ShouldShow() => Animal.PregnancyProgress.ExactTime > 0;
}
