using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BodyPartEditor
{
    public class BodyPartEditor : MonoBehaviour
    {
        [Header("Elements")]
        public Button SaveButton;
        public UI_BodyPartPreview BodyPartPreview;
        public UI_BodyPartSelector Selector_Torso;

        void Start()
        {
            BodyPartLibrary.Init();

            Selector_Torso.Init(BodyPartLibrary.BodyParts[BodyPartId.Torso], BodyPartPreview);

            SaveButton.onClick.AddListener(SaveBodyPart);
        }

        private void SaveBodyPart()
        {
            BodyPartPreview.SaveCurrentBodyPart();
        }
    }
}
