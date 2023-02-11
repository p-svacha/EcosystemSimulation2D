using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class BodyPartLibrary
{
    public static Dictionary<BodyPartId, List<BodyPartData>> BodyParts;

    public static void Init()
    {
        BodyParts = new Dictionary<BodyPartId, List<BodyPartData>>();

        // Retrieve all body part data from json files
        string[] files = Directory.GetFiles("Assets/Resources/Animals/BodyParts", "*.json", SearchOption.AllDirectories);
        foreach(string filePath in files)
        {
            using(StreamReader r = new StreamReader(filePath))
            {
                // Create object
                string json = r.ReadToEnd();
                BodyPartData bodyPart = JsonConvert.DeserializeObject<BodyPartData>(json);
                bodyPart.Path = filePath;

                // Set sprite
                string spritePath = Path.ChangeExtension(filePath, ".png");
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

    public static void SaveBodyPartData(BodyPartData data)
    {
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(data.Path, json);
        Debug.Log("Successfully saved " + data.Name + " data");
    }
}
