using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SWC_TileObjectBase : UI_SelectionWindowContent
{
    [Header("Elements")]
    public UI_ValueBar HealthBar;

    private VisibleTileObject TileObject;

    public override void Init(IThing thing)
    {
        TileObject = (VisibleTileObject)thing;
        HealthBar.Init("Health");
    }

    protected virtual void Update()
    {
        HealthBar.SetValue(TileObject.Health, TileObject.MaxHealth);
    }

    public override bool CanFocusCamera() => true;
    public override void FocusCamera() => CameraHandler.Singleton.FocusPosition(TileObject.transform.position);


    public override bool CanRemoveObject() => true;
    public override void RemoveObject()
    {
        World.Singleton.RemoveObject(TileObject);
    }

    public override bool CanSelectNextLayer() => true;
    public override void SelectNextLayer() => TileObject.Tile.SelectNextLayer(TileObject);

}
