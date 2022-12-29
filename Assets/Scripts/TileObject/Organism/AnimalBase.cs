using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public abstract class AnimalBase : OrganismBase
{
    // IThing
    public override UI_SelectionWindowContent SelectionWindowContent => ResourceManager.Singleton.SWC_AnimalBase;

    // Required Attributes
    protected abstract float LAND_MOVEMENT_SPEED_BASE { get; }
    protected abstract float WATER_MOVEMENT_SPEED_BASE { get; }
    protected abstract float NUTRITION_BASE { get; }
    protected abstract float HUNGER_RATE_BASE { get; }
    protected abstract List<NutrientType> DIET { get; }
    protected abstract float VISION_RANGE { get; }
    protected abstract SimulationTime PREGNANCY_MAX_AGE { get; }
    protected abstract float PREGNANCY_CHANCE_BASE { get; }
    protected abstract SimulationTime PREGNANCY_DURATION { get; }
    protected abstract int NUM_OFFSPRING_MIN { get; }
    protected abstract int NUM_OFFSPRING_MAX { get; }

    protected abstract int SPAWN_GROUP_SIZE_MIN { get; }
    protected abstract int SPAWN_GROUP_SIZE_MAX { get; }

    // Optional Attributes
    protected virtual float EATING_SPEED => 1f;

    // Behaviour Logic
    protected List<WorldTile> CurrentPath;
    public bool IsMoving { get; private set; }
    private List<Activity> PossibleActivites = new List<Activity>();
    public Activity CurrentActivity { get; private set; }

    #region Initialize

    public override void Init()
    {
        base.Init();

        if (LAND_MOVEMENT_SPEED_BASE >= 1f) Debug.LogWarning("Movement Speed of an animal is not allowed to be greater than 1 because it breaks Pathfinding.");

        // Attributes
        // General
        _Attributes.Add(AttributeId.VisionRange, new StaticAttribute<float>(AttributeId.VisionRange, "Vision Range", "General", VISION_RANGE));

        // Movement
        _Attributes.Add(AttributeId.LandMovementSpeedBase, new StaticAttribute<float>(AttributeId.LandMovementSpeedBase, "Base Land Movement Speed", "Movement", LAND_MOVEMENT_SPEED_BASE));
        _Attributes.Add(AttributeId.WaterMovementSpeedBase, new StaticAttribute<float>(AttributeId.WaterMovementSpeed, "Base Water Movement Speed", "Movement", WATER_MOVEMENT_SPEED_BASE));
        _Attributes.Add(AttributeId.Movement, new Att_Movement(this));
        _Attributes.Add(AttributeId.LandMovementSpeed, new Att_LandMovementSpeed(this));
        _Attributes.Add(AttributeId.WaterMovementSpeed, new Att_WaterMovementSpeed(this));

        // Needs
        _Attributes.Add(AttributeId.Diet, new Att_Diet(DIET));
        _Attributes.Add(AttributeId.NutritionBase, new StaticAttribute<float>(AttributeId.NutritionBase, "Base Nutrition", "Needs", NUTRITION_BASE));
        _Attributes.Add(AttributeId.Nutrition, new Att_Nutrition(this));
        _Attributes.Add(AttributeId.HungerRateBase, new StaticAttribute<float>(AttributeId.HungerRateBase, "Base Hunger Rate", "Needs", HUNGER_RATE_BASE));
        _Attributes.Add(AttributeId.HungerRate, new Att_HungerRate(this));
        _Attributes.Add(AttributeId.EatingSpeed, new StaticAttribute<float>(AttributeId.EatingSpeed, "Eating Speed", "Needs", EATING_SPEED));

        // Reproduction
        _Attributes.Add(AttributeId.PregnancyMaxAge, new TimeAttribute(AttributeId.PregnancyMaxAge, "Maximum Age for Pregnancy", "Reproduction", PREGNANCY_MAX_AGE));
        _Attributes.Add(AttributeId.PregnancyDuration, new TimeAttribute(AttributeId.PregnancyDuration, "Pregnancy Duration", "Reproduction", PREGNANCY_DURATION));
        _Attributes.Add(AttributeId.PregnancyChanceBase, new StaticAttribute<float>(AttributeId.PregnancyChanceBase, "Base Pregnancy Chance", "Reproduction", PREGNANCY_CHANCE_BASE));
        _Attributes.Add(AttributeId.PregnancyChance, new Att_PregnancyChance(this));
        _Attributes.Add(AttributeId.NumOffspring, new StaticRangeAttribute(AttributeId.NumOffspring, "Amount of offspring", "Reproduction", "Amount of children an animal will produce when giving birth.", NUM_OFFSPRING_MIN, NUM_OFFSPRING_MAX));

        // Spawn
        _Attributes.Add(AttributeId.SpawnGroupSize, new StaticRangeAttribute(AttributeId.SpawnGroupSize, "Spawn Group Size", "Spawn", "Amount that gets spawned when an objects is created.", SPAWN_GROUP_SIZE_MIN, SPAWN_GROUP_SIZE_MAX));

        // Status Displays
        ConditionalStatusDisplays.Add(new SD_LowHealth(this));

        // Activities
        PossibleActivites.Add(new Act_WanderAround(this));
        PossibleActivites.Add(new Act_Stand(this));
        PossibleActivites.Add(new Act_Eat(this));
    }

    public override void InitNew()
    {
        base.InitNew();
        Nutrition.Init(initialRatio: 1f);
    }

    public override void InitExisting()
    {
        base.InitExisting();
        Nutrition.Init(initialRatio: Random.value);
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
        if (Nutrition.Value <= 0 && !HasStatusEffect(StatusEffectId.Malnutrition)) AddStatusEffect(new SE_Malnutrition()); // Malnutrition status effect
    }

    private void UpdateActivity()
    {
        // Find and set new activity when previous one is done
        if (CurrentActivity == null || !CurrentActivity.IsActive)
        {
            float highestUrgency = float.MinValue;
            Activity highestUrgencyActivity = null;
            foreach (Activity act in PossibleActivites)
            {
                float urgency = act.GetUrgency();
                if (urgency > highestUrgency)
                {
                    highestUrgency = urgency;
                    highestUrgencyActivity = act;
                }
            }

            CurrentActivity = highestUrgencyActivity;
            highestUrgencyActivity.Start();
        }

        else CurrentActivity.OnTick();
    }

    /// <summary>
    /// Forces the animal to instantly end the current activity and look for a new one.
    /// </summary>
    protected void ForceEndCurrentActivity()
    {
        CurrentActivity.End();
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

    public void MoveTo(WorldTile target)
    {
        List<WorldTile> pathToTarget = Pathfinder.GetPath(this, Tile, target);
        SetMovementPath(pathToTarget);
    }

    public void SetMovementPath(List<WorldTile> path)
    {
        if (path == null || path.Count <= 1) return; // no path found

        CurrentPath = path;
        IsMoving = true;

        if (CurrentPath[1].Coordinates.x > CurrentPath[0].Coordinates.x) GetComponent<SpriteRenderer>().flipX = false;
        else if (CurrentPath[1].Coordinates.x < CurrentPath[0].Coordinates.x) GetComponent<SpriteRenderer>().flipX = true;
    }

    public void GiveBirth()
    {
        int numOffspring = NumOffspring.RandomValue;
        for(int i = 0; i < numOffspring; i++) World.Singleton.SpawnTileObject(Tile, ObjectId);
    }

    /// <summary>
    /// Takes a tile outside the vision range of the animal and walks there on the fastest path. Used when needing something but not seeing it.
    /// </summary>
    public void Search()
    {
        int numFailedAttempts = 0;
        // Take a random coordinate twice the vision range
        while (!IsMoving && numFailedAttempts < 5)
        {
            List<WorldTile> targetPath = Pathfinder.GetRandomDirectedPath(this, Tile, (int)VisionRange, (int)(VisionRange * 2));
            SetMovementPath(targetPath);
            numFailedAttempts++;
        }
    }

    #endregion

    #region Getters

    /// <summary><include file='Assets/Resources/Strings/GlobalStrings.xml' path='GlobalStrings/AttributeDescriptions/VisionRange'/></summary>
    public float VisionRange => GetFloatAttribute(AttributeId.VisionRange);
    public float Movement => GetFloatAttribute(AttributeId.Movement);
    public float LandMovementSpeed => GetFloatAttribute(AttributeId.LandMovementSpeed);
    public float WaterMovementSpeed => GetFloatAttribute(AttributeId.WaterMovementSpeed);
    public bool CanSwim => GetFloatAttribute(AttributeId.WaterMovementSpeed) > 0f;

    public List<NutrientType> Diet => ((Att_Diet)Attributes[AttributeId.Diet]).Diet;
    public DynamicBarAttribute Nutrition => Attributes[AttributeId.Nutrition] as DynamicBarAttribute;
    public float HungerRate => GetFloatAttribute(AttributeId.HungerRate);
    public float EatingSpeed => GetFloatAttribute(AttributeId.EatingSpeed);

    public float PregnancyMaxAge => GetFloatAttribute(AttributeId.PregnancyMaxAge);
    public SimulationTime PregnancyDuration => (Attributes[AttributeId.PregnancyDuration] as TimeAttribute).GetStaticValue();
    public bool IsPregnant => HasStatusEffect(StatusEffectId.Pregnancy);
    public float PregnancyChance => GetFloatAttribute(AttributeId.PregnancyChance);
    public StaticRangeAttribute NumOffspring => Attributes[AttributeId.NumOffspring] as StaticRangeAttribute;

    public StaticRangeAttribute SpawnGroupSize => Attributes[AttributeId.SpawnGroupSize] as StaticRangeAttribute;

    #endregion
}
