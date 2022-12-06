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

    public float LastFrameHoursPassed { get; private set; }
    public const int SPEED0_MODIFIER = 0; // Pause
    public const int SPEED1_MODIFIER = 1;
    public const int SPEED2_MODIFIER = 2;
    public const int SPEED3_MODIFIER = 4;

    public bool IsPaused => SpeedModifier == SPEED0_MODIFIER;

    /// <summary>
    /// Every individual WorldTile only gets updated every n'th frame according to this value for performance reasons.
    /// <br/> During world generation tiles get assigned into one of n pots and each frame one pot of tiles gets updated.
    /// <br/> Objects are not affected.
    /// </summary>
    public const int TILE_UPDATE_POTS = 16;

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
        SetSpeed(SPEED1_MODIFIER);
    }

    #endregion

    #region Flow / Update

    private void Update()
    {
        UpdateTime();
        UpdateHoveredObjects();
    }

    private void UpdateTime()
    {
        LastFrameHoursPassed = (Time.deltaTime / RealSecondsPerHour) * SpeedModifier;
        CurrentTime.IncreaseTime(LastFrameHoursPassed);
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
