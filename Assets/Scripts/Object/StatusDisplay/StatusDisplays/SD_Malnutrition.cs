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
    public override string DisplayValue => TileObject.Attributes[AttributeId.Malnutrition].GetValue().ToString("F0");
}
