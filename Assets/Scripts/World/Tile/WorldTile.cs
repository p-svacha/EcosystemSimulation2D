using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Attribute;

/// <summary>
/// Contains all information for a single tile
/// </summary>
public class WorldTile : IThing
{
    // IThing
    public ThingId ThingId => ThingId.Tile;
    public string Name => Surface.Name + " Tile";
    public string Description => "A " + Surface.Name + " tile in the world.";
    public Dictionary<AttributeId, Attribute> Attributes => _Attributes;
    public UI_SelectionWindowContent SelectionWindowContent => null;

    // World
    public World World { get; private set; }
    public Vector2Int Coordinates { get; private set; }
    public Vector3Int Coordinates3 => new Vector3Int(Coordinates.x, Coordinates.y, 0);
    public Vector2 WorldPosition { get; private set; }
    public Vector3 WorldPosition3 => new Vector3(WorldPosition.x, WorldPosition.y, 0);

    // Content
    public SurfaceBase Surface { get; private set; }
    public List<TileObjectBase> TileObjects = new List<TileObjectBase>();
    /// <summary> Elevation that each corner of this tile has. Order is NW, NE, SE, SW. </summary>
    public int[] Elevation { get; private set; }
    public int MinElevation { get; private set; }
    public int MaxElevation { get; private set; }
    public TileElevationType ElevationType { get; private set; }
    /// <summary> Direction in which this tile is facing upwards towards higher elevation </summary>
    public TileElevationDirection ElevationDirection { get; private set; }

    // Attributes
    private Dictionary<AttributeId, Attribute> _Attributes = new Dictionary<AttributeId, Attribute>();
    protected Dictionary<AttributeId, float> FloatAttributeCache = new Dictionary<AttributeId, float>();

    public WorldTile(World world, Vector2Int coordinates)
    {
        World = world;
        Coordinates = coordinates;
        WorldPosition = new Vector2(coordinates.x + 0.5f, coordinates.y + 0.5f);

        // Attributes
        _Attributes.Add(AttributeId.Coordinates, new StaticAttribute<string>(AttributeId.Coordinates, "Coordinates", "General", Coordinates.x + " / " + Coordinates.y));
        _Attributes.Add(AttributeId.Surface, new StaticAttribute<string>(AttributeId.Surface, "Surface", "General", ""));
        _Attributes.Add(AttributeId.MovementCost, new Att_MovementCost(this));
    }

    public void SetElevation(int[] elevation)
    {
        Elevation = elevation;
        MinElevation = elevation.Min();
        MaxElevation = elevation.Max();
        int elevationDiff = MaxElevation - MinElevation;
        if (elevationDiff == 0) ElevationType = TileElevationType.Flat;
        else if(elevationDiff == 1) ElevationType = TileElevationType.Slope;
        else if(elevationDiff > 1) ElevationType = TileElevationType.Cliff;
        UpdateElevationDirection();
    }

    private void UpdateElevationDirection()
    {
        if (MinElevation == MaxElevation) ElevationDirection = TileElevationDirection.None;
        else if (Elevation[0] == MaxElevation && Elevation[1] == MaxElevation && Elevation[2] == MaxElevation) ElevationDirection = TileElevationDirection.Side_NE;
        else if (Elevation[1] == MaxElevation && Elevation[2] == MaxElevation && Elevation[3] == MaxElevation) ElevationDirection = TileElevationDirection.Side_SE;
        else if (Elevation[2] == MaxElevation && Elevation[3] == MaxElevation && Elevation[0] == MaxElevation) ElevationDirection = TileElevationDirection.Side_SW;
        else if (Elevation[3] == MaxElevation && Elevation[0] == MaxElevation && Elevation[1] == MaxElevation) ElevationDirection = TileElevationDirection.Side_NW;
        else if (Elevation[0] == MaxElevation && Elevation[1] == MaxElevation) ElevationDirection = TileElevationDirection.Side_N;
        else if (Elevation[1] == MaxElevation && Elevation[2] == MaxElevation) ElevationDirection = TileElevationDirection.Side_E;
        else if (Elevation[2] == MaxElevation && Elevation[3] == MaxElevation) ElevationDirection = TileElevationDirection.Side_S;
        else if (Elevation[3] == MaxElevation && Elevation[0] == MaxElevation) ElevationDirection = TileElevationDirection.Side_W;
        else if (Elevation[0] == MaxElevation && Elevation[2] == MaxElevation) ElevationDirection = TileElevationDirection.Corners_NW_SE;
        else if (Elevation[1] == MaxElevation && Elevation[3] == MaxElevation) ElevationDirection = TileElevationDirection.Corners_NE_SW;
        else if (Elevation[0] == MaxElevation) ElevationDirection = TileElevationDirection.Corner_NW;
        else if (Elevation[1] == MaxElevation) ElevationDirection = TileElevationDirection.Corner_NE;
        else if (Elevation[2] == MaxElevation) ElevationDirection = TileElevationDirection.Corner_SE;
        else if (Elevation[3] == MaxElevation) ElevationDirection = TileElevationDirection.Corner_SW;
    }

