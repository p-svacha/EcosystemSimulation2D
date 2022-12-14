using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;
using static Attribute;

public class UI_ThingInfoWindow : MonoBehaviour, IPointerClickHandler
{
    [Header("Elements")]
    public Button CloseBtn;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI DescriptionText;
    public GameObject AttributeListContainer;
    public TextMeshProUGUI AttributeBreakdownText;

    [Header("Prefabs")]
    public GameObject AttributeCategoryPrefab;
    public UI_ThingInfoAttributeDisplay AttributePrefab;

    private IThing Thing;

    // Update / Refresh
    private List<UI_ThingInfoAttributeDisplay> AttributeDisplays = new List<UI_ThingInfoAttributeDisplay>(); 
    private Attribute SelectedAttribute;

    private void Start()
    {
        CloseBtn.onClick.AddListener(() => UIHandler.Singleton.CloseThingInfoWindow(Thing));
    }

    public void Init(IThing thing)
    {
        Thing = thing;
        TitleText.text = thing.Name;
        DescriptionText.text = thing.Description;

        // Reset container
        HelperFunctions.DestroyAllChildredImmediately(AttributeListContainer);

        // Create temporary attributes for ID, Name and Description to display in as rows.
        Attribute tempIdAtt = new StaticAttribute<string>(AttributeId.Id, "Thing ID", "Base", thing.ThingId.ToString());
        Attribute tempNameAtt = new StaticAttribute<string>(AttributeId.Name, "Name", "Base", thing.Name);
        Attribute tempDescAtt = new StaticAttribute<string>(AttributeId.Description, "Description", "Base", thing.Description);

        // Collect all attributes to display
        List<Attribute> attributes = new List<Attribute>() { tempIdAtt, tempNameAtt, tempDescAtt };
        foreach (Attribute att in thing.Attributes.Values) attributes.Add(att);

        // Group attributes by category
        Dictionary<string, List<Attribute>> groupedAttributes = new Dictionary<string, List<Attribute>>();
        foreach(Attribute att in attributes)
        {
            if (!groupedAttributes.ContainsKey(att.Category)) groupedAttributes.Add(att.Category, new List<Attribute>() { att });
            else groupedAttributes[att.Category].Add(att);
        }

        // Display all attributes
        foreach (KeyValuePair<string, List<Attribute>> attGroup in groupedAttributes)
        {
            GameObject catDisplay = Instantiate(AttributeCategoryPrefab, AttributeListContainer.transform);
            catDisplay.GetComponentInChildren<TextMeshProUGUI>().text = attGroup.Key;
            foreach (Attribute att in attGroup.Value)
            {
                UI_ThingInfoAttributeDisplay attDisplay = Instantiate(AttributePrefab, AttributeListContainer.transform);
                attDisplay.Init(att, this);
                AttributeDisplays.Add(attDisplay);
            }
        }

        // Init layout
        DisplayAttributeBreakdown();
        UpdateLayout();
    }

    private void UpdateAttributeDisplays()
    {
        foreach (UI_ThingInfoAttributeDisplay attDisplay in AttributeDisplays) attDisplay.UpdateValueDisplay();
    }

    void Update()
    {
        UpdateAttributeDisplays();
        DisplayAttributeBreakdown();
    }

    public void SetSelectedAttribute(Attribute att)
    {
        SelectedAttribute = att;
    }

    public void DisplayAttributeBreakdown()
    {
        if (SelectedAttribute == null)
        {
            AttributeBreakdownText.text = "";
            return;
        }

        string text = SelectedAttribute.Name + "\n\n";
        if(SelectedAttribute.Description != "") text += SelectedAttribute.Description + "\n\n";

        text += SelectedAttribute.GetValueBreakdownText();
        AttributeBreakdownText.text = text;

        UpdateLayout();
    }

    private void UpdateLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(AttributeBreakdownText.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(AttributeListContainer.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }
}
