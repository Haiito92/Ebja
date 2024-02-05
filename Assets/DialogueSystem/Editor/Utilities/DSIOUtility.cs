using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DSIOUtility
{
    private static DSGraphView _graphView;

    private static string _graphFileName;
    private static string _containerFolderPath;

    public static void Initialize(string graphName)
    {
        _graphFileName = graphName;
        _containerFolderPath = $"Assets/DialogueSystem/Runtime/Dialogues/{_graphFileName}";
    }

    #region Save Methods
    public static void Save()
    {
        CreateStaticFolders();

        GetElementsFromGraphView();
    }
    #endregion

    #region Creation Methods
    private static void CreateStaticFolders()
    {
        CreateFolder("Assets/DialogueSystem/Editor", "Graphs");

        CreateFolder("Assets/DialogueSystem/Runtime", "Dialogues");

        CreateFolder("Assets/DialogueSystem/Runtime/Dialogues", _graphFileName);
        CreateFolder(_containerFolderPath, "Global");
        CreateFolder(_containerFolderPath, "Groups");
        CreateFolder($"{_containerFolderPath}/Global", "Dialogues");
    }
    #endregion

    #region Fetch Methods
    private static void GetElementsFromGraphView()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Utility Methods
    private static void CreateFolder(string path, string fileName)
    {
        if (AssetDatabase.IsValidFolder($"{path}/{fileName}"))
        {
            return;
        }

        AssetDatabase.CreateFolder(path, fileName);
    }
    #endregion
}
