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
    public UI_SelectionWindowContent SelectionWindowContent => ResourceManager.Singleton.SWC_TileObjectBase;

    // General
    public abstract TileObjectType Type { get; }
    protected abstract string ObjectName { get; }
    protected abstract string ObjectDescription { get; }
    public Sprite Sprite => ResourceManager.Singleton.GetTileObjectSprite(Type);

    protected Dictionary<AttributeId, Attribute> _Attributes = new Dictionary<AttributeId, Attribute>();

    // Simulation
    public bool IsSimulated { get; private set; }
    public WorldTile Tile { get; private set; }

    public virtual void Init() { }

    public void SetTile(WorldTile tile)
    {
        Tile = tile;
    }
    public void SetIsSimulated(bool value)
    {
        IsSimulated = value;
    }

}

public enum TileObjectType
{
    TallGrass,
    Creer
} 
