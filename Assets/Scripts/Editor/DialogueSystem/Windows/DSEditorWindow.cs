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
    private readonly string _defaultFileName = "Dialogues File Name";
    private Button _saveButton;

    [MenuItem("Tools/Dialogue Graph")]
    public static void OpenWindow()
    {
        GetWindow<DSEditorWindow>("Dialogue Graph");
    }

    private void CreateGUI()
    {
        AddGraphView();
        AddToolbar();

        AddStyles();
    }

    #region Elements Addition
    private void AddGraphView()
    {
        DSGraphView graphView = new DSGraphView(this);

        graphView.StretchToParentSize();

        rootVisualElement.Add(graphView);
    }

    private void AddToolbar()
    {
        Toolbar toolbar = new Toolbar();

        TextField fileNameTextField = DSElementUtility.CreateTextField(_defaultFileName, "File Name:");

        _saveButton = DSElementUtility.CreateButton("Save");

        toolbar.Add(fileNameTextField);
        toolbar.Add(_saveButton);

        toolbar.AddStyleSheets("Assets/Scripts/Editor/Resources/DialogueSystem/DSToolbarStyles.uss");

        rootVisualElement.Add(toolbar);
    }

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets("Assets/Scripts/Editor/Resources/DialogueSystem/DSVariables.uss");
    }
    #endregion

    #region Utility Methods
    public void EnableSaving()
    {
        _saveButton.SetEnabled(true);
    }

    public void DisableSaving()
    {
        _saveButton.SetEnabled(false);
    }
    #endregion
}
