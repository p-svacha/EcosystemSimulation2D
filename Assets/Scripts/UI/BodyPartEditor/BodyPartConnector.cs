using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BodyPartEditor
{
    public class BodyPartConnector : MonoBehaviour
    {
        private UI_BodyPartPreview PreviewWindow;
        private float PreviewWindowSize;
        private float PreviewWindowScale;

        public BodyPartData SourceBodyPartData;
        public BodyPartConnectorData ConnectorData;

        private GameObject PreviewBodyPart;
        private int CurrentPreviewIndex;

        [Header("Elements")]
        public TextMeshProUGUI Text;
        public Button PrevButton;
        public Button NextButton;
        public Button RemoveButton;

        public void Init(BodyPartData source, BodyPartConnectorData data, UI_BodyPartPreview previewWindow)
        {
            SourceBodyPartData = source;
            ConnectorData = data;
            PreviewWindow = previewWindow;
            Text.text = data.BodyPartId.ToString();

            PreviewWindowSize = previewWindow.GetComponent<RectTransform>().sizeDelta.x;
            PreviewWindowScale = PreviewWindowSize / BodyPartLibrary.BODY_PART_SPRITE_SIZE;
            Debug.Log("Preview size: " + PreviewWindowSize + ", Preview scale: " + PreviewWindowScale);

            Vector2 position = new Vector2(data.x, data.y);
            Vector2 realPosition = position * PreviewWindowScale;
            transform.localPosition = new Vector3(realPosition.x, realPosition.y, 0f);

            PrevButton.onClick.AddListener(SetPreviousPreview);
            NextButton.onClick.AddListener(SetNextPreview);
            RemoveButton.onClick.AddListener(RemoveButton_OnClick);

            SetPreview(-1);     
        }

        public void Update()
        {
            ConnectorData.x = transform.localPosition.x / PreviewWindowScale;
            ConnectorData.y = transform.localPosition.y / PreviewWindowScale;
        }

        private void RemoveButton_OnClick()
        {
            PreviewWindow.RemoveConnector(ConnectorData);
        }

        private void SetPreviousPreview()
        {
            if (CurrentPreviewIndex == -1) SetPreview(BodyPartLibrary.BodyParts[ConnectorData.BodyPartId].Count - 1);
            else SetPreview(CurrentPreviewIndex - 1);
        }

        private void SetNextPreview()
        {
            if (CurrentPreviewIndex == BodyPartLibrary.BodyParts[ConnectorData.BodyPartId].Count - 1) SetPreview(-1);
            else SetPreview(CurrentPreviewIndex + 1);
        }

        /// <summary>
        /// Set a specific body part to preview at this connector by index. -1 will remove the preview.
        /// </summary>
        private void SetPreview(int index)
        {
            CurrentPreviewIndex = index;

            if (PreviewBodyPart != null) GameObject.Destroy(PreviewBodyPart);

            if (index == -1) return;

            BodyPartData bpToPreview = BodyPartLibrary.BodyParts[ConnectorData.BodyPartId][index];
            GameObject previewObject = new GameObject(ConnectorData.BodyPartId + " connection preview");
            previewObject.transform.SetParent(transform);
            previewObject.transform.SetAsFirstSibling();
            Image previewImage = previewObject.AddComponent<Image>();
            previewImage.sprite = bpToPreview.Sprite;
            previewImage.raycastTarget = false;

            previewObject.GetComponent<RectTransform>().sizeDelta = new Vector2(PreviewWindowSize, PreviewWindowSize);
            previewObject.transform.localScale = Vector3.one;

            BodyPartConnectorData conData = bpToPreview.GetConnectorTo(SourceBodyPartData.BodyPartId);
            Vector2 connectionPosition = new Vector2(conData.x, conData.y);
            Vector2 offsetPosition = new Vector2(BodyPartLibrary.BODY_PART_SPRITE_SIZE / 2, BodyPartLibrary.BODY_PART_SPRITE_SIZE / 2) - connectionPosition;
            Debug.Log("Connector position: " + connectionPosition + ", Offset position: " + offsetPosition);
            previewObject.transform.localPosition = offsetPosition * PreviewWindowScale;


            PreviewBodyPart = previewObject;
        }
    }
}
