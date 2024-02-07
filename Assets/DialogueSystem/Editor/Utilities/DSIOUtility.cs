using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private static Dictionary<string, DSDialogueSO> _createdDialogues;

    public static void Initialize(DSGraphView dsGraphView, string graphName)
    {
        _graphView = dsGraphView;

        _graphFileName = graphName;
        _containerFolderPath = $"Assets/DialogueSystem/Runtime/Dialogues/{_graphFileName}";

        _groups = new List<DSGroup>();
        _nodes = new List<DSNode>();

        _createdDialogueGroups = new Dictionary<string, DSDialogueGroupSO>();
        _createdDialogues = new Dictionary<string, DSDialogueSO>();
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
        List<string> groupNames = new List<string>();

        foreach(DSGroup group in _groups)
        {
            SaveGroupToGraph(group, graphData);
            SaveGroupToScriptableObject(group, dialogueContainer);

            groupNames.Add(group.title);
        }

        UpdateOldGroups(groupNames, graphData);
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

    private static void UpdateOldGroups(List<string> currentGroupNames, DSGraphSaveDataSO graphData)
    {
        if(graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
        {
            List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

            foreach(string groupToRemove in groupsToRemove)
            {
                RemoveFolder($"{_containerFolderPath}/Groups/{groupToRemove}");
            }
        }

        graphData.OldGroupNames = new List<string>(currentGroupNames);
    }
    #endregion

    #region Nodes
    private static void SaveNodes(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
    {
        SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
        List<string> ungroupedNodeNames = new List<string>();

        foreach(DSNode node in _nodes)
        {
            SaveNodeToGraph(node, graphData);
            SaveNodeToScriptableObject(node, dialogueContainer);

            if(node.Group != null)
            {
                groupedNodeNames.AddItem(node.Group.title, node.DialogueName);

                continue;
            }

            ungroupedNodeNames.Add(node.DialogueName);
        }

        UpdateDialoguesChoicesConnections();

        UpdateOldGroupedNodes(groupedNodeNames, graphData);
        UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
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

            dialogueContainer.DialogueGroups.AddItem(_createdDialogueGroups[node.Group.ID], dialogue);
        }
        else
        {
            dialogue = CreateAsset<DSDialogueSO>($"{_containerFolderPath}/Global/Dialogues", node.DialogueName);

            dialogueContainer.UngroupedDialogues.Add(dialogue);
        }

        dialogue.Initialize(
            node.DialogueName,
            node.Text,
            ConvertNodeChoicesToDialogueChoices(node.Choices),
            node.DialogueType,
            node.IsStartingNode()
        );

        _createdDialogues.Add(node.ID, dialogue);

        SaveAsset(dialogue);
    }

    private static List<DSDialogueChoiceData> ConvertNodeChoicesToDialogueChoices(List<DSChoiceSaveData> nodeChoices)
    {
        List<DSDialogueChoiceData> dialogueChoices = new List<DSDialogueChoiceData>();

        foreach(DSChoiceSaveData nodeChoice in nodeChoices)
        {
            DSDialogueChoiceData choiceData = new DSDialogueChoiceData()
            {
                Text = nodeChoice.Text,
            };

            dialogueChoices.Add(choiceData);
        }

        return dialogueChoices;
    }

    private static void UpdateDialoguesChoicesConnections()
    {
        foreach(DSNode node in _nodes)
        {
            DSDialogueSO dialogue = _createdDialogues[node.ID];

            for(int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex)
            {
                DSChoiceSaveData nodeChoice = node.Choices[choiceIndex];

                if (string.IsNullOrEmpty(nodeChoice.NodeID))
                {
                    continue;
                }

                dialogue.Choices[choiceIndex].NextDialogue = _createdDialogues[nodeChoice.NodeID];

                SaveAsset(dialogue);
            } 

        }
    }

    private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, DSGraphSaveDataSO graphData)
    {
        if(graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count != 0)
        {
            foreach(KeyValuePair<string, List<string>> oldGroupedNode in graphData.OldGroupedNodeNames)
            {
                List<string> nodesToRemove = new List<string>();

                if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                {
                    nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key]).ToList();
                }

                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{_containerFolderPath}/Groups/{oldGroupedNode.Key}/Dialogues", nodeToRemove);
                }
            }
        }

        graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
    }

    private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, DSGraphSaveDataSO graphData)
    {
        if(graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
        {
            List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();

            foreach(string nodeToRemove in nodesToRemove)
            {
                RemoveAsset($"{_containerFolderPath}/Global/Dialogues", nodeToRemove);
            }
        }

        graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
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

    private static void RemoveFolder(string fullPath)
    {
        FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
        FileUtil.DeleteFileOrDirectory($"{fullPath}/");
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

    private static void RemoveAsset(string path, string assetName)
    {
        AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
    }

    private static void SaveAsset(UnityEngine.Object asset)
    {
        EditorUtility.SetDirty(asset);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    #endregion
}
