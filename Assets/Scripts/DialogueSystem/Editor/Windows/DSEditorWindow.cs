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
    private readonly string _defaultFileName = "DialoguesFileName";
    private TextField _fileNameTextField;
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

        _fileNameTextField = DSElementUtility.CreateTextField(_defaultFileName, "File Name:", callback =>
        {
            _fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
        });

        _saveButton = DSElementUtility.CreateButton("Save");

        toolbar.Add(_fileNameTextField);
        toolbar.Add(_saveButton);

        toolbar.AddStyleSheets("DSToolbarStyles");

        rootVisualElement.Add(toolbar);
    }

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets("DSVariables");
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
