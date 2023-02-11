using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BodyPartEditor
{
    public class UI_BodyPartSelector : MonoBehaviour
    {
        [Header("Prefabs")]
        public UI_BodyPartButton BodyPartButtonPrefab;

        [Header("Elements")]
        public GameObject SelectionContainer;

        public void Init(List<BodyPartData> data, UI_BodyPartPreview bodyPartPreview)
        {
            HelperFunctions.DestroyAllChildredImmediately(SelectionContainer);

            foreach (BodyPartData bodyPart in data)
            {
                UI_BodyPartButton btn = Instantiate(BodyPartButtonPrefab, SelectionContainer.transform);
                btn.Init(bodyPart, bodyPartPreview);
            }
        }
    }
}

