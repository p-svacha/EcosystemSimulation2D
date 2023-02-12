using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class BodyPartLibrary
{
    public static Dictionary<BodyPartId, List<BodyPartData>> BodyParts;
    public const int BODY_PART_SPRITE_SIZE = 512;

    public static void Init()
    {
        BodyParts = new Dictionary<BodyPartId, List<BodyPartData>>();

        // Retrieve all body part sprites
        string[] files = Directory.GetFiles("Assets/Resources/Animals/BodyParts", "*.png", SearchOption.AllDirectories);
        foreach(string pngFilePath in files)
        {
            // Check if the sprite has a json with the same name
            string jsonFilePath = Path.ChangeExtension(pngFilePath, ".json");
            if(!File.Exists(jsonFilePath)) // Create a new json if one does not exist already
            {
                string bodyPartType = new DirectoryInfo(jsonFilePath).Parent.Name;

                BodyPartData newBodyPartData = new BodyPartData()
                {
                    Name = Path.GetFileNameWithoutExtension(jsonFilePath),
                    Path = jsonFilePath,
                    Connectors = new List<BodyPartConnectorData>(),
                    BodyPartId = (BodyPartId) (Enum.Parse(typeof(BodyPartId), bodyPartType))
                };

                SaveBodyPartData(newBodyPartData);
            }

            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                // Create object
                string json = r.ReadToEnd();
                BodyPartData bodyPart = JsonConvert.DeserializeObject<BodyPartData>(json);
                bodyPart.Path = jsonFilePath;

                // Set sprite
                string spritePath = Path.ChangeExtension(jsonFilePath, ".png");
                bodyPart.Sprite = HelperFunctions.LoadNewSprite(spritePath, 512);

                // Save to library
                if (BodyParts.ContainsKey(bodyPart.BodyPartId)) BodyParts[bodyPart.BodyPartId].Add(bodyPart);
                else BodyParts.Add(bodyPart.BodyPartId, new List<BodyPartData>() { bodyPart });
            }
        }


        foreach (var category in BodyParts)
        {
            Debug.Log("BodyPartLibrary: Found " + category.Value.Count + " " + category.Key);
            foreach (BodyPartData bodyPart in category.Value) Debug.Log(bodyPart);
        }
    }

    /// <summary>
    /// Checks if all sprites within the body part folder have a json with the same name and creates new json files for those that don't.
    /// </summary>
    private static void CreateMissingJson()
    {

    }

    public static void SaveBodyPartData(BodyPartData data)
    {
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(data.Path, json);
        Debug.Log("Successfully saved " + data.Name + " data");
    }
}
