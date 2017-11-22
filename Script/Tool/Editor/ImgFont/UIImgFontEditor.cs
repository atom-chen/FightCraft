
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;


/// <summary>
/// Custom Editor for editing vertices and exporting the mesh.
/// </summary>
[CustomEditor(typeof(UIImgFont))]
public class UIImgFontEditor : Editor
{
    //navmesh object reference
    private UIImgFont script;

    private bool placing;


    void OnEnable()
    {
        script = (UIImgFont)target;
    }


    /// <summary>
    /// Custom inspector override for buttons.
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();

        if (GUILayout.Button("FitChars"))
        {
            var chars = script.GetComponentsInChildren<UIImgChar>();
            foreach (var _char in chars)
            {
                _char._CharHeight = _char._Image.rect.height;
                _char._CharWidth = _char._Image.rect.width;
            }
        }


    }

}
