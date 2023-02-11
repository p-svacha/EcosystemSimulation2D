using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Body Part connector data as it is loaded from a json in Assets/Resources/Animals/BodyParts.
/// </summary>
[Serializable]
public class BodyPartConnectorData
{
    [JsonConverter(typeof(StringEnumConverter))]
    public BodyPartId BodyPartId { get; set; }
    public float x { get; set; }
    public float y { get; set; }
}

