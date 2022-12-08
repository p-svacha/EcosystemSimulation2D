using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : VisibleTileObject
{
    // IThing
    public override UI_SelectionWindowContent SelectionWindowContent => ResourceManager.Singleton.SWC_AnimalBase;

    // Required Attributes
    protected abstract float MOVEMENT_SPEED { get; }
    protected abstract float WATER_MOVEMENT_SPEED { get; }
    protected abstract float MAX_NUTRITION { get; }
    protected abstract float HUNGER_RATE { get; }
    protected abstract List<NutrientType> DIET { get; }

    // Constant Values (could be replaced by attributes if necessary)
    private float MALNUTRITION_RATE => HUNGER_RATE; // How much malnutrition increases per hour when out of food. 1 Malnutrition = -1 Health per hour
    private float MALNUTRITION_REGENERATION_RATE => 1f; // How much malnutrition decreases per hour when not out of food. 1 Malnutrition = -1 Health per hour

    // Movement Logic
    protected const float BASE_MOVEMENT_SPEED_MODIFIER = 30f;
    protected List<WorldTile> CurrentPath;
    protected bool IsMoving;

    public override void Init()
    {
        base.Init();

        if (MOVEMENT_SPEED >= 1f) Debug.LogWarning("Movement Speed of an animal is not allowed to be greater than 1 because it breaks Pathfinding.");

        // Attributes
        _Attributes.Add(AttributeId.MovementSpeed, new StaticAttribute<float>(this, AttributeId.MovementSpeed, "Movement", "Movement Speed", "How fast this animal is moving.", MOVEMENT_SPEED));
        _Attributes.Add(AttributeId.WaterMovementSpeed, new StaticAttribute<float>(this, AttributeId.WaterMovementSpeed, "Movement", "Water Movement Speed", "How fast this animal is moving on water.", WATER_MOVEMENT_SPEED));

        _Attributes.Add(AttributeId.Diet, new Att_Diet(this, DIET));
        _Attributes.Add(AttributeId.MaxNutrition, new StaticAttribute<float>(this, AttributeId.MaxNutrition, "Needs", "Max Nutrition", "Maximum amount of nutrition an animal can store.", MAX_NUTRITION));
        _Attributes.Add(AttributeId.Nutrition, new StaticAttribute<float>(this, AttributeId.Nutrition, "Needs", "Nutrition", "Current amount of nutrition an animal has.", MAX_NUTRITION));
        _Attributes.Add(AttributeId.HungerRate, new StaticAttribute<float>(this, AttributeId.HungerRate, "Needs", "Hunger Rate", "Amount at which the nutrition of an animal drops per hour.", HUNGER_RATE));
        _Attributes.Add(AttributeId.Malnutrition, new StaticAttribute<float>(this, AttributeId.Malnutrition, "Needs", "Malnutrition", "How advanced the malnutrition of an animal is. The higher it is, the more health it loses.", 0f));

        // Status Displays
        StatusDisplays.Add(new SD_Malnutrition(this));
    }

    #region Update

    protected override void UpdateSelf()
    {
        base.UpdateSelf();

        UpdateMovement();
        UpdateNeeds();
        UpdateHealth();
    }

    private void UpdateMovement()
    {
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
                    if (CurrentPath[1].Coordinates.x > CurrentPath[0].Coordinates.x) GetComponent<SpriteRenderer>().flipX = false;
                    else if (CurrentPath[1].Coordinates.x < CurrentPath[0].Coordinates.x) GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }
    }
    private void UpdateNeeds()
    {
        // Nutrition
        ChangeAttribute(AttributeId.Nutrition, -(HungerRate * Simulation.Singleton.LastFrameHoursPassed), 0f, MaxNutrition); // Decrease Nutrition by HungerRate
        if (Nutrition == 0) ChangeAttribute(AttributeId.Malnutrition, MALNUTRITION_RATE * Simulation.Singleton.LastFrameHoursPassed, 0f, 1000f); // Increase malnutrition
        else ChangeAttribute(AttributeId.Malnutrition, -(MALNUTRITION_REGENERATION_RATE * Simulation.Singleton.LastFrameHoursPassed), 0f, 1000f); // Decrease malnutrition
    }
    private void UpdateHealth()
    {
        if (Malnutrition > 0) ChangeAttribute(AttributeId.Health, -(Malnutrition * Simulation.Singleton.LastFrameHoursPassed), 0f, MaxHealth); // Decrease health by malnutrition
    }

    #endregion


    protected void MoveTo(WorldTile target)
    {
        List<WorldTile> pathToTarget = Pathfinder.GetPath(this, Tile, target);
        SetMovementPath(pathToTarget);
    }

    protected void SetMovementPath(List<WorldTile> path)
    {
        if (path == null || path.Count <= 1) return; // no path found

        CurrentPath = path;
        IsMoving = true;

        if (CurrentPath[1].Coordinates.x > CurrentPath[0].Coordinates.x) GetComponent<SpriteRenderer>().flipX = false;
        else if (CurrentPath[1].Coordinates.x < CurrentPath[0].Coordinates.x) GetComponent<SpriteRenderer>().flipX = true;
    }

    #region Getters

    public float MovementSpeed => Attributes[AttributeId.MovementSpeed].GetValue();
    public float WaterMovementSpeed => Attributes[AttributeId.WaterMovementSpeed].GetValue();
    public bool CanSwim => (Attributes.ContainsKey(AttributeId.WaterMovementSpeed) && Attributes[AttributeId.WaterMovementSpeed].GetValue() > 0f);

    public List<NutrientType> Diet => ((Att_Diet)Attributes[AttributeId.Diet]).Diet;
    public float MaxNutrition => Attributes[AttributeId.MaxNutrition].GetValue();
    public float Nutrition => Attributes[AttributeId.Nutrition].GetValue();
    public float HungerRate => Attributes[AttributeId.HungerRate].GetValue();
    public float Malnutrition => Attributes[AttributeId.Malnutrition].GetValue();

    #endregion
}
