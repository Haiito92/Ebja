using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSMultipleChoiceNode : DSNode
{
    public override void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
    {
        base.Initialize(nodeName, dsGraphView, position);

        DialogueType = DSDialogueType.MultipleChoice;

        DSChoiceSaveData choiceData = new DSChoiceSaveData()
        {
            Text = "New Choice"
        };

        Choices.Add(choiceData);
    }

    public override void Draw()
    {
        base.Draw();

        /* MAIN CONTAINER */

        Button addChoiceButton = DSElementUtility.CreateButton("Add Choice", () =>
        {
            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = "New Choice"
            };

            Choices.Add(choiceData);

            Port choicePort = CreateChoicePort(choiceData);

            outputContainer.Add(choicePort);
        });

        addChoiceButton.AddToClassList("ds-node__button");

        mainContainer.Insert(1, addChoiceButton);

        /* OUTPUT CONTAINER */

        foreach (DSChoiceSaveData choice in Choices)
        {
            Port choicePort = CreateChoicePort(choice);

            outputContainer.Add(choicePort);
        }

        RefreshExpandedState();
    }

    #region Elements Creation
    private Port CreateChoicePort(object userData)
    {
        Port choicePort = this.CreatePort();

        choicePort.userData = userData;

        DSChoiceSaveData choiceData = (DSChoiceSaveData)userData;

        Button deleteChoiceButton = DSElementUtility.CreateButton("X", () =>
        {
            if(Choices.Count == 1) 
            {
                return;
            }

            if(choicePort.connected)
            {
                _graphView.DeleteElements(choicePort.connections);
            }

            Choices.Remove(choiceData);

            _graphView.RemoveElement(choicePort);
        });

        deleteChoiceButton.AddToClassList("ds-node__button");

        TextField choiceTextField = DSElementUtility.CreateTextField(choiceData.Text, null, callback =>
        {
            choiceData.Text = callback.newValue;
        });

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
