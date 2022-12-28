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
    protected abstract bool REQUIRES_SWIMMING { get; }

    private const string spawnChancePrefix = "Chance per hour that ";
    private const string spawnChanceSuffix = " will start to grow on a tile with this surface.";
    protected virtual float TALL_GRASS_SPAWN_CHANCE => 0f;


    public SurfaceBase()
    {
        _Attributes.Add(AttributeId.MovementCost, new StaticAttribute<float>(AttributeId.MovementCost, "Movement", "Movement Cost", "How hard it is to move across this surface.", MOVEMENT_COST));
        _Attributes.Add(AttributeId.RequiresSwimming, new StaticAttribute<float>(AttributeId.RequiresSwimming, "Movement", "Requires Swimming", "If objects need to be able to swim to traverse this surface.", REQUIRES_SWIMMING? 1 : 0));

        _Attributes.Add(AttributeId.TallGrassSpawnChance, new StaticAttribute<float>(AttributeId.TallGrassSpawnChance, "Production", "Tall Grass Spawn Chance", spawnChancePrefix + "tall grass" + spawnChanceSuffix, TALL_GRASS_SPAWN_CHANCE));
    }

    /// <summary>
    /// Gets called when the world is generated.
    /// </summary>
    public void OnWorldGeneration(WorldTile tile)
    {
        if (Random.value < TALL_GRASS_SPAWN_CHANCE * 50f) World.Singleton.SpawnTileObject(tile, TileObjectId.TallGrass, isNew: false);
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
            if (Random.value < spawnChance && tile.TileObjects.Where(x => x.ObjectId == TileObjectId.TallGrass).Count() == 0) World.Singleton.SpawnTileObject(tile, TileObjectId.TallGrass);
        }
    }

    #region Getters
    public float MovementCost => Attributes[AttributeId.MovementCost].GetValue();
    public bool RequiresSwimming => Attributes[AttributeId.RequiresSwimming].GetValue() == 1f;
    #endregion
}

public enum SurfaceType
{
    Soil,
    Sand,
    Water
}
