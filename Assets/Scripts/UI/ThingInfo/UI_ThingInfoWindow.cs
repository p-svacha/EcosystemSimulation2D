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
        Attribute tempIdAtt = new StaticAttribute<string>(thing, AttributeId.Id, AttributeCategory.Base, "Thing ID", "Unique key to identify what kind of thing this is.", thing.Id.ToString());
        UI_ThingInfoAttribute tempIdAttDisplay = Instantiate(AttributePrefab, AttributeListContainer.transform);
        tempIdAttDisplay.Init(tempIdAtt, this);

        Attribute tempNameAtt = new StaticAttribute<string>(thing, AttributeId.Name, AttributeCategory.Base, "Name", "Name of this thing.", thing.Name);
        UI_ThingInfoAttribute tempNameAttDisplay = Instantiate(AttributePrefab, AttributeListContainer.transform);
        tempNameAttDisplay.Init(tempNameAtt, this);

        Attribute tempDescAtt = new StaticAttribute<string>(thing, AttributeId.Description, AttributeCategory.Base, "Description", "Description of the thing.", thing.Description);
        UI_ThingInfoAttribute tempDescAttDisplay = Instantiate(AttributePrefab, AttributeListContainer.transform);
        tempDescAttDisplay.Init(tempDescAtt, this);

        // Display all attributes
        foreach (Attribute att in thing.Attributes.Values)
        {
            UI_ThingInfoAttribute attDisplay = Instantiate(AttributePrefab, AttributeListContainer.transform);
            attDisplay.Init(att, this);
        }

        // Init Layout
        AttributeBreakdownText.text = "";
        UpdateLayout();
    }

    public void ShowAttributeBreakdown(Attribute att)
    {
        string text = att.Name + "\n\n" + att.Description + "\n\n\n";

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
