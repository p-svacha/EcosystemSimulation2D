using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// A SelectionWindow displays the information about an IThing.
/// <br/> It consist of a header that is the same for all things and a dynamic container that can differ from Thing to Thing.
/// <br/> The dynamic part is part of the IThing interface.
/// </summary>
public class UI_SelectionWindow : MonoBehaviour, IPointerClickHandler
{
    [Header("Elements")]
    public TextMeshProUGUI TitleText;
    public Button InfoButton;
    public Button FocusCameraButton;
    public Button RemoveObjectButton;
    public Button SelectNextLayerButton;
    public Button CloseButton;
    public GameObject DynamicContentContainer;

    private IThing Thing;
    public UI_SelectionWindowContent SelectionWindowContent;

    public void Init(IThing thing)
    {
        Thing = thing;
        TitleText.text = thing.Name;

        // Load dynamic content from thing
        if (thing.SelectionWindowContent != null)
        {
            SelectionWindowContent = Instantiate(thing.SelectionWindowContent, DynamicContentContainer.transform);
            SelectionWindowContent.Init(thing);
            SelectionWindowContent.ParentWindow = this;
        }

        // Static Buttons
        InfoButton.onClick.AddListener(InfoButton_OnClick);
        CloseButton.onClick.AddListener(CloseButton_OnClick);

        // Dynamic Button
        if (SelectionWindowContent == null || !SelectionWindowContent.CanFocusCamera()) FocusCameraButton.gameObject.SetActive(false);
        else FocusCameraButton.onClick.AddListener(SelectionWindowContent.FocusCamera);

        if (SelectionWindowContent == null || !SelectionWindowContent.CanRemoveObject()) RemoveObjectButton.gameObject.SetActive(false);
        else RemoveObjectButton.onClick.AddListener(SelectionWindowContent.RemoveObject);

        if (SelectionWindowContent == null || !SelectionWindowContent.CanSelectNextLayer()) SelectNextLayerButton.gameObject.SetActive(false);
        else SelectNextLayerButton.onClick.AddListener(SelectionWindowContent.SelectNextLayer);

        ForceUpdateLayout();
    }


    private void ForceUpdateLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(DynamicContentContainer.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private void InfoButton_OnClick()
    {
        UIHandler.Singleton.AddThingInfoWindow(Thing);
    }
    private void CloseButton_OnClick()
    {
        UIHandler.Singleton.CloseSelectionWindow(Thing);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }
}
