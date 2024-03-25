using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    private static DialogueManager _instance;
    public static DialogueManager Instance => _instance;

    private void InitialiazeSingleton()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private void Awake()
    {
        InitialiazeSingleton();
    }

    public void StartDialogue(DSDialogue dialogue)
    {
        Debug.Log($"Dialogue Started : {dialogue.StartingDialogue.DialogueName}");
    }
}
