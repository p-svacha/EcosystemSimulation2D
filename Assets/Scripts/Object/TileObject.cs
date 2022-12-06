using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A TileObject is a object/agent in a specific position on the map.
/// <br/> TileObjects can have inherent logic and act by themselves.
/// </summary>
public abstract class TileObject : MonoBehaviour, IThing
{
    // IThing
    public ThingId Id => ThingId.Object;
    public string Name => ObjectName;
    public string Description => ObjectDescription;
    public Dictionary<AttributeId, Attribute> Attributes => _Attributes;
    public virtual UI_SelectionWindowContent SelectionWindowContent => ResourceManager.Singleton.SWC_TileObjectBase;

    // General
    public abstract TileObjectType Type { get; }
    public Sprite Sprite => ResourceManager.Singleton.GetTileObjectSprite(Type);

    protected Dictionary<AttributeId, Attribute> _Attributes = new Dictionary<AttributeId, Attribute>();

    // Required Attributes
    protected abstract string ObjectName { get; }
    protected abstract string ObjectDescription { get; }
    protected abstract int MAX_HEALTH { get; }

    // Simulation
    public bool IsSimulated { get; private set; }
    public WorldTile Tile { get; private set; }

    public virtual void Init()
    {
        _Attributes.Add(AttributeId.MaxHealth, new StaticAttribute<float>(this, AttributeId.MaxHealth, "Health", "Max Health", "Maximum amount of HP an object can have.", MAX_HEALTH));
        _Attributes.Add(AttributeId.Health, new StaticAttribute<float>(this, AttributeId.Health, "Health", "Current Health", "Current amount of HP an object has.", MAX_HEALTH));
    }

    public void SetTile(WorldTile tile)
    {
        Tile = tile;
    }
    public void SetIsSimulated(bool value)
    {
        IsSimulated = value;
    }

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
