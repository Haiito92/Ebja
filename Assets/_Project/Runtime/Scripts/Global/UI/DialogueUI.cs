using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Runtime.Scripts.Global.UI
{
    public class DialogueUI : MonoBehaviour
    {
        //Refs
        [SerializeField] private GameObject _parent;
        [SerializeField] private TextMeshProUGUI _dialogueText;

        //Dialogue Displayed
        private Queue<string> _sentencesToDisplay;

        //Writing
        [SerializeField, Tooltip("Number of letters written per second.")] private float _writingRate;
        private Coroutine _writingCoroutine; //Writing Sentence IEnumerator
        
        //Events
        public event Action OnDialogueDisplayStart;
        public event Action OnDialogueDisplayEnd;
        
        public void StartNewDialogue(DSDialogue newDialogue)
        {
            //The logic here suppose that we are only using single choice nodes, needs to be adapted for cases where you have both single choice and multiple choices nodes.

            DSDialogueSO currentDialogue = newDialogue.Dialogue;

            _sentencesToDisplay = new Queue<string>();            
            
            while (currentDialogue != null)
            {
                _sentencesToDisplay.Enqueue(currentDialogue.Text);
                currentDialogue = currentDialogue.Choices[0].NextDialogue;
            }
            
            ShowUI();
            NextSentence();
        }

        public void NextSentence()
        {
            _dialogueText.text = String.Empty;

            if (_writingCoroutine != null)
            {
                StopCoroutine(_writingCoroutine);
                _writingCoroutine = null;
            }

            if (_sentencesToDisplay.Count == 0)
            {
                EndDialogue();
                return;
            }
            
            string nextSentence = _sentencesToDisplay.Dequeue();
            
            _writingCoroutine = StartCoroutine(WriteSentence(nextSentence));
        }

        private IEnumerator WriteSentence(string text)
        {
            foreach (char t in text)
            {
                _dialogueText.text += t;
                yield return new WaitForSeconds(1f/_writingRate);
            }
        }

        private void EndDialogue()
        {
            if (_writingCoroutine != null)
            {
                StopCoroutine(_writingCoroutine);
                _writingCoroutine = null;
            }
            
            HideUI();
            OnDialogueDisplayEnd?.Invoke();
        }
        
        #region Show/Hide

        private void ShowUI()
        {
            _parent.SetActive(true);
        }

        private void HideUI()
        {
            _parent.SetActive(false);
        }

        #endregion
    }
}
