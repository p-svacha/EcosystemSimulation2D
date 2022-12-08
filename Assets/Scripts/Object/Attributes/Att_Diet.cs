using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Diet : StaticAttribute<List<NutrientType>>
{
    // Attribute Base
    private const AttributeId ID = AttributeId.Diet;
    private const string CATEGORY = "Needs";
    private const string NAME = "Diet";
    private const string DESCRIPTION = "What types of food an animal is able to eat.";

    // Individual
    public List<NutrientType> Diet { get; private set; }

    public Att_Diet(TileObject obj, List<NutrientType> diet) : base(obj, ID, CATEGORY, NAME, DESCRIPTION, diet)
    {
        Diet = diet;
    }

    public override string GetValueString() => HelperFunctions.ListToString(Diet);
}
