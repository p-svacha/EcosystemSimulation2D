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
    protected abstract float VISION_RANGE { get; }

    // Optional Attributes
    protected virtual float EATING_SPEED => 1f;

    // Constant Values (could be replaced by attributes if necessary)
    private float MALNUTRITION_RATE => HUNGER_RATE; // How much malnutrition increases per hour when out of food. 1 Malnutrition = -1 Health per hour
    private float MALNUTRITION_REGENERATION_RATE => 1f; // How much malnutrition decreases per hour when not out of food. 1 Malnutrition = -1 Health per hour

    // Behaviour Logic
    protected List<WorldTile> CurrentPath;
    protected bool IsMoving;
    public string CurrentActivity { get; protected set; }

    public override void Init()
    {
        base.Init();

        if (MOVEMENT_SPEED >= 1f) Debug.LogWarning("Movement Speed of an animal is not allowed to be greater than 1 because it breaks Pathfinding.");

        // Attributes
        _Attributes.Add(AttributeId.MovementSpeed, new StaticAttribute<float>(this, AttributeId.MovementSpeed, "General", "Movement Speed", "How fast this animal is moving.", MOVEMENT_SPEED));
        _Attributes.Add(AttributeId.WaterMovementSpeed, new StaticAttribute<float>(this, AttributeId.WaterMovementSpeed, "General", "Water Movement Speed", "How fast this animal is moving on water.", WATER_MOVEMENT_SPEED));
        _Attributes.Add(AttributeId.VisionRange, new StaticAttribute<float>(this, AttributeId.VisionRange, "General", "Vision Range", "How many tiles an animal can see in all directions and detect specific objects.", VISION_RANGE));

        _Attributes.Add(AttributeId.Diet, new Att_Diet(this, DIET));
        _Attributes.Add(AttributeId.Nutrition, new RangeAttribute(this, AttributeId.Nutrition, "Needs", "Nutrition", "Current and maximum amount of nutrition an animal has.", MAX_NUTRITION, MAX_NUTRITION));
        _Attributes.Add(AttributeId.HungerRate, new StaticAttribute<float>(this, AttributeId.HungerRate, "Needs", "Hunger Rate", "Amount at which the nutrition of an animal drops per hour.", HUNGER_RATE));
        _Attributes.Add(AttributeId.Malnutrition, new StaticAttribute<float>(this, AttributeId.Malnutrition, "Needs", "Malnutrition", "How advanced the malnutrition of an animal is. The higher it is, the more health it loses.", 0f));
        _Attributes.Add(AttributeId.EatingSpeed, new StaticAttribute<float>(this, AttributeId.EatingSpeed, "Needs", "Eating Speed", "How fast an animal is at eating food generally.", EATING_SPEED));

        // Status Displays
        StatusDisplays.Add(new SD_Malnutrition(this));
    }

    #region Update

    protected override void UpdateSelf()
    {
        base.UpdateSelf();

        UpdateNeeds();
        UpdateHealth();
        UpdateActivity();
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, CurrentPath[1].WorldPosition, (1f / Tile.GetMovementCost(this)) * Simulation.MOVEMENT_SPEED_MODIFIER * Simulation.Singleton.LastFrameHoursPassed);

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

    private void UpdateActivity()
    {
        if (IsMoving) return; // Just keep moving when already moving

        // If standing on food and not full, eat that
        if(NutritionRatio < 1f)
        {
            TileObject currentFood = GetCurrentFood();
            if (currentFood != null)
            {
                Eat(currentFood);
                CurrentActivity = "Eating " + currentFood.Name;
                return;
            }
        }

        // Actively for food when hungry
        if (NutritionRatio < 0.25f)
        {
            TileObject closestFood = FindClosestFood((int)VisionRange);
            if (closestFood != null)
            {
                MoveTo(closestFood.Tile);
                CurrentActivity = "Moving to eat " + closestFood.Name;
                return;
            }
        }

        // If there are no important tasks to do, have a chance to wander around
        if (Random.value < 0.001f)
        {
            SetMovementPath(Pathfinder.GetRandomPath(this, Tile, 3));
            CurrentActivity = "Wandering around aimlessly";
            return;
        }

        CurrentActivity = "Standing";
        return;
    }

    private void Eat(TileObject obj)
    {
        // Calculate how much % of the object gets consumed this frame
        float chunkEaten = obj.GetEatingSpeed(this) * Simulation.Singleton.LastFrameHoursPassed;

        // Reduces objects health by that amount
        float lostHealth = obj.MaxHealth * chunkEaten;
        obj.ChangeAttribute(AttributeId.Health, -lostHealth, 0f, obj.MaxHealth);

        // Increase animals nutrition by that amount
        float gainedNutrition = obj.NutrientValue * chunkEaten;
        ChangeAttribute(AttributeId.Nutrition, gainedNutrition, 0f, MaxNutrition);
    }

    #endregion

    #region Behaviour Logic

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

    /// <summary>
    /// Finds the closest object that is edible for this animal.
    /// </summary>
    protected TileObject FindClosestFood(int maxRange)
    {
        int range = 0;
        List<Vector2Int> currentRangePositions = new List<Vector2Int>() { Tile.Coordinates };
        while (range < maxRange)
        {
            foreach (Vector2Int pos in currentRangePositions)
            {
                WorldTile tile = World.Singleton.GetTile(pos);
                foreach (TileObject obj in tile.TileObjects)
                    if (obj.GetNutrientsForAnimal(this) > 0) return obj;
            }
            range++;
            currentRangePositions = HelperFunctions.GetAllPositionsWithRange(Tile.Coordinates, range);
        }
        return null;
    }

    /// <summary>
    /// Returns an edible object the animal is standing on and that can started to be consumed immediately.
    /// </summary>
    protected TileObject GetCurrentFood()
    {
        if (IsMoving) return null;
        foreach (TileObject obj in Tile.TileObjects)
            if (obj.GetNutrientsForAnimal(this) > 0) return obj;
        return null;
    }

    #endregion

    #region Getters

    public float VisionRange => Attributes[AttributeId.VisionRange].GetValue();
    public float MovementSpeed => Attributes[AttributeId.MovementSpeed].GetValue();
    public float WaterMovementSpeed => Attributes[AttributeId.WaterMovementSpeed].GetValue();
    public bool CanSwim => (Attributes.ContainsKey(AttributeId.WaterMovementSpeed) && Attributes[AttributeId.WaterMovementSpeed].GetValue() > 0f);

    public List<NutrientType> Diet => ((Att_Diet)Attributes[AttributeId.Diet]).Diet;
    public float MaxNutrition => ((RangeAttribute)Attributes[AttributeId.Nutrition]).MaxValue;
    public float Nutrition => Attributes[AttributeId.Nutrition].GetValue();
    public float NutritionRatio => ((RangeAttribute)Attributes[AttributeId.Nutrition]).Ratio;
    public float HungerRate => Attributes[AttributeId.HungerRate].GetValue();
    public float Malnutrition => Attributes[AttributeId.Malnutrition].GetValue();
    public float EatingSpeed => Attributes[AttributeId.EatingSpeed].GetValue();

    #endregion
}
