using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_NutrientType : StaticAttribute<NutrientType>
{
    // Attribute Base
    private const AttributeId ID = AttributeId.NutrientType;
    private const string CATEGORY = "Nutrition";
    private const string NAME = "Nutrient Type";

    // Individual
    public NutrientType NutrientType { get; private set; }

    public Att_NutrientType(NutrientType nutrientType) : base(ID, NAME, CATEGORY, nutrientType)
    {
        NutrientType = nutrientType;
    }

    public override string GetValueString()
    {
        return NutrientType.ToString();
    }
}
