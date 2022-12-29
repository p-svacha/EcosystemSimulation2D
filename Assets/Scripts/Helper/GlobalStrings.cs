using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

/// <summary>
/// Handles reading of constant strings that are stored in GlobalStrings.xml.
/// </summary>
public static class GlobalStrings
{
    public const string GLOBAL_STRINGS_FILE_PATH = "Assets/Resources/Strings/GlobalStrings.xml";

    private static readonly XDocument doc = XDocument.Load(GLOBAL_STRINGS_FILE_PATH);

    public static string GetAttributeDescription(AttributeId id)
    {
        return doc.Root.Element("AttributeDescriptions").Element(id.ToString()).Value;
    }
}
