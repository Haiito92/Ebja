using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    abstract void Interact();
    abstract void ShowUI();
    abstract void HideUI();
}
