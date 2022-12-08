using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all information for a single tile
/// </summary>
public class WorldTile : IThing
{
    // IThing
    public ThingId Id => ThingId.Tile;
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
    public Surface Surface { get; private set; }
    public List<TileObject> TileObjects = new List<TileObject>();

    // Attributes
    private Dictionary<AttributeId, Attribute> _Attributes = new Dictionary<AttributeId, Attribute>();
    private float MovementCost => Attributes[AttributeId.MovementCost].GetValue();

    public WorldTile(World world, Vector2Int coordinates)
    {
        World = world;
        Coordinates = coordinates;
        WorldPosition = new Vector2(coordinates.x + 0.5f, coordinates.y + 0.5f);

        // Attributes
        _Attributes.Add(AttributeId.Coordinates, new StaticAttribute<string>(this, AttributeId.Coordinates, "General", "Coordinates", "Position of this tile on the world map.", Coordinates.x + " / " + Coordinates.y));
        _Attributes.Add(AttributeId.Surface, new StaticAttribute<string>(this, AttributeId.Surface, "General", "Surface", "Surface type of this tile.", ""));
        _Attributes.Add(AttributeId.MovementCost, new Att_MovementCost(this));
    }

    /// <summary>
    /// Gets called every n'th frame. See Simulation > TILE_UPDATE_POTS.
    /// </summary>
    public void Update(float hoursSinceLastUpdate)
    {
        Surface.Update(this, hoursSinceLastUpdate);
    }

    #region UI

    public void SelectNextLayer(IThing sourceThing)
    {
        if (sourceThing is VisibleTileObject tileObject)
        {
            int index = TileObjects.IndexOf(tileObject);
            if (index == TileObjects.Count - 1) UIHandler.Singleton.AddSelectionWindow(TileObjects[0]);
            else UIHandler.Singleton.AddSelectionWindow(TileObjects[index + 1]);
        }
    }

    #endregion

    #region Getters

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
    public bool IsPassable(Animal animal)
    {
        if (Surface.RequiresSwimming && !animal.CanSwim) return false;
        return true;
    }

    /// <summary>
    /// Calculates the exact MovementCost of an animal on this tile.
    /// <br/> The higher the cost, the slower the animal will move on the tile.
    /// </summary>
    public float GetMovementCost(Animal animal)
    {
        return MovementCost / animal.MovementSpeed;
    }

    #endregion

    #region Setters

    public void SetSurface(Surface surface)
    {
        Surface = surface;
        ((StaticAttribute<string>)_Attributes[AttributeId.Surface]).SetValue(surface.Name);
    }

    public void AddObject(TileObject tileObject)
    {
        TileObjects.Add(tileObject);
    }

    public void RemoveObject(TileObject obj)
    {
        TileObjects.Remove(obj);
    }

    #endregion

}
