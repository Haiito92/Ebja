using System.Collections.Generic;
using UnityEngine;

public class DSDialogue : MonoBehaviour
{
    #region For Custom Inspector Only
    /* Filters */
    [SerializeField] private bool _groupedDialogues;
    [SerializeField] private bool _startingDialoguesOnly;

    /* Indexes */

    [SerializeField] private int _selectedDialogueGroupIndex;
    [SerializeField] private int _selectedDialogueIndex;
    #endregion

    /* Dialogue Scriptable Objects */

    [SerializeField] private DSDialogueContainerSO _dialogueContainer;
    [SerializeField] private DSDialogueGroupSO _dialogueGroup;
    [SerializeField] private DSDialogueSO _dialogue;

    /* Dialogue */

    private DSDialogueSO _startingDialogue;

    #region Properties

    public DSDialogueSO StartingDialogue => _startingDialogue;

    #endregion

    private void Awake()
    {
        SetStartingDialogue();
        //if(_startingDialogue != null ) Debug.Log(_startingDialogue.DialogueName);
    }

    private void SetStartingDialogue()
    {
        foreach(List<DSDialogueSO> dialogues in _dialogueContainer.DialogueGroups.Values)
        {
            foreach(DSDialogueSO dialogue in dialogues)
            {
                if(dialogue.IsStartingDialogue)
                {
                    _startingDialogue = dialogue;
                    return;
                }
            }
        }

        foreach(DSDialogueSO dialogue in _dialogueContainer.UngroupedDialogues)
        {
            if (dialogue.IsStartingDialogue)
            {
                _startingDialogue = dialogue;
                return;
            }
        }
    }
}
