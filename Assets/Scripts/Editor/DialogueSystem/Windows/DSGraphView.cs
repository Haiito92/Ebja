using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSGraphView : GraphView
{
    private DSEditorWindow _editorWindow;
    private DSSearchWindow _searchWindow;

    private SerializableDictionary<string, DSNodeErrorData> _ungroupedNodes;
    private SerializableDictionary<string, DSGroupErrorData> _groups;
    private SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>> _groupedNodes;

    private int _repeatedNamesAmount;

    public int RepeatedNamesAmount
    {
        get 
        { 
            return _repeatedNamesAmount; 
        } 

        set
        {
            _repeatedNamesAmount = value;

            if(_repeatedNamesAmount == 0)
            {
                _editorWindow.EnableSaving();
            }

            if(_repeatedNamesAmount == 1)
            {
                _editorWindow.DisableSaving();
            }
        }
    }

    public DSGraphView(DSEditorWindow dsEditorWindow) 
    {
        _editorWindow = dsEditorWindow;

        _ungroupedNodes = new SerializableDictionary<string, DSNodeErrorData>();
        _groups = new SerializableDictionary<string, DSGroupErrorData>();
        _groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>>();

        AddManipulators();
        AddSearchWindow();
        GenerateGridBackground();

        OnElementsDeleted();
        OnGroupElementsAdded();
        OnGroupElementsRemoved();
        OnGroupRenamed();

        AddStyles();
    }

    #region Overrided Methods
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if (startPort == port) return;

            if (startPort.node == port.node) return;

            if (startPort.direction == port.direction) return;

            compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }
    #endregion

    #region Manipulators
    private void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);


        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DSDialogueType.SingleChoice));
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Mutiple Choice)", DSDialogueType.MultipleChoice));

        this.AddManipulator(CreateGroupContextualMenu("Add Group"));
    }

    private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
        );

        return contextualMenuManipulator;
    }

    private IManipulator CreateGroupContextualMenu(string actionTitle)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => CreateGroup("Dialogue Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
        );

        return contextualMenuManipulator;
    }
    #endregion

    #region Elements Creation
    public DSGroup CreateGroup(string title, Vector2 localMousePosition)
    {
        DSGroup group = new DSGroup(title, localMousePosition);

        AddGroup(group);

        AddElement(group);

        foreach (GraphElement selectedElement in selection)
        {
            if (!(selectedElement is DSNode)) 
            {
                continue;
            }

            DSNode node = (DSNode) selectedElement;

            group.AddElement(node);
        }

        return group;
    }

    public DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
    {
        Type nodeType = Type.GetType($"DS{dialogueType}Node");

        DSNode node = (DSNode) Activator.CreateInstance(nodeType);

        node.Initialize(this, position);
        node.Draw();

        AddUngroupedNode(node);

        return node;
    }
    #endregion

    #region Callbacks

    private void OnElementsDeleted()
    {
        deleteSelection = (operationName, askUser) =>
        {
            Type groupType = typeof(DSGroup);
            Type edgeType = typeof(Edge);

            List<DSGroup> groupsToDelete = new List<DSGroup>();
            List<Edge> edgesToDelete = new List<Edge>();
            List<DSNode> nodesToDelete = new List<DSNode>();

            foreach(GraphElement element in selection)
            {
                if (element is DSNode)
                {
                    nodesToDelete.Add((DSNode) element);

                    continue;
                }

                if (element.GetType() == edgeType)
                {
                    Edge edge = (Edge) element;

                    edgesToDelete.Add(edge);

                    continue;
                }

                if(element.GetType() != groupType) 
                {
                    continue;
                }

                DSGroup group = (DSGroup) element;

                groupsToDelete.Add(group);
            }

            foreach(DSGroup group in groupsToDelete)
            {
                List<DSNode> groupNodes = new List<DSNode>();

                foreach(GraphElement groupElement in group.containedElements)
                {
                    if(!(groupElement is DSNode))
                    {
                        continue;
                    }

                    DSNode groupNode = (DSNode) groupElement;

                    groupNodes.Add(groupNode);
                }

                group.RemoveElements(groupNodes);

                RemoveGroup(group);

                RemoveElement(group);
            }

            DeleteElements(edgesToDelete);

            foreach(DSNode node in nodesToDelete)
            {
                if(node.Group != null)
                {
                    node.Group.RemoveElement(node);
                }

                RemoveUngroupedNode(node);

                node.DisconnectAllPorts();

                RemoveElement(node);
            }
        };
    }

    private void OnGroupElementsAdded()
    {
        elementsAddedToGroup = (group, elements) =>
        {
            foreach(GraphElement element in elements)
            {
                if(!(element is DSNode))
                {
                    continue;
                }

                DSGroup nodeGroup = (DSGroup) group;

                DSNode node = (DSNode) element;

                RemoveUngroupedNode(node);
                AddGroupedNode(node, nodeGroup);
            }
        };
    }

    private void OnGroupElementsRemoved()
    {
        elementsRemovedFromGroup = (group, elements) =>
        {
            foreach (GraphElement element in elements)
            {
                if (!(element is DSNode))
                {
                    continue;
                }

                DSNode node = (DSNode)element;

                RemoveGroupedNode(node, group);
                AddUngroupedNode(node);
            }
        };
    }

    private void OnGroupRenamed()
    {
        groupTitleChanged = (group, newTitle) =>
        {
            DSGroup dSGroup = (DSGroup) group;

            RemoveGroup(dSGroup);

            dSGroup.oldTitle = newTitle;

            AddGroup(dSGroup);
        };
    }
    #endregion

    #region Repeated Elements
    public void AddUngroupedNode(DSNode node)
    {
        string nodeName = node.DialogueName;

        if(!_ungroupedNodes.ContainsKey(nodeName))
        {
            DSNodeErrorData nodeErrorData = new DSNodeErrorData();

            nodeErrorData.Nodes.Add(node);

            _ungroupedNodes.Add(nodeName, nodeErrorData);

            return;
        }

        List<DSNode> ungroupedNodesList = _ungroupedNodes[nodeName].Nodes;

        ungroupedNodesList.Add(node);

        Color errorColor = _ungroupedNodes[nodeName].ErrorData.Color;

        node.SetErrorStyle(errorColor);

        if (ungroupedNodesList.Count == 2) 
        {
            ++RepeatedNamesAmount;

            ungroupedNodesList[0].SetErrorStyle(errorColor);
        }
    }
    
    public void RemoveUngroupedNode(DSNode node)
    {
        string nodeName = node.DialogueName;

        List<DSNode> ungroupedNodesList = _ungroupedNodes[nodeName].Nodes;

        ungroupedNodesList.Remove(node);

        node.ResetStyle();

        if (ungroupedNodesList.Count == 1 )
        {
            --RepeatedNamesAmount;

            ungroupedNodesList[0].ResetStyle();

            return;
        }

        if (ungroupedNodesList.Count == 0)
        {
            _ungroupedNodes.Remove(nodeName);
        }
    }

    public void AddGroup(DSGroup group)
    {
        string groupName = group.title;

        if (!_groups.ContainsKey(groupName))
        {
            DSGroupErrorData groupErrorData = new DSGroupErrorData();

            groupErrorData.Groups.Add(group);

            _groups.Add(groupName, groupErrorData);

            return;
        }

        List<DSGroup> groupsList = _groups[groupName].Groups;

        groupsList.Add(group);

        Color errorColor = _groups[groupName].ErrorData.Color;

        group.SetErrorStyle(errorColor);

        if(groupsList.Count == 2)
        {
            ++RepeatedNamesAmount;

            groupsList[0].SetErrorStyle(errorColor);
        }
    }

    private void RemoveGroup(DSGroup group)
    {
        string oldGroupName = group.oldTitle;

        List<DSGroup> groupsList = _groups[oldGroupName].Groups;

        groupsList.Remove(group);

        group.ResetStyle();

        if(groupsList.Count == 1)
        {
            --RepeatedNamesAmount;

            groupsList[0].ResetStyle();

            return;
        }

        if(groupsList.Count == 0) 
        {
            _groups.Remove(oldGroupName);
        }
    }

    public void AddGroupedNode(DSNode node, DSGroup group)
    {
        string nodeName = node.DialogueName;

        node.Group = group;

        if (!_groupedNodes.ContainsKey(group))
        {
            _groupedNodes.Add(group, new SerializableDictionary<string, DSNodeErrorData>());
        }

        if (!_groupedNodes[group].ContainsKey(nodeName))
        {
            DSNodeErrorData nodeErrorData = new DSNodeErrorData();

            nodeErrorData.Nodes.Add(node);

            _groupedNodes[group].Add(nodeName, nodeErrorData);

            return;
        }

        List<DSNode> groupedNodesList = _groupedNodes[group][nodeName].Nodes;

        groupedNodesList.Add(node);

        Color errorColor = _groupedNodes[group][nodeName].ErrorData.Color;

        node.SetErrorStyle(errorColor);

        if (groupedNodesList.Count == 2)
        {
            ++RepeatedNamesAmount;

            groupedNodesList[0].SetErrorStyle(errorColor);
        }
    }

    public void RemoveGroupedNode(DSNode node, Group group)
    {
        string nodeName = node.DialogueName;

        node.Group = null;

        List<DSNode> groupedNodesList = _groupedNodes[group][nodeName].Nodes;

        groupedNodesList.Remove(node);

        node.ResetStyle();

        if(groupedNodesList.Count == 1)
        {
            --RepeatedNamesAmount;

            groupedNodesList[0].ResetStyle();

            return;
        }

        if(groupedNodesList.Count == 0)
        {
            _groupedNodes[group].Remove(nodeName);

            if (_groupedNodes[group].Count == 0)
            {
                _groupedNodes.Remove(group);
            }
        }
    }
    #endregion

    #region Elements Addition

    private void AddSearchWindow()
    {
        if (_searchWindow == null)
        {
            _searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();

            _searchWindow.Initialize(this);
        }

        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    private void GenerateGridBackground()
    {
        GridBackground gridBackground = new GridBackground();

        gridBackground.StretchToParentSize();

        Insert(0, gridBackground);
    }
    
    private void AddStyles()
    {
        this.AddStyleSheets(
            "Assets/Scripts/Editor/Resources/DialogueSystem/DSGraphViewStyles.uss",
            "Assets/Scripts/Editor/Resources/DialogueSystem/DSNodeStyles.uss"
        );
    }
    #endregion

    #region Utilities

    public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
    {
        Vector2 worldMousePosition = mousePosition;

        if (isSearchWindow)
        {
            worldMousePosition -= _editorWindow.position.position;
        }

        Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

        return localMousePosition;
    }

    #endregion
}
