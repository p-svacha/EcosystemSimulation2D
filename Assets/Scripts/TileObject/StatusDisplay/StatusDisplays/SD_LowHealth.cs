using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_LowHealth : ConditionalStatusDisplay
{
    // StatusDisplay Base
    public override string Name => "Low Health";
    public override Sprite DisplaySprite => ResourceManager.Singleton.SD_LowHealth;
    public override bool DoShowDisplayValue => false;

    // Individual
    public SD_LowHealth(TileObjectBase obj) : base(obj) { }
    public override bool ShouldShow() => (TileObject.Attributes[AttributeId.Health] as RangeAttribute).Ratio < 0.2f;
}
