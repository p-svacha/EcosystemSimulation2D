using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeId
{
    /// <summary> Used as null. </summary>
    None,

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


    /// <summary> Current and maximum amount of HP an object has. </summary>
    Health,




    /// <summary> Speed at which an animal moves on land. </summary>
    MovementSpeed,

    /// <summary> Speed at which an animal moves on water. </summary>
    WaterMovementSpeed,

    /// <summary> How many tiles an animal can see in all directions and detect specific objects. </summary>
    VisionRange,


    /// <summary> Current and maximum amount of nutrition an animal has. </summary>
    Nutrition,

    /// <summary> Amount at which the nutrition of an animal drops per hour. </summary>
    HungerRate,

    /// <summary> How advanced the malnutrition of an animal is. The higher it is, the more health it loses. </summary>
    Malnutrition,

    /// <summary> What kind of diet is needed to be able to eat an object. </summary>
    NutrientType,

    /// <summary> How much nutrition an object provides at when being eaten from full health to 0. </summary>
    NutrientValue,

    /// <summary> What types of food an animal is able to eat. </summary>
    Diet,

    /// <summary> How fast an animal is at eating food generally. </summary>
    EatingSpeed,

    /// <summary> How difficult an object is to eat generally. 1 means it takes 1 hour for the full thing to be eaten. </summary>
    EatingDifficulty,
}
