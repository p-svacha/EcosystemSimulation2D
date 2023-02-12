using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ContextMenuEntry : MonoBehaviour
{
    [Header("Elements")]
    public Button Button;
    public TextMeshProUGUI Text;

    public void Init(UI_ContextMenu menu, ContextMenuEntry entry)
    {
        Text.text = entry.Text;
        Button.onClick.AddListener(() => OnClick(menu, entry));
    }

    private void OnClick(UI_ContextMenu menu, ContextMenuEntry entry)
    {
        entry.Action();
        menu.Hide();
    }
}
