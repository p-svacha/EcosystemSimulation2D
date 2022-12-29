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
    private SE_Pregnancy PregnancyEffect;
    public SD_Pregnancy(SE_Pregnancy effect)
    {
        PregnancyEffect = effect;
    }
    public override string DisplayValue => PregnancyEffect.PregnancyProgress.AbsoluteDay + " / " + (TileObject as AnimalBase).PregnancyDuration.AbsoluteDay;
}
