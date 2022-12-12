using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public abstract class AnimalBase : OrganismBase
{
    // IThing
    public override UI_SelectionWindowContent SelectionWindowContent => ResourceManager.Singleton.SWC_AnimalBase;

    // Required Attributes
    protected abstract float MOVEMENT_SPEED_BASE { get; }
    protected abstract float WATER_MOVEMENT_SPEED { get; }
    protected abstract float NUTRITION_BASE { get; }
    protected abstract float HUNGER_RATE_BASE { get; }
    protected abstract List<NutrientType> DIET { get; }
    protected abstract float VISION_RANGE { get; }
    protected abstract SimulationTime PREGNANCY_MAX_AGE { get; }
    protected abstract float PREGNANCY_CHANCE_BASE { get; }
    protected abstract SimulationTime PREGNANCY_DURATION { get; }
    protected abstract int MIN_NUM_OFFSPRING { get; }
    protected abstract int MAX_NUM_OFFSPRING { get; }

    // Optional Attributes
    protected virtual float EATING_SPEED => 1f;

    // Constant Values (could be replaced by attributes if necessary)
    private float MALNUTRITION_RATE => HUNGER_RATE_BASE; // How much malnutrition increases per hour when out of food. 1 Malnutrition = -1 Health per hour
    private float MALNUTRITION_REGENERATION_RATE => HUNGER_RATE_BASE; // How much malnutrition decreases per hour when not out of food. 1 Malnutrition = -1 Health per hour

    // Behaviour Logic
    protected List<WorldTile> CurrentPath;
    protected bool IsMoving;
    public string CurrentActivity { get; protected set; }

    #region Initialize

    public override void Init()
    {
        base.Init();

        if (MOVEMENT_SPEED_BASE >= 1f) Debug.LogWarning("Movement Speed of an animal is not allowed to be greater than 1 because it breaks Pathfinding.");

        // Attributes
        _Attributes.Add(AttributeId.LandMovementSpeedBase, new StaticAttribute<float>(this, AttributeId.LandMovementSpeedBase, "General", "Base Movement Speed", "Base speed at which an animal moves on land.", MOVEMENT_SPEED_BASE));
        _Attributes.Add(AttributeId.LandMovementSpeed, new Att_MovementSpeed(this));
        _Attributes.Add(AttributeId.WaterMovementSpeed, new StaticAttribute<float>(this, AttributeId.WaterMovementSpeed, "General", "Water Movement Speed", "Base speed at which an animal moves on water.", WATER_MOVEMENT_SPEED));
        _Attributes.Add(AttributeId.VisionRange, new StaticAttribute<float>(this, AttributeId.VisionRange, "General", "Vision Range", "How many tiles an animal can see in all directions and detect specific objects.", VISION_RANGE));

        _Attributes.Add(AttributeId.Diet, new Att_Diet(this, DIET));
        _Attributes.Add(AttributeId.NutritionBase, new StaticAttribute<float>(this, AttributeId.NutritionBase, "Needs", "Base Nutrition", "Base amount of nutrition an animal can store.", NUTRITION_BASE));
        _Attributes.Add(AttributeId.Nutrition, new Att_Nutrition(this));
        _Attributes.Add(AttributeId.HungerRateBase, new StaticAttribute<float>(this, AttributeId.HungerRateBase, "Needs", "Base Hunger Rate", "Base amount at which the nutrition of an animal drops per hour.", HUNGER_RATE_BASE));
        _Attributes.Add(AttributeId.HungerRate, new Att_HungerRate(this));
        _Attributes.Add(AttributeId.Malnutrition, new StaticAttribute<float>(this, AttributeId.Malnutrition, "Needs", "Malnutrition", "How advanced the malnutrition of an animal is. The higher it is, the more health it loses.", 0f));
        _Attributes.Add(AttributeId.EatingSpeed, new StaticAttribute<float>(this, AttributeId.EatingSpeed, "Needs", "Eating Speed", "How fast an animal is at eating food generally.", EATING_SPEED));

        _Attributes.Add(AttributeId.PregnancyMaxAge, new StaticAttribute<SimulationTime>(this, AttributeId.PregnancyMaxAge, "Reproduction", "Maximum Age for Pregnancy", "Maximum age at which an animal can get pregnant.", PREGNANCY_MAX_AGE));
        _Attributes.Add(AttributeId.PregnancyDuration, new StaticAttribute<SimulationTime>(this, AttributeId.PregnancyDuration, "Reproduction", "Pregnancy Duration", "How long an animal is pregnant for.", PREGNANCY_DURATION));
        _Attributes.Add(AttributeId.PregnancyProgress, new StaticAttribute<SimulationTime>(this, AttributeId.PregnancyProgress, "Reproduction", "Pregnancy Progress", "How long an animal has been pregnant for.", new SimulationTime()));
        _Attributes.Add(AttributeId.PregnancyChanceBase, new StaticAttribute<float>(this, AttributeId.PregnancyChanceBase, "Reproduction", "Base Pregnancy Chance", "Base chance per hour that an animal gets pregnant. Actualy chance depends on a lot of factors like age and health.", PREGNANCY_CHANCE_BASE));
        _Attributes.Add(AttributeId.PregnancyChance, new Att_PregnancyChance(this));
        _Attributes.Add(AttributeId.MinNumOffspring, new StaticAttribute<int>(this, AttributeId.MinNumOffspring, "Reproduction", "Minimum amount of offspring", "Minimum amount of children an animal will produce when giving birth.", MIN_NUM_OFFSPRING));
        _Attributes.Add(AttributeId.MaxNumOffspring, new StaticAttribute<int>(this, AttributeId.MaxNumOffspring, "Reproduction", "Maximum amount of offspring", "Minimum amount of children an animal will produce when giving birth.", MAX_NUM_OFFSPRING));

        // Status Displays
        ConditionalStatusDisplays.Add(new SD_LowHealth(this));
    }

    public override void LateInit()
    {
        base.LateInit();
        Nutrition.Init(initialRatio: 1f);
    }

    #endregion

    #region Update

    // Performance Profilers
    static readonly ProfilerMarker pm_all = new ProfilerMarker("Update AnimalBase");
    static readonly ProfilerMarker pm_needs = new ProfilerMarker("Update Needs");
    static readonly ProfilerMarker pm_reproduction = new ProfilerMarker("Update Reproduction");
    static readonly ProfilerMarker pm_activity = new ProfilerMarker("Update Activity");
    static readonly ProfilerMarker pm_movement = new ProfilerMarker("Update Movement");

    public override void Tick()
    {
        pm_all.Begin();
        base.Tick();

        pm_needs.Begin();
        UpdateNeeds();
        pm_needs.End();

        pm_reproduction.Begin();
        UpdateReproduction();
        pm_reproduction.End();


        pm_activity.Begin();
        UpdateActivity();
        pm_activity.End();

        pm_movement.Begin();
        UpdateMovement();
        pm_movement.End();

        pm_all.End();
    }

    private void UpdateReproduction()
    {
        if (PregnancyChance > 0 && Random.value < (PregnancyChance * Simulation.Singleton.TickTime) && !HasStatusEffect(StatusEffectId.Pregnancy)) // Get pregnant
        {
            AddStatusEffect(new SE_Pregnancy());
        }
    }
    
    private void UpdateNeeds()
    {
        // Nutrition
        Nutrition.CalculateNewValues();
        Nutrition.ChangeValue(-(HungerRate * Simulation.Singleton.TickTime)); // Decrease Nutrition by HungerRate
        if (Nutrition.Value == 0) ChangeStaticAttribute(AttributeId.Malnutrition, MALNUTRITION_RATE * Simulation.Singleton.TickTime, 0f, 1000f); // Increase malnutrition
        else ChangeStaticAttribute(AttributeId.Malnutrition, -(MALNUTRITION_REGENERATION_RATE * Simulation.Singleton.TickTime), 0f, 1000f); // Decrease malnutrition

        if (Malnutrition > 0 && !HasStatusEffect(StatusEffectId.Malnutrition)) AddStatusEffect(new SE_Malnutrition()); // Malnutrition status effect
    }

    private void UpdateActivity()
    {
        if (IsMoving) return; // Just keep moving when already moving

        // If standing on food and not full, eat that
        if(Nutrition.Ratio < 1f)
        {
            TileObjectBase currentFood = GetCurrentFood();
            if (currentFood != null)
            {
                Eat(currentFood);
                CurrentActivity = "Eating " + currentFood.Name;
                return;
            }
        }

        // Actively for food when hungry
        if (Nutrition.Ratio < 0.5f)
        {
            TileObjectBase closestFood = FindClosestFood((int)VisionRange);
            if (closestFood != null)
            {
                MoveTo(closestFood.Tile);
                CurrentActivity = "Moving to eat " + closestFood.Name;
                return;
            }
        }

        // If there are no important tasks to do, have a chance to wander around
        if (Random.value < 0.003f)
        {
            SetMovementPath(Pathfinder.GetRandomPath(this, Tile, 8));
            CurrentActivity = "Wandering around aimlessly";
            return;
        }

        CurrentActivity = "Standing";
        return;
    }
    private void UpdateMovement()
    {
        if (IsMoving)
        {
            WorldTile previousTile = CurrentPath[0]; // We are moving from this tile
            WorldTile nextTile = CurrentPath[1]; // We are moving to this tile

            // Check if the tile we are moving towards is still passable. If not, go back
            if (!nextTile.IsPassable(this))
            {
                if (!previousTile.IsPassable(this)) // If previous tile is also not reachable anymore, find another way out
                {
                    List<WorldTile> stuckEscapePath = Pathfinder.GetRandomPath(this, previousTile, 1);
                    if (stuckEscapePath == null || stuckEscapePath.Count == 1) // We are stuck with no way out
                    {
                        Die();
                        return;
                    }
                    SetMovementPath(stuckEscapePath);
                    return;
                }
                // Go back to previous tile
                SetMovementPath(new List<WorldTile>() { nextTile, previousTile });
                return;
            }

            // Move towards next tile
            transform.position = Vector3.MoveTowards(transform.position, nextTile.WorldPosition, (1f / Tile.GetMovementCost(this)) * Simulation.MOVEMENT_SPEED_MODIFIER * Simulation.Singleton.TickTime);

            // Check on which tile the animal is at this moment exactly. Update tile references accordingly.
            WorldTile currentTile = World.Singleton.GetTile(transform.position);
            if (Tile != currentTile)
            {
                Tile.RemoveObject(this);
                SetTile(currentTile);
                Tile.AddObject(this);
            }

            if (transform.position == nextTile.WorldPosition3) // We arrived at tile center of next tile
            {
                // Arrived at destination OR next tile is not passable => stop moving
                if (CurrentPath.Count == 2 || !CurrentPath[2].IsPassable(this))
                {
                    CurrentPath = null;
                    IsMoving = false;
                }
                else
                {
                    CurrentPath.RemoveAt(0);
                    WorldTile newPreviousTile = CurrentPath[0];
                    WorldTile newNextTile = CurrentPath[1];

                    // Sprite Orientation
                    if (newNextTile.Coordinates.x > newPreviousTile.Coordinates.x) GetComponent<SpriteRenderer>().flipX = false;
                    else if (newNextTile.Coordinates.x < newPreviousTile.Coordinates.x) GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }
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
    protected TileObjectBase FindClosestFood(int maxRange)
    {
        int range = 0;
        List<WorldTile> currentRangeTiles = new List<WorldTile>() { Tile };
        while (range < maxRange)
        {
            foreach (WorldTile tile in currentRangeTiles)
            {
                foreach (TileObjectBase obj in tile.TileObjects)
                    if (obj.GetNutrientsForAnimal(this) > 0) return obj;
            }
            range++;
            currentRangeTiles = Pathfinder.GetAllReachablePositionsWithRange(this, Tile, range);
            if (currentRangeTiles == null) return null; // No tiles reachable with that range (we are in a closed space)
        }
        return null;
    }

    /// <summary>
    /// Returns an edible object the animal is standing on and that can started to be consumed immediately.
    /// </summary>
    protected TileObjectBase GetCurrentFood()
    {
        if (IsMoving) return null;
        foreach (TileObjectBase obj in Tile.TileObjects)
            if (obj.GetNutrientsForAnimal(this) > 0) return obj;
        return null;
    }

    protected void Eat(TileObjectBase obj)
    {
        // Calculate how much % of the object gets consumed this frame
        float chunkEaten = obj.GetEatingSpeed(this) * Simulation.Singleton.TickTime;

        // Reduces objects health by that amount
        float lostHealth = obj.Health.MaxValue * chunkEaten;
        obj.Health.ChangeValue(-lostHealth);

        // Increase animals nutrition by that amount
        float gainedNutrition = obj.NutrientValue * chunkEaten;
        Nutrition.ChangeValue(gainedNutrition);
    }

    public void GiveBirth()
    {
        int numOffspring = Random.Range(MinNumOffspring, MaxNumOffspring + 1);
        for(int i = 0; i < numOffspring; i++) World.Singleton.SpawnTileObject(Tile, Type);
    }

    #endregion

    #region Getters

    public float VisionRange => GetFloatAttribute(AttributeId.VisionRange);
    public float MovementSpeed => GetFloatAttribute(AttributeId.LandMovementSpeed);
    public float WaterMovementSpeed => GetFloatAttribute(AttributeId.WaterMovementSpeed);
    public bool CanSwim => GetFloatAttribute(AttributeId.WaterMovementSpeed) > 0f;

    public List<NutrientType> Diet => ((Att_Diet)Attributes[AttributeId.Diet]).Diet;
    public DynamicRangeAttribute Nutrition => Attributes[AttributeId.Nutrition] as DynamicRangeAttribute;
    public float HungerRate => GetFloatAttribute(AttributeId.HungerRate);
    public float Malnutrition => GetFloatAttribute(AttributeId.Malnutrition);
    public float EatingSpeed => GetFloatAttribute(AttributeId.EatingSpeed);

    public float PregnancyMaxAge => GetFloatAttribute(AttributeId.PregnancyMaxAge);
    public SimulationTime PregnancyDuration => ((StaticAttribute<SimulationTime>)Attributes[AttributeId.PregnancyDuration]).GetStaticValue();
    public SimulationTime PregnancyProgress => ((StaticAttribute<SimulationTime>)Attributes[AttributeId.PregnancyProgress]).GetStaticValue();
    public bool IsPregnant => HasStatusEffect(StatusEffectId.Pregnancy);
    public float PregnancyChance => GetFloatAttribute(AttributeId.PregnancyChance);
    public int MinNumOffspring => (int)Attributes[AttributeId.MinNumOffspring].GetValue();
    public int MaxNumOffspring => (int)Attributes[AttributeId.MaxNumOffspring].GetValue();

    #endregion
}
