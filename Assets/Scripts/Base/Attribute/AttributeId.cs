using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public enum AttributeId
{
    /// <summary> Used as null. </summary>
    None,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Id'/></summary>
    Id,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Name'/></summary>
    Name,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Description'/></summary>
    Description,


    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Coordinates'/></summary>
    Coordinates,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Surface'/></summary>
    Surface,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/PlantGrowChance'/></summary>
    PlantGrowChance,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/PlantSpawnChance'/></summary>
    PlantSpawnChance,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/AnimalSpawnChance'/></summary>
    AnimalSpawnChance,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/RequiresSwimming'/></summary>
    RequiresSwimming,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/MovementCost'/></summary>
    MovementCost,


    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Category'/></summary>
    Category,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Age'/></summary>
    Age,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/MaturityAge'/></summary>
    MaturityAge,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/HealthBase'/></summary>
    HealthBase,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Health'/></summary>
    Health,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Size'/></summary>
    Size,


    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Commonness'/></summary>
    Commonness,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/SpawnSurfaces'/></summary>
    SpawnSurfaces,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/SpawnGroupSize'/></summary>
    SpawnGroupSize,


    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Movement'/></summary>
    Movement,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/LandMovementSpeedBase'/></summary>
    LandMovementSpeedBase,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/LandMovementSpeed'/></summary>
    LandMovementSpeed,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/WaterMovementSpeedBase'/></summary>
    WaterMovementSpeedBase,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/WaterMovementSpeed'/></summary>
    WaterMovementSpeed,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/VisionRange'/></summary>
    VisionRange,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/NutritionBase'/></summary>
    NutritionBase,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Nutrition'/></summary>
    Nutrition,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/HungerRateBase'/></summary>
    HungerRateBase,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/HungerRate'/></summary>
    HungerRate,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/NutrientType'/></summary>
    NutrientType,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/NutrientValueBase'/></summary>
    NutrientValueBase,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/NutrientValue'/></summary>
    NutrientValue,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/Diet'/></summary>
    Diet,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/EatingSpeed'/></summary>
    EatingSpeed,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/EatingDifficulty'/></summary>
    EatingDifficulty,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/PregnancyMinAge'/></summary>
    PregnancyMinAge,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/PregnancyMaxAge'/></summary>
    PregnancyMaxAge,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/PregnancyChanceBase'/></summary>
    PregnancyChanceBase,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/PregnancyChance'/></summary>
    PregnancyChance,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/PregnancyDuration'/></summary>
    PregnancyDuration,

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/NumOffspring'/></summary>
    NumOffspring,
}

