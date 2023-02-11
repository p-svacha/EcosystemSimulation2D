using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BodyPartEditor
{
    public class UI_BodyPartButton : MonoBehaviour
    {
        [Header("Elements")]
        public Image DisplayImage;
        public Button Button;

        public void Init(BodyPartData bodyPart, UI_BodyPartPreview preview)
        {
            DisplayImage.sprite = bodyPart.Sprite;
            Button.onClick.AddListener(() => preview.PreviewBodyPart(bodyPart));
        }
    }
}
