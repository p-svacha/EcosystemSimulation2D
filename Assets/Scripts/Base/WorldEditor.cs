using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WorldEditorSpace
{
    public class WorldEditor : MonoBehaviour
    {
        public Simulation Simulation;
        public World World;

        public EditorTool EditorTool; // what tool is selected (i.e. place surface)
        public IThing EditorToolThing; // what type is selected for current tool (i.e. place WATER surface)

        private UI_EditorSelectionElement SelectedToolElement;

        private WorldTile LastFrameTile;
        private bool IsLeftMouseDown;

        private void Start()
        {
            EditorTool = EditorTool.None;
        }

        private void Update()
        {
            bool isUiClick = EventSystem.current.IsPointerOverGameObject();

            if (Input.GetKeyDown(KeyCode.Mouse0)) IsLeftMouseDown = true;
            if (Input.GetKeyUp(KeyCode.Mouse0)) IsLeftMouseDown = false;

            WorldTile tile = Simulation.HoveredWorldTile;
            bool tileChanged = tile != LastFrameTile;
            LastFrameTile = tile;

            

            if (!isUiClick && (Input.GetKeyDown(KeyCode.Mouse0) || (IsLeftMouseDown && tileChanged))) // Click or Drag (drag only applies when going to new tile)
            {
                switch (EditorTool)
                {
                    case EditorTool.PlaceSurface:
                        Surface surface = (Surface)EditorToolThing;
                        if (surface != null && tile != null) World.SetTerrain(tile, surface);
                        break;

                    case EditorTool.SpawnObject:
                        VisibleTileObject tileObject = (VisibleTileObject)EditorToolThing;
                        if (tileObject != null && tile != null) World.SpawnTileObject(tile, tileObject.Type);
                        break;

                    case EditorTool.RemoveObject:
                        if (tile != null) World.RemoveObjects(tile);
                        break;
                }
            }

            if(!isUiClick && EditorTool == EditorTool.None && Input.GetKeyDown(KeyCode.Mouse0) && Simulation.HoveredThing != null) // Select things
                UIHandler.Singleton.AddSelectionWindow(Simulation.HoveredThing);
        }

        public void SetEditorTool(EditorTool tool, IThing thing, UI_EditorSelectionElement selectionElement)
        {
            if(EditorTool == tool && EditorToolThing == thing)
            {
                UnselectEditorTool();
                return;
            }

            EditorTool = tool;
            EditorToolThing = thing;

            // Update UI
            if (SelectedToolElement != null) SelectedToolElement.SetSelected(false);
            SelectedToolElement = selectionElement;
            if (SelectedToolElement != null) SelectedToolElement.SetSelected(true);
        }

        private void UnselectEditorTool()
        {
            EditorTool = EditorTool.None;
            EditorToolThing = null;
            if (SelectedToolElement != null) SelectedToolElement.SetSelected(false);
            SelectedToolElement = null;
        }
    }

    public enum EditorTool
    {
        None,
        PlaceSurface,
        SpawnObject,
        RemoveObject
    }
}
