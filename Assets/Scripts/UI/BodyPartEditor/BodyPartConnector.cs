using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BodyPartEditor
{
    public class BodyPartConnector : MonoBehaviour
    {
        private const float PreviewScale = 2f; // Preview window is not the same size as image

        public BodyPartConnectorData Data;

        public TextMeshProUGUI Text;

        public void Init(BodyPartConnectorData data)
        {
            Data = data;
            Text.text = data.BodyPartId.ToString();

            Vector2 position = new Vector2(data.x, data.y);
            Vector2 realPosition = position * PreviewScale;
            transform.localPosition = new Vector3(realPosition.x, realPosition.y, 0f);
        }

        public void Update()
        {
            Data.x = transform.localPosition.x / PreviewScale;
            Data.y = transform.localPosition.y / PreviewScale;
        }
    }
}
