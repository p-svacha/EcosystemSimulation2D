using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SWC_TileObjectBase : UI_SelectionWindowContent
{
    private TileObject TileObject;

    public override void Init(IThing thing)
    {
        TileObject = (TileObject)thing;
    }

    public override bool CanFocusCamera() => true;
    public override void FocusCamera() => CameraHandler.Singleton.FocusPosition(TileObject.transform.position);


    public override bool CanRemoveObject() => true;
    public override void RemoveObject()
    {
        World.Singleton.RemoveObject(TileObject);
        UIHandler.Singleton.CloseSelectionWindow(TileObject);
    }

    public override bool CanSelectNextLayer() => true;
    public override void SelectNextLayer() => TileObject.Tile.SelectNextLayer(TileObject);

}