    /// <summary>
    /// Gets called when the world is generated.
    /// </summary>
    public void OnWorldGeneration()
    {
        Surface.OnWorldGeneration(this);
    }

    /// <summary>
    /// Gets called every n'th frame.
    /// </summary>
    public void Tick(float hoursSinceLastUpdate)
    {
        FloatAttributeCache.Clear();
        Surface.Tick(this, hoursSinceLastUpdate);
    }

    #region UI

    public void SelectNextLayer(IThing sourceThing)
    {
        if (sourceThing is VisibleTileObjectBase tileObject)
        {
            int index = TileObjects.IndexOf(tileObject);
            if (index == TileObjects.Count - 1) UIHandler.Singleton.AddSelectionWindow(TileObjects[0]);
            else UIHandler.Singleton.AddSelectionWindow(TileObjects[index + 1]);
        }
    }

    #endregion

    #region Getters

    /// <summary>
    /// Returns the value of a DynamicAttribute and caches it for the remainder of the frame.
    /// </summary>
    protected float GetFloatAttribute(AttributeId id)
    {
        if (FloatAttributeCache.TryGetValue(id, out float cachedValue)) return cachedValue;

        float value = Attributes[id].GetValue();
        FloatAttributeCache[id] = value;
        return value;
    }
    private float MovementCost => GetFloatAttribute(AttributeId.MovementCost);


    /// <summary>
    /// Returns all existing adjacent tiles of this tile.
    /// </summary>
    public List<WorldTile> GetAdjacentTiles()
    {
        List<WorldTile> adjacentTiles = new List<WorldTile>();
        foreach (Vector2Int pos in HelperFunctions.GetAdjacentCoordinates(Coordinates))
        {
            WorldTile adjacentTile = World.GetTile(pos);
            if (adjacentTile != null) adjacentTiles.Add(adjacentTile);
        }

        return adjacentTiles;
    }

    /// <summary>
    /// Returns if a specified thing can traverse this tile.
    /// </summary>
    public bool IsPassable(AnimalBase animal)
    {
        if (Surface.RequiresSwimming && !animal.CanSwim) return false;
        if (ElevationType == TileElevationType.Cliff) return false;
        return true;
    }

    /// <summary>
    /// Calculates the exact MovementCost of an animal on this tile.
    /// <br/> The higher the cost, the slower the animal will move on the tile.
    /// </summary>
    public float GetMovementCost(AnimalBase animal)
    {
        return MovementCost / animal.LandMovementSpeed;
    }

    #endregion

    #region Setters

    public void SetSurface(SurfaceBase surface)
    {
        Surface = surface;
        ((StaticAttribute<string>)_Attributes[AttributeId.Surface]).SetValue(surface.Name);
    }

    public void AddObject(TileObjectBase tileObject)
    {
        TileObjects.Add(tileObject);
    }

    public void RemoveObject(TileObjectBase obj)
    {
        TileObjects.Remove(obj);
    }

    #endregion

}

public enum TileElevationType
{
    Flat,
    Slope,
    Cliff
}

/// <summary> Direction in which a tile is facing upwards towards higher elevation </summary>
public enum TileElevationDirection
{
    None,
    Corner_NE,
    Corner_NW,
    Corner_SW,
    Corner_SE,
    Corners_NE_SW,
    Corners_NW_SE,
    Side_N,
    Side_E,
    Side_S,
    Side_W,
    Side_NE,
    Side_NW,
    Side_SW,
    Side_SE
}
