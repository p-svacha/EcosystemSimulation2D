using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BodyPartEditor
{
    public class BodyPartEditor : MonoBehaviour
    {
        [Header("Elements")]
        public Button AddConnectorButton;
        public Button SaveButton;
        public UI_BodyPartPreview BodyPartPreview;
        public UI_BodyPartSelector Selector_Torso;
        public UI_BodyPartSelector Selector_Neck;
        public UI_BodyPartSelector Selector_Head;
        public UI_BodyPartSelector Selector_LegsHind;
        public UI_BodyPartSelector Selector_LegsFront;
        public UI_BodyPartSelector Selector_Headgear;
        public UI_BodyPartSelector Selector_Tail;

        void Start()
        {
            BodyPartLibrary.Init();

            AddConnectorButton.onClick.AddListener(AddConnectorButton_OnClick);
            SaveButton.onClick.AddListener(SaveButton_OnClick);

            Selector_Torso.Init(BodyPartLibrary.BodyParts[BodyPartId.Torso], BodyPartPreview);
            Selector_Neck.Init(BodyPartLibrary.BodyParts[BodyPartId.Neck], BodyPartPreview);
            Selector_LegsFront.Init(BodyPartLibrary.BodyParts[BodyPartId.LegsFront], BodyPartPreview);
            Selector_LegsHind.Init(BodyPartLibrary.BodyParts[BodyPartId.LegsHind], BodyPartPreview);
            Selector_Tail.Init(BodyPartLibrary.BodyParts[BodyPartId.Tail], BodyPartPreview);
            Selector_Head.Init(BodyPartLibrary.BodyParts[BodyPartId.Head], BodyPartPreview);
            Selector_Headgear.Init(BodyPartLibrary.BodyParts[BodyPartId.Headgear], BodyPartPreview);     
        }

        private void AddConnectorButton_OnClick()
        {
            BodyPartPreview.ShowAddConnectorMenu();
        }

        private void SaveButton_OnClick()
        {
            BodyPartPreview.SaveCurrentBodyPart();
        }
    }
}
