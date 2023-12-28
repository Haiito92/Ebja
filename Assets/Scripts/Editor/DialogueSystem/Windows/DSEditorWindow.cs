using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;

public class DSEditorWindow : EditorWindow
{
    [MenuItem("Tools/Dialogue Graph")]
    public static void OpenWindow()
    {
        GetWindow<DSEditorWindow>("Dialogue Graph");
    }

    private void CreateGUI()
    {
        GenerateGraphView();

        AddStyles();
    }

    #region Elements Addition
    private void GenerateGraphView()
    {
        DSGraphView graphView = new DSGraphView();

        graphView.StretchToParentSize();

        rootVisualElement.Add(graphView);
    }

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets("Assets/Scripts/Editor/Resources/DialogueSystem/DSVariables.uss");
    }
    #endregion
}
