using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(NoiseTester))]
public class NoiseTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NoiseTester noiseTester = (NoiseTester)target;

        // Every time a value is changed
        if (DrawDefaultInspector())
        {

        }


        // Add a Generate button that generates a new map with the current values
        if (GUILayout.Button("Show Noise"))
        {
            noiseTester.DisplayNoise();
        }

    }
}
