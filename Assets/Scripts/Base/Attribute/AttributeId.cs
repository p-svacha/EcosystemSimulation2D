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


    /// <summary> Time at which an object has been created. </summary>
    CreatedAt,

    /// <summary> How long an object has been existing in the world. </summary>
    Age,

    /// <summary> The age at which an organism reaches full size. Animals are able to get pregnant at that point. </summary>
    MaturityAge,

    /// <summary> Base max health of an object. </summary>
    HealthBase,

    /// <summary> Current and maximum amount of HP an object currently has. </summary>
    Health,

    /// <summary> How big an organism is compared to its default size. </summary>
    Size,




    /// <summary> Base speed at which an animal moves on land. </summary>
    LandMovementSpeedBase,

    /// <summary> Actual speed at which an animal moves on land. </summary>
    LandMovementSpeed,

    /// <summary> Speed at which an animal moves on water. </summary>
    WaterMovementSpeed,

    /// <summary> How many tiles an animal can see in all directions and detect specific objects. </summary>
    VisionRange,


    /// <summary> Base amount of nutrition an animal can store. </summary>
    NutritionBase,

    /// <summary> Current and maximum amount of nutrition an animal has. </summary>
    Nutrition,

    /// <summary> Base amount at which the nutrition of an animal drops per hour. </summary>
    HungerRateBase,

    /// <summary> Actual amount at which the nutrition of an animal drops per hour. </summary>
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

    /// <summary> Minimum age at which an animal can get pregnant. </summary>
    PregnancyMinAge,

    /// <summary> Maximum age at which an animal can get pregnant. </summary>
    PregnancyMaxAge,

    /// <summary> Base chance per hour that an animal gets pregnant. Actualy chance depends on a lot of factors like age and health. </summary>
    PregnancyChanceBase,

    /// <summary> Actual chance per hour that an animal gets pregnant. </summary>
    PregnancyChance,

    /// <summary> How long an animal is pregnant for. </summary>
    PregnancyDuration,

    /// <summary> How long an animal has been pregnant for. </summary>
    PregnancyProgress,

    /// <summary> Minimum amount of children an animal will produce when giving birth. </summary>
    MinNumOffspring,

    /// <summary> Maximum amount of children an animal will produce when giving birth. </summary>
    MaxNumOffspring,
}
