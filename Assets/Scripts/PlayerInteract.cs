using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    //Fields//
    //Interaction
    [Header("Interaction Field")]
    [Space]
    [SerializeField] Transform _interactionCheck;
    [SerializeField] float _interactionRadius;

    //InputReferences//
    [Header("InputAction References")]
    [SerializeField] InputActionReference _interact;

    private void Awake()
    {
        _interact.action.started += Interact;
    }

    IInteractable GetClosestInteractable()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_interactionCheck.position, _interactionRadius);

        List<GameObject> inRangeGO = new List<GameObject>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable)) inRangeGO.Add(collider.gameObject);
        }

        IInteractable closestInteractable = null;
        if (inRangeGO.Count > 0)
        {
            float closestDistance = Vector2.Distance(_interactionCheck.position, inRangeGO[0].transform.position);
            closestInteractable = inRangeGO[0].GetComponent<IInteractable>();
            foreach (GameObject go in inRangeGO)
            {
                float distance = Vector2.Distance(_interactionCheck.position, go.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = go.GetComponent<IInteractable>();
                }
            }
        }

        return closestInteractable;
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        IInteractable interactable = GetClosestInteractable();
        if(interactable != null) interactable.Interact();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionCheck.position, _interactionRadius);
    }
}
