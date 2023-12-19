using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Fields
    [SerializeField] float _speed;

    //References
    Rigidbody2D _rb;

    //InputReferences
    [SerializeField] InputActionReference _walk;

    //Coroutines
    Coroutine _walking;

    //Events
    public UnityEvent OnStartWalkingEvent;
    public UnityEvent OnStopWalkingEvent;

    //Actions
    public Action OnStartWalking;
    public Action OnStopWalking;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _walk.action.started += StartWalking;
        _walk.action.canceled += StopWalking;

        OnStartWalkingEvent.AddListener(() => OnStartWalking?.Invoke());
    }

    void StartWalking(InputAction.CallbackContext ctx)
    {
        if (_walking == null)
        {
            _walking = StartCoroutine(Walking());
            OnStartWalkingEvent?.Invoke();
        }
    }

    IEnumerator Walking()
    {
        while (true)
        {
            Vector2 direction = _walk.action.ReadValue<Vector2>();
            _rb.velocity = new Vector2(direction.x, _rb.velocity.y)  * _speed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    void StopWalking(InputAction.CallbackContext ctx)
    {
        if(_walking != null)
        {
            StopCoroutine(_walking);
            _walking = null;
        }
        _rb.velocity = Vector2.zero;
        OnStopWalkingEvent?.Invoke();
    }

    private void OnDisable()
    {
        _walk.action.started -= StartWalking;
        _walk.action.canceled -= StopWalking;
    }
}
