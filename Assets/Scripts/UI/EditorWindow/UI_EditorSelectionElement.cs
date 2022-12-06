using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WorldEditorSpace
{
    public class UI_EditorSelectionElement : MonoBehaviour
    {
        [Header("Elements")]
        public Image DisplayImage;
        public GameObject SelectionButttonContainer;
        public Button SelectionButton;
        public TextMeshProUGUI Text;
        public Button InfoButton;
        public Image SelectionFrame;

        public WorldEditor Editor { get; private set; }
        /// <summary>
        /// EditorTool that is active when this element is selected.
        /// </summary>
        public EditorTool EditorTool { get; private set; }
        /// <summary>
        /// EditorToolThing that is active when this element is selected.
        /// </summary>
        public IThing EditorToolThing { get; private set; }

        public void Init(WorldEditor editor, EditorTool tool, IThing thing, string displayName, Sprite displayImage)
        {
            Editor = editor;
            EditorTool = tool;
            EditorToolThing = thing;

            DisplayImage.sprite = displayImage;
            Text.text = displayName;
            InfoButton.onClick.AddListener(InfoButton_OnClick);
            if (thing == null) SelectionButttonContainer.gameObject.SetActive(false);
            SelectionButton.onClick.AddListener(SelectionButton_OnClick);
            SelectionFrame.gameObject.SetActive(false);
        }

        public void SetSelected(bool value)
        {
            SelectionFrame.gameObject.SetActive(value);
        }

        private void SelectionButton_OnClick()
        {
            Editor.SetEditorTool(EditorTool, EditorToolThing, this);
        }
        private void InfoButton_OnClick()
        {
            UIHandler.Singleton.AddThingInfoWindow(EditorToolThing);
        }
    }
}
