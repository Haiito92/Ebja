using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CustomInspectorUtility
{
    public static void DrawProperties(params SerializedProperty[] properties)
    {
        foreach (SerializedProperty property in properties)
        {
            EditorGUILayout.PropertyField(property);
        }
    }
}
