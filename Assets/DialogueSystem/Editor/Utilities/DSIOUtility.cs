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

    private static List<DSGroup> _groups;
    private static List<DSNode> _nodes;

    private static Dictionary<string, DSDialogueGroupSO> _createdDialogueGroups;

    public static void Initialize(DSGraphView dsGraphView, string graphName)
    {
        _graphView = dsGraphView;

        _graphFileName = graphName;
        _containerFolderPath = $"Assets/DialogueSystem/Runtime/Dialogues/{_graphFileName}";

        _groups = new List<DSGroup>();
        _nodes = new List<DSNode>();

        _createdDialogueGroups = new Dictionary<string, DSDialogueGroupSO>();
    }

    #region Save Methods
    public static void Save()
    {
        CreateStaticFolders();

        GetElementsFromGraphView();

        DSGraphSaveDataSO graphData = CreateAsset<DSGraphSaveDataSO>("Assets/DialogueSystem/Editor/Graphs", $"{_graphFileName}Graph");

        graphData.Initialize(_graphFileName);

        DSDialogueContainerSO dialogueContainer = CreateAsset<DSDialogueContainerSO>(_containerFolderPath, _graphFileName);

        dialogueContainer.Initialize(_graphFileName);

        SaveGroups(graphData, dialogueContainer);
        SaveNodes(graphData, dialogueContainer);

        SaveAsset(graphData);
        SaveAsset(dialogueContainer);
    }

    #region Groups
    private static void SaveGroups(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
    {
        foreach(DSGroup group in _groups)
        {
            SaveGroupToGraph(group, graphData);
            SaveGroupToScriptableObject(group, dialogueContainer);
        }
    }

    private static void SaveGroupToGraph(DSGroup group, DSGraphSaveDataSO graphData)
    {
        DSGroupSaveData groupData = new DSGroupSaveData()
        {
            ID = group.ID,
            Name = group.title,
            Position = group.GetPosition().position
        };

        graphData.Groups.Add(groupData);
    }

    private static void SaveGroupToScriptableObject(DSGroup group, DSDialogueContainerSO dialogueContainer)
    {
        string groupName = group.title;

        CreateFolder($"{_containerFolderPath}/Groups", groupName);
        CreateFolder($"{_containerFolderPath}/Groups/{groupName}", "Dialogues");

        DSDialogueGroupSO dialogueGroup = CreateAsset<DSDialogueGroupSO>($"{_containerFolderPath}/Groups/{groupName}", groupName);

        dialogueGroup.Initialize(groupName);

        _createdDialogueGroups.Add(group.ID, dialogueGroup);

        dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<DSDialogueSO>());

        SaveAsset(dialogueGroup);
    }
    #endregion

    #region Nodes
    private static void SaveNodes(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
    {
        foreach(DSNode node in _nodes)
        {
            SaveNodeToGraph(node, graphData);
            SaveNodeToScriptableObject(node, dialogueContainer);
        }
    }

    private static void SaveNodeToGraph(DSNode node, DSGraphSaveDataSO graphData)
    {
        List<DSChoiceSaveData> choices = new List<DSChoiceSaveData>();

        foreach(DSChoiceSaveData choice in node.Choices)
        {
            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = choice.Text,
                NodeID = choice.NodeID,
            };

            choices.Add(choiceData);
        }

        DSNodeSaveData nodeData = new DSNodeSaveData()
        {
            ID = node.ID,
            Name = node.DialogueName,
            Choices = choices,
            Text = node.Text,
            GroupID = node.Group?.ID,
            DialogueType = node.DialogueType,
            Position = node.GetPosition().position
        };

        graphData.Nodes.Add(nodeData);
    }

    private static void SaveNodeToScriptableObject(DSNode node, DSDialogueContainerSO dialogueContainer)
    {
        DSDialogueSO dialogue;

        if(node.Group != null)
        {
            dialogue = CreateAsset<DSDialogueSO>($"{_containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
        }
    }
    #endregion

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
        Type groupType = typeof(DSGroup);

        _graphView.graphElements.ForEach(graphElement =>
        {

            if(graphElement is DSNode node)
            {
                _nodes.Add(node);

                return;
            }

            if(graphElement.GetType() == groupType)
            {
                DSGroup group = (DSGroup) graphElement;

                _groups.Add(group);

                return;
            }

        });
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

    private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        string fullPath = $"{path}/{assetName}.asset";

        T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);

        if(asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, fullPath);
        }

        return asset;
    }

    private static void SaveAsset(UnityEngine.Object asset)
    {
        EditorUtility.SetDirty(asset);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    #endregion
}
