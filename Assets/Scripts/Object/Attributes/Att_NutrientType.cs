using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_NutrientType : StaticAttribute<NutrientType>
{
    // Attribute Base
    private const AttributeId ID = AttributeId.NutrientType;
    private const string CATEGORY = "Nutrition";
    private const string NAME = "Nutrient Type";
    private const string DESCRIPTION = "What kind of diet is needed to be able to eat this object.";

    // Individual
    public NutrientType NutrientType { get; private set; }

    public Att_NutrientType(TileObject obj, NutrientType nutrientType) : base(obj, ID, CATEGORY, NAME, DESCRIPTION, nutrientType)
    {
        NutrientType = nutrientType;
    }

    public override string GetValueString()
    {
        return NutrientType.ToString();
    }
}
