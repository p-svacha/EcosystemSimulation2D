using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Body Part data as it is loaded from a json in Assets/Resources/Animals/BodyParts.
/// </summary>
[Serializable]
public class BodyPartData
{
    public string Name { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public BodyPartId BodyPartId { get; set; }

    public List<BodyPartConnectorData> Connectors { get; set; }

    public Sprite Sprite;

    [NonSerialized]
    public string Path;

    public override string ToString()
    {
        string s = "Body Part Info for " + Name + " (" + BodyPartId.ToString() + ")";
        foreach(BodyPartConnectorData c in Connectors)
        {
            s += "\nConnector " + c.BodyPartId.ToString() + ": " + c.x + "/" + c.y;
        }
        return s;
    }
}