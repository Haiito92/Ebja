using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSNode : Node
{
    public string DialogueName { get; set; }
    public List<string> Choices { get; set; }
    public string Text { get; set; }
    public DSDialogueType DialogueType { get; set; }
    public Group Group { get; set; }

    private DSGraphView _graphView;
    private Color _defaultBackgroundColor;

    public virtual void Initialize(DSGraphView dsGraphView, Vector2 position)
    {
        DialogueName = "DialogueName";
        Choices = new List<string>();
        Text = "Dialogue text.";

        _graphView = dsGraphView;
        _defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

        SetPosition(new Rect(position, Vector2.zero));

        mainContainer.AddToClassList("ds-node__main-container");
        extensionContainer.AddToClassList("ds-node__extension-container");
    }

    public virtual void Draw()
    {
        /* TITLE CONTAINER */

        TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, callback =>
        {
            if(Group == null)
            {
                _graphView.RemoveUngroupedNode(this);

                DialogueName = callback.newValue;

                _graphView.AddUngroupedNode(this);

                return;
            }

            Group currentGroup = Group;

            _graphView.RemoveGroupedNode(this, Group);

            DialogueName = callback.newValue;

            _graphView.AddGroupedNode(this, currentGroup);

        });

        dialogueNameTextField.AddClasses(
            "ds-node__textfield",
            "ds-node__filename-textfield",
            "ds-node__textfield__hidden"
        );

        titleContainer.Insert(0, dialogueNameTextField);

        /* INPUT CONTAINER */

        Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

        inputContainer.Add(inputPort);

        /* EXTENSIONS CONTAINER */

        VisualElement customDataContainer = new VisualElement();

        customDataContainer.AddToClassList("ds-node__custom-data-container");

        Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");

        TextField textTextField = DSElementUtility.CreateTextArea(Text);

        textTextField.AddClasses(
            "ds-node__textfield",
            "ds-node__quote-textfield"
        );

        textFoldout.Add(textTextField);

        customDataContainer.Add(textFoldout);

        extensionContainer.Add(customDataContainer);
    }

    public void SetErrorStyle(Color color)
    {
        mainContainer.style.backgroundColor = color;
    }

    public void ResetStyle()
    {
        mainContainer.style.backgroundColor = _defaultBackgroundColor;
    }
}
