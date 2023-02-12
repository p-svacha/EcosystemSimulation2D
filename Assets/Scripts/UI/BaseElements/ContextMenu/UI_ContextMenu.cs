using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ContextMenu : MonoBehaviour
{
    [Header("Prefabs")]
    public UI_ContextMenuEntry ContextMenuEntryPrefab;

    [Header("Elements")]
    public GameObject Container;

    private void Start()
    {
        _Singleton = GameObject.Find("ContextMenu").GetComponent<UI_ContextMenu>();
        gameObject.SetActive(false);
    }

    public void Show(List<ContextMenuEntry> entries)
    {
        HelperFunctions.DestroyAllChildredImmediately(Container);

        foreach(ContextMenuEntry entry in entries)
        {
            UI_ContextMenuEntry uiEntry = Instantiate(ContextMenuEntryPrefab, Container.transform);
            uiEntry.Init(this, entry);
        }

        transform.position = Input.mousePosition;

        gameObject.SetActive(true);
    }

    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            Hide();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private static UI_ContextMenu _Singleton;
    public static UI_ContextMenu Singleton => _Singleton;
}
