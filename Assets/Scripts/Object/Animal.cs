using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : TileObject
{
    // Movement
    protected const float BASE_MOVEMENT_SPEED_MODIFIER = 30f;
    protected List<WorldTile> CurrentPath;
    protected bool IsMoving;

    // Required Attributes
    protected abstract float MOVEMENT_SPEED { get; }
    public float MovementSpeed => Attributes[AttributeId.MovementSpeed].GetValue();
    protected abstract float WATER_MOVEMENT_SPEED { get; }
    public float WaterMovementSpeed => Attributes[AttributeId.WaterMovementSpeed].GetValue();
    public bool CanSwim => (Attributes.ContainsKey(AttributeId.WaterMovementSpeed) && Attributes[AttributeId.WaterMovementSpeed].GetValue() > 0f);

    public override void Init()
    {
        base.Init();

        if (MOVEMENT_SPEED >= 1f) Debug.LogWarning("Movement Speed of an animal is not allowed to be greater than 1 because it breaks Pathfinding.");

        _Attributes.Add(AttributeId.MovementSpeed, new StaticAttribute<float>(this, AttributeId.MovementSpeed, "Movement", "Movement Speed", "How fast this animal is moving.", MOVEMENT_SPEED));
        _Attributes.Add(AttributeId.WaterMovementSpeed, new StaticAttribute<float>(this, AttributeId.WaterMovementSpeed, "Movement", "Water Movement Speed", "How fast this animal is moving on water.", WATER_MOVEMENT_SPEED));
    }

    protected void MoveTo(WorldTile target)
    {
        List<WorldTile> pathToTarget = Pathfinder.GetPath(this, Tile, target);
        SetMovementPath(pathToTarget);
    }

    protected void SetMovementPath(List<WorldTile> path)
    {
        if (path == null || path.Count == 0) return; // no path found

        CurrentPath = path;
        IsMoving = true;

        if (CurrentPath[1].Coordinates.x > CurrentPath[0].Coordinates.x) transform.localScale = new Vector3(1f, 1f, 1f);
        else if (CurrentPath[1].Coordinates.x < CurrentPath[0].Coordinates.x) transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    protected void Update()
    {
        if (!IsSimulated) return;

        if (IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, CurrentPath[1].WorldPosition, (1f / Tile.GetMovementCost(this)) * BASE_MOVEMENT_SPEED_MODIFIER * Simulation.Singleton.LastFrameHoursPassed);

            // Check on which tile the animal is at this moment exactly. Update tile references accordingly.
            WorldTile currentTile = World.Singleton.GetTile(transform.position);
            if (Tile != currentTile)
            {
                Tile.RemoveObject(this);
                SetTile(currentTile);
                Tile.AddObject(this);
            }

            if (transform.position == CurrentPath[1].WorldPosition3) // Character arrived at tile center
            {
                // Update position
                transform.position = CurrentPath[1].WorldPosition;
                if (CurrentPath.Count == 2) // Arrived at destination
                {
                    CurrentPath = null;
                    IsMoving = false;
                }
                else
                {
                    CurrentPath.RemoveAt(0);

                    // Sprite Orientation
                    if (CurrentPath[1].Coordinates.x > CurrentPath[0].Coordinates.x) transform.localScale = new Vector3(1f, 1f, 1f);
                    else if (CurrentPath[1].Coordinates.x < CurrentPath[0].Coordinates.x) transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
        }
    }
}
