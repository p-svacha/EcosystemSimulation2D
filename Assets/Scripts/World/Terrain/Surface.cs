using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A surface represents one type of terrain. The higher the precedence the more prominent it gets drawn.
/// </summary>
public abstract class Surface : IThing
{
    // IThing
    public ThingId Id => ThingId.Surface;
    public string Name => SurfaceName;
    public string Description => SurfaceDescription;
    public Dictionary<AttributeId, Attribute> Attributes => _Attributes;
    public UI_SelectionWindowContent SelectionWindowContent => null;

    protected Dictionary<AttributeId, Attribute> _Attributes = new Dictionary<AttributeId, Attribute>();

    // Required Attributes

    /// <summary>
    /// Unique identifier.
    /// </summary>
    public abstract SurfaceType Type { get; }

    /// <summary>
    /// Determines the overlay order between surfaces. The higher the precedence, the more other surface types this type overlays.
    /// <br/>Precedence must be unique.
    /// </summary>
    public abstract int Precedence { get; }
    protected abstract string SurfaceName { get; }
    protected abstract string SurfaceDescription { get; }

    protected abstract float MOVEMENT_COST { get; }
    public float MovementCost => Attributes[AttributeId.MovementCost].GetValue();
    protected abstract bool REQUIRES_SWIMMING { get; }
    public bool RequiresSwimming => Attributes[AttributeId.RequiresSwimming].GetValue() == 1f;


    private World World;
    public Surface(World world)
    {
        World = world;

        _Attributes.Add(AttributeId.MovementCost, new StaticAttribute<float>(this, AttributeId.MovementCost, "Movement", "Movement Cost", "How hard it is to move across this surface.", MOVEMENT_COST));
        _Attributes.Add(AttributeId.RequiresSwimming, new StaticAttribute<float>(this, AttributeId.RequiresSwimming, "Movement", "Requires Swimming", "If objects need to be able to swim to traverse this surface.", REQUIRES_SWIMMING? 1 : 0));
    }

    /// <summary>
    /// Gets called every n'th frame.
    /// </summary>
    public void Tick(WorldTile tile, float hoursSinceLastUpdate)
    {
        Attribute att;
        if(Attributes.TryGetValue(AttributeId.TallGrassSpawnChance, out att))
        {
            float spawnChance = hoursSinceLastUpdate * att.GetValue();
            if (Random.value < spawnChance) World.SpawnTileObject(tile, TileObjectType.TallGrass);
        }
    }
}

public enum SurfaceType
{
    Soil,
    Sand,
    Water
}
