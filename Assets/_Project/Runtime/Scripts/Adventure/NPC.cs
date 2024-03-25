using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] string _name;

    [SerializeField] SpriteRenderer _interactionFX;

    public void Interact()
    {
        Debug.Log("Interact with : " + _name);
    }

    public void ShowUI()
    {
        Debug.Log("Show UI of : " + _name);
        _interactionFX.enabled = true;
    }

    public void HideUI()
    {
        Debug.Log("Hide UI of : " + _name);
        _interactionFX.enabled = false;
    }
}