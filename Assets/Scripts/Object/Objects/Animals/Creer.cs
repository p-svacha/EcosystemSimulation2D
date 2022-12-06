using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creer : Animal
{
    // TileObject
    protected override string ObjectName => "Creer";
    protected override string ObjectDescription => "A peaceful animal that eats tall grass.";
    public override TileObjectType Type => TileObjectType.Creer;

    // Animal
    protected override float MOVEMENT_SPEED => 0.2f;
    protected override float WATER_MOVEMENT_SPEED => 0f;

    // Individual

    protected new void Update()
    {
        if (!IsSimulated) return;
        base.Update();

        if (!IsMoving && Random.value < 0.05f) MoveTo(World.Singleton.GetRandomTileInRange(Tile.Coordinates, 15));
    }

}
