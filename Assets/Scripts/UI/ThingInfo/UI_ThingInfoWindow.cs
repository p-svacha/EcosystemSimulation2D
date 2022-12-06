using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

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
    public UI_ThingInfoAttribute AttributePrefab;

    private IThing Thing;

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
        Attribute tempIdAtt = new StaticAttribute<string>(thing, AttributeId.Id, "Base", "Thing ID", "Unique key to identify what kind of thing this is.", thing.Id.ToString());
        Attribute tempNameAtt = new StaticAttribute<string>(thing, AttributeId.Name, "Base", "Name", "", thing.Name);
        Attribute tempDescAtt = new StaticAttribute<string>(thing, AttributeId.Description, "Base", "Description", "", thing.Description);

        // Collect all attributes to display
        List<Attribute> attributesToDisplay = new List<Attribute>() { tempIdAtt, tempNameAtt, tempDescAtt };
        foreach (Attribute att in thing.Attributes.Values) attributesToDisplay.Add(att);

        // Group attributes by category
        Dictionary<string, List<Attribute>> groupedAttributes = new Dictionary<string, List<Attribute>>();
        foreach(Attribute att in attributesToDisplay)
        {
            if (!groupedAttributes.ContainsKey(att.Category)) groupedAttributes.Add(att.Category, new List<Attribute>() { att });
            else groupedAttributes[att.Category].Add(att);
        }

        // Display all attributes
        foreach(KeyValuePair<string, List<Attribute>> attGroup in groupedAttributes)
        {
            GameObject catDisplay = Instantiate(AttributeCategoryPrefab, AttributeListContainer.transform);
            catDisplay.GetComponentInChildren<TextMeshProUGUI>().text = attGroup.Key;
            foreach (Attribute att in attGroup.Value)
            {
                UI_ThingInfoAttribute attDisplay = Instantiate(AttributePrefab, AttributeListContainer.transform);
                attDisplay.Init(att, this);
            }
        }

        // Init Layout
        AttributeBreakdownText.text = "";
        UpdateLayout();
    }

    public void ShowAttributeBreakdown(Attribute att)
    {
        string text = att.Name + "\n\n";
        if(att.Description != "") text += att.Description + "\n\n";


        if (att.Type == AttributeType.Dynamic)
        {
            DynamicAttribute numAtt = (DynamicAttribute)att;
            List<DynamicAttributeModifier> modifiers = numAtt.GetValueModifiers();

            DynamicAttributeModifier baseMod = modifiers.First(x => x.Type == AttributeModifierType.BaseValue);
            text += baseMod.Description + ":\t" + baseMod.Value + "\n";

            foreach (DynamicAttributeModifier mod in modifiers.Where(x => x.Type == AttributeModifierType.Add))
                text += "\n" + mod.Description + ":\t" + (mod.Value >= 0 ? "+" : "") + mod.Value;
            foreach (DynamicAttributeModifier mod in modifiers.Where(x => x.Type == AttributeModifierType.Multiply))
                text += "\n" + mod.Description + ":\tx" + mod.Value;
            text += "\n\nFinal Value:\t" + numAtt.GetValue();
        }
        else if(att.Type == AttributeType.Static)
        {
            text += att.GetValueString();
        }

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
