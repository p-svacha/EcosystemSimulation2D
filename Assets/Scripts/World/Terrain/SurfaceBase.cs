using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        _Attributes.Add(AttributeId.MovementCost, new StaticAttribute<float>(AttributeId.MovementCost, "Movement", "Movement Cost", "How hard it is to move across this surface.", MOVEMENT_COST));
        _Attributes.Add(AttributeId.RequiresSwimming, new StaticAttribute<float>(AttributeId.RequiresSwimming, "Movement", "Requires Swimming", "If objects need to be able to swim to traverse this surface.", REQUIRES_SWIMMING? 1 : 0));

        _Attributes.Add(AttributeId.PlantGrowChance, new StaticAttribute<float>(AttributeId.PlantGrowChance, "Life Support", "Plant Grow Chance", "Chance per hour that a plant will spawn on a tile with this surface.", PLANT_GROW_CHANCE));
        _Attributes.Add(AttributeId.PlantSpawnChance, new StaticAttribute<float>(AttributeId.PlantSpawnChance, "Life Support", "Plant Spawn Chance", "Chance per tile that a plant is spawned on a tile with this surface during world generation.", PLANT_SPAWN_CHANCE));
        _Attributes.Add(AttributeId.AnimalSpawnChance, new StaticAttribute<float>(AttributeId.AnimalSpawnChance, "Life Support", "Animal Spawn Chance", "Chance per tile that a group of animals is spawned on a tile with this surface during world generation.", ANIMAL_SPAWN_CHANCE));
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
