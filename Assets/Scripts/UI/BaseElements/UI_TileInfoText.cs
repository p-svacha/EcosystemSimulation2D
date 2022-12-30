using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_TileInfoText : MonoBehaviour
{
    [Header("Elements")]
    public TextMeshProUGUI TileInfoText;

    float deltaTime; // for fps

    private void Update()
    {
        WorldTile tile = Simulation.Singleton.HoveredWorldTile;

        if(tile == null)
        {
            TileInfoText.text = "";
            return;
        }

        string text = "";
        text += "Coordinates: " + tile.Coordinates.ToString();
        text += "\nSurface: " + tile.Surface.Name;
        text += "\nElevation: " + tile.MaxElevation + " / " + tile.ElevationType.ToString() + " (" + tile.ElevationDirection.ToString() + ")";
        if(tile.TileObjects.Count > 0)
        {
            text += "\nObjects: ";
            foreach (VisibleTileObjectBase tobj in tile.TileObjects) text += tobj.Name + ", ";
            text = text.TrimEnd(' ');
            text = text.TrimEnd(',');
        }

        // Add FPS
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        text += "\n" + Mathf.Ceil(fps).ToString() + " FPS (" + Simulation.Singleton.NumObjects + " Objects)";


        TileInfoText.text = text;
    }


    public static UI_TileInfoText Singleton { get { return GameObject.Find("TileInfo").GetComponent<UI_TileInfoText>(); } }
}
