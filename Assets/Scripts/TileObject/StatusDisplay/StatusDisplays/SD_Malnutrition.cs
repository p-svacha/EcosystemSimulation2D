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
    private SE_Malnutrition MalnutritionEffect;
    public SD_Malnutrition(SE_Malnutrition effect)
    {
        MalnutritionEffect = effect;
    }

    public override string DisplayValue => MalnutritionEffect.MalnutritionAdvancement.ToString("F0");
}
