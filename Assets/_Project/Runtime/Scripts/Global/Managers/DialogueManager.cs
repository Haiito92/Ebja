using System;
using UnityEngine;

namespace _Project.Runtime.Scripts.Global.Managers
{
    using UI;
    
    public class DialogueManager : MonoBehaviour
    {
        //Ref to UI
        [SerializeField] private DialogueUI _dialogueUI;
        
        //Events
        public event Action OnDialogueStarted;  
        public event Action OnDialogueEnded;  
        
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

        private void Start()
        {
            if (_dialogueUI != null)
            {
                _dialogueUI.OnDialogueDisplayEnd += EndDialogue;
            }
        }

        public void StartDialogue(DSDialogue dialogue)
        {
            _dialogueUI.StartNewDialogue(dialogue);
            OnDialogueStarted?.Invoke();
        }

        private void CutDialogue()
        {
            throw new NotImplementedException();
        }

        private void EndDialogue()
        {
            OnDialogueEnded?.Invoke();
        }
    }
}
