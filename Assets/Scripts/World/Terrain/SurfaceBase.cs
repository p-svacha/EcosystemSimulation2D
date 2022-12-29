using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Attribute;

/// <summary>
/// A surface represents one type of terrain. The higher the precedence the more prominent it gets drawn.
/// </summary>
public abstract class SurfaceBase : IThing
{
    // IThing
    public ThingId ThingId => ThingId.Surface;
    public string Name => SurfaceName;
    public string Description => SurfaceDescription;
    public Dictionary<AttributeId, Attribute> Attributes => _Attributes;
    public UI_SelectionWindowContent SelectionWindowContent => null;

    protected Dictionary<AttributeId, Attribute> _Attributes = new Dictionary<AttributeId, Attribute>();
    protected Dictionary<AttributeId, float> FloatAttributeCache = new Dictionary<AttributeId, float>();

    // Required Attributes

    public abstract SurfaceId SurfaceId { get; }

    /// <summary> Determines the overlay order between surfaces. The higher the precedence, the more other surface types this type overlays. Precedence must be unique. </summary>
    public abstract int Precedence { get; }
    protected abstract string SurfaceName { get; }
    protected abstract string SurfaceDescription { get; }

    protected abstract float MOVEMENT_COST { get; }
    protected abstract bool REQUIRES_SWIMMING { get; }
    protected abstract float PLANT_GROW_CHANCE { get; }
    protected abstract float PLANT_SPAWN_CHANCE { get; }
    protected abstract float ANIMAL_SPAWN_CHANCE { get; }


    public SurfaceBase()
    {
        _Attributes.Add(AttributeId.MovementCost, new StaticAttribute<float>(AttributeId.MovementCost, "Movement Cost", "Movement", MOVEMENT_COST));
        _Attributes.Add(AttributeId.RequiresSwimming, new StaticAttribute<float>(AttributeId.RequiresSwimming, "Requires Swimming", "Movement", REQUIRES_SWIMMING ? 1 : 0));

        _Attributes.Add(AttributeId.PlantGrowChance, new StaticAttribute<float>(AttributeId.PlantGrowChance, "Plant Grow Chance", "Life Support", PLANT_GROW_CHANCE));
        _Attributes.Add(AttributeId.PlantSpawnChance, new StaticAttribute<float>(AttributeId.PlantSpawnChance, "Plant Spawn Chance", "Life Support", PLANT_SPAWN_CHANCE));
        _Attributes.Add(AttributeId.AnimalSpawnChance, new StaticAttribute<float>(AttributeId.AnimalSpawnChance, "Animal Spawn Chance", "Life Support", ANIMAL_SPAWN_CHANCE));
    }

    /// <summary>
    /// Gets called when the world is generated.
    /// </summary>
    public void OnWorldGeneration(WorldTile tile)
    {
        // Spawn plant
        if (Random.value < GetFloatAttribute(AttributeId.PlantSpawnChance))
        {
            TileObjectId chosenPlant = HelperFunctions.GetRandomPlantForSurface(SurfaceId);
            World.Singleton.SpawnTileObject(tile, chosenPlant, isNew: false);
        }

        // Spawn animal group
        if (Random.value < GetFloatAttribute(AttributeId.AnimalSpawnChance))
        {
            TileObjectId chosenAnimal = HelperFunctions.GetRandomAnimalForSurface(SurfaceId);
            int numAnimalsToSpawn = (TileObjectFactory.DummyObjects[chosenAnimal] as AnimalBase).SpawnGroupSize.RandomValue;
            for(int i = 0; i < numAnimalsToSpawn; i++)
            {
                WorldTile spawnTile = World.Singleton.GetTile(HelperFunctions.GetRandomPositionWithinRange(tile.Coordinates, 3));
                World.Singleton.SpawnTileObject(spawnTile, chosenAnimal, isNew: false);
            }
            
        }
    }
    
    /// <summary>
    /// Gets called every n'th frame.
    /// </summary>
    public void Tick(WorldTile tile, float hoursSinceLastUpdate)
    {
        FloatAttributeCache.Clear();

        // Grow Plant
        if(Random.value < GetFloatAttribute(AttributeId.PlantGrowChance))
        {
            TileObjectId chosenPlant = HelperFunctions.GetRandomPlantForSurface(SurfaceId);
            World.Singleton.SpawnTileObject(tile, chosenPlant, isNew: true);
        }
    }

    #region Getters

    /// <summary>
    /// Returns the value of a DynamicAttribute and caches it for the remainder of the tick.
    /// </summary>
    public float GetFloatAttribute(AttributeId id)
    {
        if (FloatAttributeCache.TryGetValue(id, out float cachedValue)) return cachedValue;

        float value = Attributes[id].GetValue();
        FloatAttributeCache[id] = value;
        return value;
    }

    public float MovementCost => Attributes[AttributeId.MovementCost].GetValue();
    public bool RequiresSwimming => Attributes[AttributeId.RequiresSwimming].GetValue() == 1f;
    #endregion
}

public enum SurfaceId
{
    Soil,
    Sand,
    Water
}
