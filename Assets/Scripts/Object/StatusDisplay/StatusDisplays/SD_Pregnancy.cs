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
    public override string DisplayValue => (TileObject as Animal).PregnancyProgress.AbsoluteDay + " / " + (TileObject as Animal).PregnancyDuration.AbsoluteDay;
}
