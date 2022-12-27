using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine;

/// <summary>
/// A TileObject is an object/agent in a specific location on the map. TileObjects can have inherent logic and act by themselves.
/// <br/> TileObjects can either be own GameObjects with SpriteRenderes (see VisibleTileObject) or part of the TileMap (see TilemapTileObject).
/// </summary>
public abstract class TileObjectBase : MonoBehaviour, IThing
{
    // IThing
    public ThingId Id => ThingId.Object;
    public string Name => ObjectName;
    public string Description => ObjectDescription;
    public Dictionary<AttributeId, Attribute> Attributes => _Attributes;
    public virtual UI_SelectionWindowContent SelectionWindowContent => ResourceManager.Singleton.SWC_TileObjectBase;

    // Required Attributes
    public abstract TileObjectId ObjectId { get; }
    protected abstract string ObjectName { get; }
    protected abstract string ObjectDescription { get; }
    protected abstract string ObjectCategory { get; }
    protected abstract int HEALTH_BASE { get; }

    // Optional Attributes
    protected virtual NutrientType NUTRIENT_TYPE => NutrientType.None;
    protected virtual float NUTRIENT_VALUE_BASE => 0f;
    protected virtual float EATING_DIFFICULTY => 1f;

    // General
    protected int NumTicks;
    protected Dictionary<AttributeId, Attribute> _Attributes = new Dictionary<AttributeId, Attribute>();
    protected Dictionary<AttributeId, float> FloatAttributeCache = new Dictionary<AttributeId, float>();
    public List<StatusEffect> StatusEffects { get; private set; }
    private List<StatusEffect> StatusEffectsToRemove; // used to not break iterator
    public List<StatusDisplay> StatusEffectDisplays { get; private set; }
    public List<ConditionalStatusDisplay> ConditionalStatusDisplays { get; private set; }
    public WorldTile Tile { get; private set; }

    #region Initialize

    /// <summary>
    /// Gets called when a new TileObject gets created.
    /// </summary>
    public virtual void Init()
    {
        // Init attributes
        _Attributes.Add(AttributeId.CreatedAt, new TimeAttribute(this, AttributeId.CreatedAt, "General", "Created At", "Time at which an object has been created.", Simulation.Singleton.CurrentTime.Copy()));
        _Attributes.Add(AttributeId.Age, new Att_Age(this));
        _Attributes.Add(AttributeId.HealthBase, new StaticAttribute<float>(this, AttributeId.HealthBase, "General", "Base Health", "Base max health of an object.", HEALTH_BASE));
        _Attributes.Add(AttributeId.Health, new Att_Health(this));

        _Attributes.Add(AttributeId.NutrientType, new Att_NutrientType(this, NUTRIENT_TYPE));
        _Attributes.Add(AttributeId.NutrientValueBase, new StaticAttribute<float>(this, AttributeId.NutrientValueBase, "Nutrition", "Base Nutrient Value", "Base amount of nutrition an object provides at when being eaten from full health to 0.", NUTRIENT_VALUE_BASE));
        _Attributes.Add(AttributeId.NutrientValue, new Att_NutrientValue(this));
        _Attributes.Add(AttributeId.EatingDifficulty, new StaticAttribute<float>(this, AttributeId.EatingDifficulty, "Nutrition", "Eating Difficulty", "How difficult an object is to eat generally.", EATING_DIFFICULTY));

        // Status effects
        StatusEffects = new List<StatusEffect>();
        StatusEffectsToRemove = new List<StatusEffect>();

        // Status displays
        StatusEffectDisplays = new List<StatusDisplay>();
        ConditionalStatusDisplays = new List<ConditionalStatusDisplay>();
    }

    /// <summary>
    /// Triggers after all Init calls are done.
    /// </summary>
    public virtual void LateInit()
    {
        Health.Init(initialRatio: 1f);
    }

    #endregion

    #region Update

    // Performance Profilers
    static readonly ProfilerMarker pm_all = new ProfilerMarker("Update TileObject");
    static readonly ProfilerMarker pm_cache = new ProfilerMarker("Clear Attribute Cache");
    static readonly ProfilerMarker pm_statusEffects = new ProfilerMarker("Update Status Effects");
    static readonly ProfilerMarker pm_statusDisplays = new ProfilerMarker("Update Status Displays");
    static readonly ProfilerMarker pm_health = new ProfilerMarker("Update Health");

