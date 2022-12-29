using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Diet : StaticAttribute<List<NutrientType>>
{
    // Attribute Base
    private const AttributeId ID = AttributeId.Diet;
    private const string CATEGORY = "Needs";
    private const string NAME = "Diet";

    // Individual
    public List<NutrientType> Diet { get; private set; }

    public Att_Diet(List<NutrientType> diet) : base(ID, NAME, CATEGORY, diet)
    {
        Diet = diet;
    }

    public override string GetValueString() => HelperFunctions.ListToString(Diet);
}
