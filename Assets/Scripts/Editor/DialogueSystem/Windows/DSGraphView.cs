using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSGraphView : GraphView
{
    private DSEditorWindow _editorWindow;
    private DSSearchWindow _searchWindow;

    private SerializableDictionary<string, DSNodeErrorData> _ungroupedNodes;

    public DSGraphView(DSEditorWindow dsEditorWindow) 
    {
        _editorWindow = dsEditorWindow;

        _ungroupedNodes = new SerializableDictionary<string, DSNodeErrorData>();

        AddManipulators();
        AddSearchWindow();
        GenerateGridBackground();

        OnElementsDeleted();

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

        this.AddManipulator(CreateGroupContextualMenu());
    }
    private IManipulator CreateGroupContextualMenu()
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("Dialogue Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
        );

        return contextualMenuManipulator;
    }


    private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
        );

        return contextualMenuManipulator;
    }
    #endregion

    #region Elements Creation
    public Group CreateGroup(string title, Vector2 localMousePosition)
    {
        Group group = new Group()
        {
            title = title
        };

        group.SetPosition(new Rect(localMousePosition, Vector2.zero));

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
            List<DSNode> nodesToDelete = new List<DSNode>();

            foreach(GraphElement element in selection)
            {
                if (element is DSNode)
                {
                    nodesToDelete.Add((DSNode) element);

                    continue;
                }
            }

            foreach(DSNode node in nodesToDelete)
            {
                RemoveUngroupedNode(node);

                RemoveElement(node);
            }
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
            ungroupedNodesList[0].ResetStyle();

            return;
        }

        if (ungroupedNodesList.Count == 0)
        {
            _ungroupedNodes.Remove(nodeName);
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
