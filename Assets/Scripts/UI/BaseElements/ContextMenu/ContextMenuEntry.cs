using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenuEntry
{
    public ContextMenuEntry(string text, Action action)
    {
        Text = text;
        Action = action;
    }

    public string Text { get; private set; }
    public System.Action Action { get; private set; }
}
