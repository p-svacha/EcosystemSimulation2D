using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public World World;

    // Time
    public SimulationTime CurrentTime;
    private const float RealSecondsPerHour = 4f; // Time at 1x speed
    private float SpeedModifier = 1f;

    /// <summary>
    /// The exact amount of hours in the SimulationTime that have passed in this tick.
    /// </summary>
    public float TickHours { get; private set; }
    public const int SPEED0_MODIFIER = 0; // Pause
    public const int SPEED1_MODIFIER = 1;
    public const int SPEED2_MODIFIER = 2;
    public const int SPEED3_MODIFIER = 4;

    public bool IsPaused => SpeedModifier == SPEED0_MODIFIER;

    // Update List
    private List<TileObject> SimulatedObjects = new List<TileObject>(); // if performance needs to improve further, change this to an array
    private List<TileObject> UnregisteredObjects = new List<TileObject>();
    public int NumObjects => SimulatedObjects.Count;

    // Simulation Constants
    public const float MOVEMENT_SPEED_MODIFIER = 30f;

    /// <summary>
    /// Every individual WorldTile only gets updated every n'th frame according to this value for performance reasons.
    /// <br/> During world generation tiles get assigned into one of n pots and each frame one pot of tiles gets updated.
    /// <br/> Objects are not affected.
    /// </summary>
    public const int NUM_TILE_UPDATE_POTS = 16;
    private int CurrentTilePot = 0;
    private Dictionary<int, SimulationTime> LastPotUpdateTimes = new Dictionary<int, SimulationTime>();

    /// <summary>
    /// The Thing the mouse is currently hovering over. This does not include WorldTiles.
    /// </summary>
    public IThing HoveredThing { get; private set; }
    /// <summary>
    /// The WorldTile the mouse is currently hovering over.
    /// </summary>
    public WorldTile HoveredWorldTile { get; private set; }

    #region Initialization

    void Start()
    {
        _Singleton = GameObject.Find("Simulation").GetComponent<Simulation>();

        WorldGenerationInfo genInfo = new WorldGenerationInfo();
        World.DrawTiles(WorldGenerator.GenerateBattleMap(World, genInfo));

        CameraHandler.Singleton.SetBounds(World.MinWorldX, World.MinWorldY, World.MaxWorldX, World.MaxWorldY);
        CameraHandler.Singleton.FocusPosition(new Vector2(World.CenterWorldX, World.CenterWorldY));

        CurrentTime = new SimulationTime(year: 1, month: 1, day: 1, hour: 0);
        for (int i = 0; i < NUM_TILE_UPDATE_POTS; i++) LastPotUpdateTimes.Add(i, CurrentTime.Copy());
        SetSpeed(SPEED1_MODIFIER);
    }

    #endregion

    #region Flow / Update

    private void Update()
    {
        UpdateTime();
        UpdateHoveredObjects();

        UpdateSimulation();
    }

    private void UpdateTime()
    {
        TickHours = (Time.deltaTime / RealSecondsPerHour) * SpeedModifier;
        CurrentTime.IncreaseTime(TickHours);
        UI_TimeControls.Singleton.SetTimeDisplay(CurrentTime);
    }

    private void UpdateHoveredObjects()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Update currently hovered object
        HoveredThing = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, 1500f);

        foreach (RaycastHit2D hit in hits)
        {
            IThing thing = hit.collider.GetComponent(typeof(IThing)) as IThing;
            if (thing != null) HoveredThing = thing;
            break; // remove this line if you want to select all currently hovered things
        }

        // Update currently hovered Battle Map Tile
        HoveredWorldTile = World.GetTile(mouseWorldPosition);
    }

    /// <summary>
    /// This is the master update method that manages all other update calls. All tiles, surfaces and object updates are called from this class and nowhere else.
    /// <br/> Centralized update is important for good optimization
    /// </summary>
    private void UpdateSimulation()
    {
        if (IsPaused) return;

        // Surfaces
        UpdateSurfaces();

        // Objects
        foreach (TileObject obj in SimulatedObjects) obj.Tick();

        // Remove unregistered objects
        foreach(TileObject obj in UnregisteredObjects) SimulatedObjects.Remove(obj);
        UnregisteredObjects.Clear();
    }

    /// <summary>
    /// Updates surfaces for this tick. For performance reasons not all tiles are updated each tick.
    /// <br/> There are NUM_TILE_UPDATE_POTS pots with tiles and only one pot gets updated each tick.
    /// </summary>
    private void UpdateSurfaces()
    {
        float hoursSinceLastUpdate = CurrentTime.ExactTime - LastPotUpdateTimes[CurrentTilePot].ExactTime;
        LastPotUpdateTimes[CurrentTilePot] = CurrentTime.Copy();

        foreach (WorldTile tile in World.TileUpdatePots[CurrentTilePot++]) tile.Tick(hoursSinceLastUpdate);
        if (CurrentTilePot >= NUM_TILE_UPDATE_POTS) CurrentTilePot = 0;
    }

    #endregion

    #region Object Registration

    /// <summary>
    /// Registers an object to be part of the simulation.
    /// </summary>
    public void RegisterObject(TileObject obj)
    {
        SimulatedObjects.Add(obj);
    }

    /// <summary>
    /// Unregisters an object from the simulation.
    /// </summary>
    public void UnregisterObject(TileObject obj)
    {
        UnregisteredObjects.Add(obj);
    }

    #endregion

    #region Time Controls

    public void SetSpeed(int value)
    {
        SpeedModifier = value;
        UI_TimeControls.Singleton.SetSpeedDisplay(value);
    }

    #endregion

    private static Simulation _Singleton;
    public static Simulation Singleton => _Singleton;
}
