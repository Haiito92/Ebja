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
    private DSGraphView _graphView;

    private readonly string _defaultFileName = "DialoguesFileName";

    private static TextField _fileNameTextField;
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
        _graphView = new DSGraphView(this);

        _graphView.StretchToParentSize();

        rootVisualElement.Add(_graphView);
    }

    private void AddToolbar()
    {
        Toolbar toolbar = new Toolbar();

        _fileNameTextField = DSElementUtility.CreateTextField(_defaultFileName, "File Name:", callback =>
        {
            _fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
        });

        _saveButton = DSElementUtility.CreateButton("Save", () => Save());

        Button clearButton = DSElementUtility.CreateButton("Clear", () => Clear());
        Button resetButton = DSElementUtility.CreateButton("Reset", () => ResetGraph());

        toolbar.Add(_fileNameTextField);
        toolbar.Add(_saveButton);
        toolbar.Add(clearButton);
        toolbar.Add(resetButton);

        toolbar.AddStyleSheets("DSToolbarStyles");

        rootVisualElement.Add(toolbar);
    }

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets("DSVariables");
    }
    #endregion

    #region Toolbar Actions
    private void Save()
    {
        if(string.IsNullOrEmpty(_fileNameTextField.value))
        {
            EditorUtility.DisplayDialog(
                "Invalid file name",
                "Please ensure the file name you've typed in is valid",
                "Roger!"
            );

            return;
        }

        DSIOUtility.Initialize(_graphView, _fileNameTextField.value);
        DSIOUtility.Save();
    }

    private void Clear()
    {
        _graphView.ClearGraph();
    }

    private void ResetGraph()
    {
        Clear();

        UpdateFileName(_defaultFileName);
    }
    #endregion

    #region Utility Methods
    public static void UpdateFileName(string newFileName)
    {
        _fileNameTextField.value = newFileName;
    }

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
