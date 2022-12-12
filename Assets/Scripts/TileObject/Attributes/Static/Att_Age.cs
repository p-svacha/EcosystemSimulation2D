using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Age : StaticAttribute<SimulationTime>
{
    // Attribute Base
    private const AttributeId ID = AttributeId.Age;
    private const string CATEGORY = "General";
    private const string NAME = "Age";
    private const string DESCRIPTION = "How long an object has been existing in the world.";

    // Individual
    private TileObjectBase Object;

    public Att_Age(TileObjectBase obj) : base(obj, ID, CATEGORY, NAME, DESCRIPTION, null)
    {
        Object = obj;
    }

    public override float GetValue() => Simulation.Singleton.CurrentTime - (Object.Attributes[AttributeId.CreatedAt] as StaticAttribute<SimulationTime>).GetStaticValue();
    public override string GetValueString() => new SimulationTime(GetValue()).ToString();
}
