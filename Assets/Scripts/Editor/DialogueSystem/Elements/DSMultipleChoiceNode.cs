using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSMultipleChoiceNode : DSNode
{
    public override void Initialize(DSGraphView dsGraphView, Vector2 position)
    {
        base.Initialize(dsGraphView, position);

        DialogueType = DSDialogueType.MultipleChoice;

        Choices.Add("New Choice");
    }

    public override void Draw()
    {
        base.Draw();

        /* MAIN CONTAINER */

        Button addChoiceButton = DSElementUtility.CreateButton("Add Choice", () =>
        {
            Port choicePort = CreateChoicePort("New Choice");

            Choices.Add("New Choice");

            outputContainer.Add(choicePort);
        });

        addChoiceButton.AddToClassList("ds-node__button");

        mainContainer.Insert(1, addChoiceButton);

        /* OUTPUT CONTAINER */

        foreach (string choice in Choices)
        {
            Port choicePort = CreateChoicePort(choice);

            outputContainer.Add(choicePort);
        }

        RefreshExpandedState();
    }

    #region Elements Creation
    private Port CreateChoicePort(string choice)
    {
        Port choicePort = this.CreatePort();

        Button deleteChoiceButton = DSElementUtility.CreateButton("X");

        deleteChoiceButton.AddToClassList("ds-node__button");

        TextField choiceTextField = DSElementUtility.CreateTextField(choice);

        choiceTextField.AddClasses(
            "ds.node__textfield",
            "ds-node__choice-textfield",
            "ds-node__textfield__hidden"
        );
        choiceTextField.style.flexDirection = FlexDirection.Column;

        choicePort.Add(choiceTextField);
        choicePort.Add(deleteChoiceButton);
        return choicePort;
    }
    #endregion
}
