using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                AddConnectorPreview(con);
        }

        public void ShowAddConnectorMenu()
        {
            // Identify for which body parts a connector can be added
            List<BodyPartId> possibleConnectors = new List<BodyPartId>();
            foreach (BodyPartId id in System.Enum.GetValues(typeof(BodyPartId)))
            {
                if (BodyPartPreview.BodyPartId == id) continue;
                if (BodyPartPreview.Connectors.Any(x => x.BodyPartId == id)) continue;
                possibleConnectors.Add(id);
            }
            if (possibleConnectors.Count == 0) return;

            // Display context menu with all options
            List<ContextMenuEntry> entries = new List<ContextMenuEntry>();
            foreach(BodyPartId id in possibleConnectors)
            {
                ContextMenuEntry entry = new ContextMenuEntry(id.ToString(), () => AddConnector(id));
                entries.Add(entry);
            }
            UI_ContextMenu.Singleton.Show(entries);
        }

        private void AddConnector(BodyPartId id)
        {
            BodyPartConnectorData newCon = new BodyPartConnectorData()
            {
                BodyPartId = id,
                x = Random.Range(0, BodyPartLibrary.BODY_PART_SPRITE_SIZE),
                y = Random.Range(0, BodyPartLibrary.BODY_PART_SPRITE_SIZE)
            };
            BodyPartPreview.Connectors.Add(newCon);
            AddConnectorPreview(newCon);
        }

        public void RemoveConnector(BodyPartConnectorData con)
        {
            BodyPartPreview.Connectors.Remove(con);
            GameObject.Destroy(ConnectorPreviews.First(x => x.ConnectorData == con).gameObject);
        }

        public void AddConnectorPreview(BodyPartConnectorData con)
        {
            BodyPartConnector connectorPreview = Instantiate(ConnectorPrefab, PreviewContainer.transform);
            connectorPreview.gameObject.name = con.BodyPartId + " connector preview";
            connectorPreview.Init(BodyPartPreview, con, this);
            ConnectorPreviews.Add(connectorPreview);
        }

        public void SaveCurrentBodyPart()
        {
            BodyPartLibrary.SaveBodyPartData(BodyPartPreview);
        }
    }
}
