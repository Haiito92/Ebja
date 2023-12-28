using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DSSingleChoiceNode : DSNode
{
    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);

        DialogueType = DSDialogueType.SingleChoice;

        Choices.Add("Next Dialogue");
    }

    public override void Draw()
    {
        base.Draw();

        /* OUTPUT CONTAINER */

        foreach (string choice in Choices)
        {
            Port choicePort = this.CreatePort(choice);

            outputContainer.Add(choicePort);
        }

        RefreshExpandedState();
    }
}
