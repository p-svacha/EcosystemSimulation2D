using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A VisibleTileObject is a visible object/agent in a specific position on the map.
/// <br/> TileObjects can have inherent logic and act by themselves.
/// </summary>
public abstract class VisibleTileObject : MonoBehaviour, IThing
{
    // IThing // refactor to TileOject
    public ThingId Id => ThingId.Object;
    public string Name => ObjectName;
    public string Description => ObjectDescription;
    public Dictionary<AttributeId, Attribute> Attributes => _Attributes;
    public virtual UI_SelectionWindowContent SelectionWindowContent => ResourceManager.Singleton.SWC_TileObjectBase;

    // General
    public abstract TileObjectType Type { get; } // refactor to TileOject
    public Sprite Sprite => ResourceManager.Singleton.GetTileObjectSprite(Type);

    protected Dictionary<AttributeId, Attribute> _Attributes = new Dictionary<AttributeId, Attribute>(); // refactor to TileOject

    protected List<StatusDisplay> StatusDisplays = new List<StatusDisplay>();

    // Required Attributes  // refactor to TileOject
    protected abstract string ObjectName { get; }
    protected abstract string ObjectDescription { get; }
    protected abstract int MAX_HEALTH { get; }

    // Simulation // refactor to TileOject
    public bool IsSimulated { get; private set; }
    public WorldTile Tile { get; private set; }

    public virtual void Init()
    {
        _Attributes.Add(AttributeId.MaxHealth, new StaticAttribute<float>(this, AttributeId.MaxHealth, "Health", "Max Health", "Maximum amount of HP an object can have.", MAX_HEALTH));
        _Attributes.Add(AttributeId.Health, new StaticAttribute<float>(this, AttributeId.Health, "Health", "Current Health", "Current amount of HP an object has.", MAX_HEALTH));
    }

    protected virtual void Update()
    {
        UpdateStatusDisplays();
        UpdateDeath(); // refactor to TileOject
    }

    private void UpdateStatusDisplays()
    {
        int numActiveStatusDisplays = StatusDisplays.Where(x => x.ShouldShow()).Count();
        int index = 0;
        foreach (StatusDisplay sd in StatusDisplays)
        {
            if(sd.ShouldShow())
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

    public void Die()
    {
        World.Singleton.RemoveObject(this);
    }

    #region Setters

    /// <summary>
    /// Sets the value of a static float attribute. 
    /// <br/> Throws an error if the attribute doesn't exist or isn't a StaticAtttribute with type float.
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
    public void SetIsSimulated(bool value)
    {
        IsSimulated = value;
    }

    #endregion

    #region Getters

    public float MaxHealth => Attributes[AttributeId.MaxHealth].GetValue();
    public float Health => Attributes[AttributeId.Health].GetValue();

    #endregion

}

public enum TileObjectType
{
    TallGrass,
    Creer
} 
