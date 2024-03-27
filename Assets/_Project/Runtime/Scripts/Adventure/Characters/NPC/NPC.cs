using UnityEngine;

namespace _Project.Runtime.Scripts.Adventure.Characters.NPC
{
    using Interfaces;
    using Global.Managers;
    
    public class NPC : MonoBehaviour, IInteractable, ITalker
    {
        [SerializeField] string _name;

        [SerializeField] SpriteRenderer _interactionFX;

        [SerializeField] DSDialogue _dialogue;

        #region IInteractable
        public void Interact()
        {
            TriggerDialogue(_dialogue);
        }

        public void ShowInteractionFX()
        {
            _interactionFX.enabled = true;
        }

        public void HideInteractionFX()
        {
            _interactionFX.enabled = false;
        }
        #endregion

        #region ITalker
        public void TriggerDialogue(DSDialogue dialogue)
        {
            if (dialogue == null || dialogue.StartingDialogue == null)
            {
                Debug.LogError("No Dialogue to trigger");
                return;
            }

            DialogueManager.Instance.StartDialogue(dialogue);
        }
        #endregion
    }
}