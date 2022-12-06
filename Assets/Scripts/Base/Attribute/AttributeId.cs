using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeId
{
    /// <summary> Unique identifier of a thing. </summary>
    Id,

    /// <summary> Name that gets displayed. </summary>
    Name,

    /// <summary> Description of a thing. </summary>
    Description,


    /// <summary> Used by WorldTile. Coordinate position of a thing. </summary>
    Coordinates,

    /// <summary> Used by WorldTile. Surface of the tile.  </summary>
    Surface,

    /// <summary> Used by Surface. % Chance per hour that tall grass gets spawned. </summary>
    TallGrassSpawnChance,

    /// <summary> Flag if animals need to be able to swim to traverse a surface. </summary>
    RequiresSwimming,

    /// <summary> How much animals are slowed down by traversing something. </summary>
    MovementCost,


    /// <summary> Maximum amount of HP an object can have. </summary>
    MaxHealth,

    /// <summary> Current amount of HP an object has. </summary>
    Health,




    /// <summary> Speed at which an Animal moves on land. </summary>
    MovementSpeed,

    /// <summary> Speed at which an Animal moves on water. </summary>
    WaterMovementSpeed,

    /// <summary> Maximum amount of nutrition an animal can store. </summary>
    MaxNutrition,

    /// <summary> Current amount of nutrition an animal has. </summary>
    Nutrition,

    /// <summary> Amount at which the nutrition of an animal drops per hour. </summary>
    HungerRate,
}
