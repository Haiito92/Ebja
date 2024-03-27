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

    #region Properties
    
    public DSDialogueSO Dialogue => _dialogue;
    
    #endregion
}
