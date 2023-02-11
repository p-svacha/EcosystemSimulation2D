using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BodyPartEditor
{
    public class UI_BodyPartPreview : MonoBehaviour
    {
        private BodyPartData BodyPartPreview;
        private List<BodyPartConnector> ConnectorPreviews = new List<BodyPartConnector>();

        [Header("Prefabs")]
        public BodyPartConnector ConnectorPrefab;

        [Header("Elements")]
        public GameObject PreviewContainer;

        public void PreviewBodyPart(BodyPartData bodyPart)
        {
            HelperFunctions.DestroyAllChildredImmediately(PreviewContainer);

            BodyPartPreview = bodyPart;

            // Create preview of body part itself
            GameObject bodyPartObject = new GameObject("bodyPart");
            bodyPartObject.transform.SetParent(PreviewContainer.transform);
            Image bodyPartImage = bodyPartObject.AddComponent<Image>();
            bodyPartImage.sprite = bodyPart.Sprite;

            RectTransform rt = bodyPartObject.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 0f);
            rt.anchorMax = new Vector2(1f, 1f);
            bodyPartObject.transform.localScale = new Vector3(1f, 1f, 1f);
            HelperFunctions.SetRectTransformMargins(rt, 0, 0, 0, 0);

            // Create preview of connectors
            ConnectorPreviews.Clear();
            foreach (BodyPartConnectorData con in bodyPart.Connectors)
            {
                BodyPartConnector connectorPreview = Instantiate(ConnectorPrefab, PreviewContainer.transform);
                connectorPreview.Init(con);
                ConnectorPreviews.Add(connectorPreview);
            }
        }

        public void SaveCurrentBodyPart()
        {
            BodyPartData newData = new BodyPartData();
            newData.Name = BodyPartPreview.Name;
            newData.Path = BodyPartPreview.Path;
            newData.BodyPartId = BodyPartPreview.BodyPartId;
            newData.Connectors = new List<BodyPartConnectorData>();
            foreach(var con in ConnectorPreviews)
            {
                newData.Connectors.Add(new BodyPartConnectorData()
                {
                    BodyPartId = con.Data.BodyPartId,
                    x = con.Data.x,
                    y = con.Data.y
                });
            }

            BodyPartLibrary.SaveBodyPartData(newData);
        }
    }
}
