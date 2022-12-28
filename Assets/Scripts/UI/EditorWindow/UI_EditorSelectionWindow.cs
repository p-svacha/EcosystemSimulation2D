using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorldEditorSpace
{
    public class UI_EditorSelectionWindow : MonoBehaviour
    {
        [Header("Elements")]
        public WorldEditor WorldEditor;
        public GameObject SurfaceTab;
        public GameObject ObjectsTab;

        [Header("Prefabs")]
        public GameObject SelectionRowPrefab;
        public UI_EditorSelectionElement SelectionElementPrefab;

        private const int ELEMENTS_PER_ROW = 4;
        private int RowElementCounter = 0;
        private GameObject CurrentTab;
        private GameObject CurrentRow;

        private void Start()
        {
            // Surfaces
            GoToTab(SurfaceTab);
            foreach(SurfaceBase surface in World.Singleton.TerrainLayer.Surfaces.Values)
            {
                AddSelectionThing(surface, EditorTool.PlaceSurface, surface.Name, HelperFunctions.Texture2DToSprite(ResourceManager.Singleton.GetSurfaceTexture(surface.SurfaceId)));
            }

            // Object
            GoToTab(ObjectsTab);

            AddSelectionThing(null, EditorTool.RemoveObject, "Remove Object", ResourceManager.Singleton.RemoveSprite);

            foreach (TileObjectBase dummyObject in TileObjectFactory.DummyObjects.Values)
                AddSelectionThing(dummyObject, EditorTool.SpawnObject, dummyObject.Name, dummyObject.DisplaySprite);


            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        private void GoToTab(GameObject tab)
        {
            CurrentTab = tab;
            AddSelectionRow();
        }

        private void AddSelectionRow()
        {
            RowElementCounter = 0;
            CurrentRow = Instantiate(SelectionRowPrefab, CurrentTab.transform);
        }

        private void AddSelectionThing(IThing thing, EditorTool tool, string displayName, Sprite displayImage)
        {
            if (RowElementCounter >= ELEMENTS_PER_ROW) AddSelectionRow();
            UI_EditorSelectionElement selectionElement = Instantiate(SelectionElementPrefab, CurrentRow.transform);
            selectionElement.Init(WorldEditor, tool, thing, displayName, displayImage);
            RowElementCounter++;
        }

        
    }
}
