using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Responsible for opening dynamic windows.
/// Attach to Canvas
/// </summary>
public class UIHandler : MonoBehaviour
{
    [Header("Elements")]
    public GameObject ModularWindowsContainer;

    [Header("Prefabs")]
    public UI_ThingInfoWindow ThingInfoWindowPrefab;
    public UI_SelectionWindow SelectionWindowPrefab;

    // Open Windows
    private Dictionary<IThing, UI_ThingInfoWindow> ThingInfoWindows = new Dictionary<IThing, UI_ThingInfoWindow>();
    private Dictionary<IThing, UI_SelectionWindow> SelectionWindows = new Dictionary<IThing, UI_SelectionWindow>();

    #region ThingInfoWindow

    public void AddThingInfoWindow(IThing thing)
    {
        if (ThingInfoWindows.ContainsKey(thing)) return;
        UI_ThingInfoWindow newWindow = Instantiate(ThingInfoWindowPrefab, ModularWindowsContainer.transform);
        newWindow.Init(thing);
        newWindow.transform.localPosition = GetNewThingWindowPosition();
        ThingInfoWindows.Add(thing, newWindow);
    }
    public void CloseThingInfoWindow(IThing thing)
    {
        if (!ThingInfoWindows.ContainsKey(thing)) return;
        DestroyImmediate(ThingInfoWindows[thing].gameObject);
        ThingInfoWindows.Remove(thing);

        CloseSelectionWindow(thing);
    }
    private Vector3 GetNewThingWindowPosition()
    {
        return new Vector3(400 + 50 * ThingInfoWindows.Count, 750 - 50 * ThingInfoWindows.Count, 0);
    }

    #endregion

    #region SelectionWindow

    public void AddSelectionWindow(IThing thing)
    {
        if (SelectionWindows.ContainsKey(thing)) return;
        UI_SelectionWindow newWindow = Instantiate(SelectionWindowPrefab, ModularWindowsContainer.transform);
        newWindow.Init(thing);
        newWindow.transform.localPosition = GetNewSelectionWindowPosition();
        SelectionWindows.Add(thing, newWindow);
    }
    public void CloseSelectionWindow(IThing thing)
    {
        if (!SelectionWindows.ContainsKey(thing)) return;
        DestroyImmediate(SelectionWindows[thing].gameObject);
        SelectionWindows.Remove(thing);

        CloseThingInfoWindow(thing);
    }
    private Vector3 GetNewSelectionWindowPosition()
    {
        int selectionWindowsPerRow = 4;
        float xGap = 470;
        float yGap = 50;
        return new Vector3(xGap * (SelectionWindows.Count % selectionWindowsPerRow), yGap * (SelectionWindows.Count / selectionWindowsPerRow), 0);
    }

    #endregion

    public static UIHandler Singleton { get { return GameObject.Find("Canvas").GetComponent<UIHandler>(); } }
}