    /// <summary>
    /// Tick gets called every frame when the simulation is running.
    /// </summary>
    public virtual void Tick()
    {
        pm_all.Begin();
        NumTicks++;

        pm_cache.Begin();
        ClearAttributeCache();
        pm_cache.End();

        pm_health.Begin();
        if(NumTicks % 60 == 0) UpdateHealth();
        pm_health.End();

        pm_statusEffects.Begin();
        UpdateStatusEffects();
        pm_statusEffects.End();

        pm_statusDisplays.Begin();
        UpdateStatusDisplays();
        pm_statusDisplays.End();

        pm_all.End();
    }

    private void ClearAttributeCache()
    {
        FloatAttributeCache.Clear();
    }

    private void UpdateStatusEffects()
    {
        if (StatusEffects.Count == 0) return;
        foreach (StatusEffect statusEffect in StatusEffects) statusEffect.Tick();
        foreach (StatusEffect toRemove in StatusEffectsToRemove) StatusEffects.Remove(toRemove);
        StatusEffectsToRemove.Clear();
    }

    private void UpdateStatusDisplays()
    {
        if (ConditionalStatusDisplays.Count == 0 && StatusEffectDisplays.Count == 0) return;
        int numActiveStatusDisplays = ConditionalStatusDisplays.Where(x => x.ShouldShow()).Count() + StatusEffectDisplays.Count;
        int index = 0;

        // from Status Effect
        foreach (StatusDisplay sd in StatusEffectDisplays)
        {
            if (sd.WorldDisplayObject != null) sd.WorldDisplayObject.UpdateDisplay(index, numActiveStatusDisplays);
            else sd.CreateWorldDisplay(transform, index, numActiveStatusDisplays);
            index++;
        }

        // Conditional
        foreach (ConditionalStatusDisplay csd in ConditionalStatusDisplays)
        {
            if (csd.ShouldShow())
            {
                if (csd.WorldDisplayObject != null) csd.WorldDisplayObject.UpdateDisplay(index, numActiveStatusDisplays);
                else csd.CreateWorldDisplay(transform, index, numActiveStatusDisplays);
                index++;
            }
            else
            {
                if (csd.WorldDisplayObject != null) Destroy(csd.WorldDisplayObject.gameObject);
            }
        }
    }

    /// <summary>
    /// Updates the maximum health and makes a check if the object is dead.
    /// </summary>
    private void UpdateHealth()
    {
        Health.CalculateNewValues();
        if (Health.Value == 0) Die();
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
    public void ChangeStaticAttribute(AttributeId id, float deltaValue, float minValue, float maxValue)
    {
        float newValue = Attributes[id].GetValue() + deltaValue;
        newValue = Mathf.Clamp(newValue, minValue, maxValue);
        SetAttribute(id, newValue);
    }

    public void SetTile(WorldTile tile)
    {
        Tile = tile;
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        statusEffect.Init(this);
        StatusEffects.Add(statusEffect);
        StatusEffectDisplays.Add(statusEffect.Display);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusEffect.End();
        StatusEffectsToRemove.Add(statusEffect);

        // Display
        if (statusEffect.Display.WorldDisplayObject != null) Destroy(statusEffect.Display.WorldDisplayObject.gameObject);
        StatusEffectDisplays.Remove(statusEffect.Display);
    }

    #endregion


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

    public bool HasStatusEffect(StatusEffectId id) => StatusEffects.Any(x => x.Id == id);

    public float Age => GetFloatAttribute(AttributeId.Age);
    public DynamicRangeAttribute Health => Attributes[AttributeId.Health] as DynamicRangeAttribute;
    public NutrientType NutrientType => ((Att_NutrientType)Attributes[AttributeId.NutrientType]).NutrientType;
    public float NutrientValue => GetFloatAttribute(AttributeId.NutrientValue);
    public float EatingDifficulty => GetFloatAttribute(AttributeId.EatingDifficulty);

    /// <summary>
    /// Returns how much nutrients this object would provide to an animal.
    /// </summary>
    public float GetNutrientsForAnimal(AnimalBase animal)
    {
        if (!animal.Diet.Contains(NutrientType)) return 0f;
        else return NutrientValue;
    }

    /// <summary>
    /// Calculates and returns the exact speed of an animal eating this object.
    /// <br/> 1 means {Simulation.EATING_SPEED_MODIFIER %} of the object will be eaten per hour, providing the same % of its nutrients to the animal.
    /// </summary>
    public float GetEatingSpeed(AnimalBase animal)
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
