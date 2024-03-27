using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Runtime.Scripts.Global.Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
    
        #region Singleton
        private static InputManager _instance;
        public static InputManager Instance => _instance;

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
            //Please change that, don't use lambda functions
            DialogueManager.Instance.OnDialogueStarted += () => ChangeKeymap("Empty");
            DialogueManager.Instance.OnDialogueEnded += () => ChangeKeymap("AdventureControls");
        }

        private void ChangeKeymap(string name)
        {
            _playerInput.SwitchCurrentActionMap(name);
        }

        
    }
}
