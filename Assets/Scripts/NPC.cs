using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] string _name;

    public void Interact()
    {
        Debug.Log(_name);
    }
}
