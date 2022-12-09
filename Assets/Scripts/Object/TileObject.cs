using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A TileObject is an object/agent in a specific location on the map. TileObjects can have inherent logic and act by themselves.
/// <br/> TileObjects can either be own GameObjects with SpriteRenderes (see VisibleTileObject) or part of the TileMap (see TilemapTileObject).
/// </summary>
public abstract class TileObject : MonoBehaviour, IThing
{
    // IThing
    public ThingId Id => ThingId.Object;
    public string Name => ObjectName;
    public string Description => ObjectDescription;
    public Dictionary<AttributeId, Attribute> Attributes => _Attributes;
    public virtual UI_SelectionWindowContent SelectionWindowContent => ResourceManager.Singleton.SWC_TileObjectBase;

    // Required Attributes
    protected abstract string ObjectName { get; }
    protected abstract string ObjectDescription { get; }
    protected abstract int MAX_HEALTH { get; }

    // Optional Attributes
    protected virtual NutrientType NUTRIENT_TYPE => NutrientType.None;
    protected virtual float NUTRIENT_VALUE => 0f;
    protected virtual float EATING_DIFFICULTY => 1f;

    // General
    public abstract TileObjectType Type { get; }
    protected Dictionary<AttributeId, Attribute> _Attributes = new Dictionary<AttributeId, Attribute>();
    protected List<StatusDisplay> StatusDisplays = new List<StatusDisplay>();
    public WorldTile Tile { get; private set; }

    #region Initialize

    public virtual void Init()
    {
        // Init attributes
        _Attributes.Add(AttributeId.Age, new StaticAttribute<SimulationTime>(this, AttributeId.Age, "General", "Age", "How long an object has been existing in the world.", new SimulationTime()));
        _Attributes.Add(AttributeId.Health, new RangeAttribute(this, AttributeId.Health, "Health", "Health", "Current and maximum amount of HP an object has.", MAX_HEALTH, MAX_HEALTH));

        _Attributes.Add(AttributeId.NutrientType, new Att_NutrientType(this, NUTRIENT_TYPE));
        _Attributes.Add(AttributeId.NutrientValue, new StaticAttribute<float>(this, AttributeId.NutrientValue, "Nutrition", "Nutrients", "How much nutrition an object provides at when being eaten from full health to 0.", NUTRIENT_VALUE));
        _Attributes.Add(AttributeId.EatingDifficulty, new StaticAttribute<float>(this, AttributeId.EatingDifficulty, "Nutrition", "Eating Difficulty", "How difficult an object is to eat generally.", EATING_DIFFICULTY));
    }

    #endregion

    #region Update

    /// <summary>
    /// Tick gets called every frame when the simulation is running.
    /// </summary>
    public virtual void Tick()
    {
        Age.IncreaseTime(Simulation.Singleton.TickTime);

        UpdateStatusDisplays();
        UpdateDeath();
    }

    private void UpdateStatusDisplays()
    {
        if (StatusDisplays.Count == 0) return;
        int numActiveStatusDisplays = StatusDisplays.Where(x => x.ShouldShow()).Count();
        int index = 0;
        foreach (StatusDisplay sd in StatusDisplays)
        {
            if (sd.ShouldShow())
            {
                if (sd.WorldDisplayObject != null) sd.WorldDisplayObject.UpdateDisplay(index, numActiveStatusDisplays);
                else sd.CreateWorldDisplay(transform, index, numActiveStatusDisplays);
                index++;
            }
            else
            {
                if (sd.WorldDisplayObject != null) Destroy(sd.WorldDisplayObject.gameObject);
            }
        }
    }

    private void UpdateDeath()
    {
        if (Health == 0) Die();
    }

    protected void Die()
    {
        World.Singleton.RemoveObject(this);
    }

    #endregion

    #region Setters

    /// <summary>
    /// Sets the value of a static float attribute. 
    /// <br/> Throws an error if the attribute doesn't exist or isn't a StaticAttribute with type float.
    /// </summary>
    public void SetAttribute(AttributeId id, float value)
    {
        ((StaticAttribute<float>)Attributes[id]).SetValue(value);
    }

    /// <summary>
    /// Changes the value of a static float attribute and clamps it to a range.
    /// <br/> Throws an error if the attribute doesn't exist or isn't a StaticAtttribute with type float.
    /// </summary>
    public void ChangeAttribute(AttributeId id, float deltaValue, float minValue, float maxValue)
    {
        float newValue = Attributes[id].GetValue() + deltaValue;
        newValue = Mathf.Clamp(newValue, minValue, maxValue);
        SetAttribute(id, newValue);
    }

    public void SetTile(WorldTile tile)
    {
        Tile = tile;
    }

    #endregion


    #region Getters

    public SimulationTime Age => ((StaticAttribute<SimulationTime>)Attributes[AttributeId.Age]).GetStaticValue();
    public float MaxHealth => ((RangeAttribute)Attributes[AttributeId.Health]).MaxValue;
    public float Health => Attributes[AttributeId.Health].GetValue();
    public float HealthRatio => ((RangeAttribute)Attributes[AttributeId.Health]).Ratio;
    public NutrientType NutrientType => ((Att_NutrientType)Attributes[AttributeId.NutrientType]).NutrientType;
    public float NutrientValue => Attributes[AttributeId.NutrientValue].GetValue();
    public float EatingDifficulty => Attributes[AttributeId.EatingDifficulty].GetValue();

    /// <summary>
    /// Returns how much nutrients this object would provide to an animal.
    /// </summary>
    public float GetNutrientsForAnimal(Animal animal)
    {
        if (!animal.Diet.Contains(NutrientType)) return 0f;
        else return NutrientValue;
    }

    /// <summary>
    /// Calculates and returns the exact speed of an animal eating this object.
    /// <br/> 1 means {Simulation.EATING_SPEED_MODIFIER %} of the object will be eaten per hour, providing the same % of its nutrients to the animal.
    /// </summary>
    public float GetEatingSpeed(Animal animal)
    {
        return (1f / EatingDifficulty) * animal.EatingSpeed;
    }

    #endregion
}

public enum NutrientType
{
    None,
    Plant,
    FruitVeg,
    Meat,
    Fish,
    Nut,
    Insect,
    Mushroom
}
