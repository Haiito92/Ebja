using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Fields//
    [Header("Walk Fields")]
    [Space]
    //Walk
    [SerializeField] float _speed;
    bool _isFacingRight = true;

    [Header("Jump Fields")]
    [Space]
    //Jump
    [SerializeField] float _jumpForce;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckRadius;
    bool _isGrounded;
    [SerializeField] LayerMask _whatIsGround;

    //References//
    Rigidbody2D _rb;

    //InputReferences//
    [Header("InputAction References")]
    [Space]
    [SerializeField] InputActionReference _walk;
    [SerializeField] InputActionReference _jump;

    //Coroutines//
    Coroutine _walking;

    //Events//
    [Header("Events")]
    [Space]
    public UnityEvent OnStartWalkingEvent;
    public UnityEvent OnStopWalkingEvent;

    //Actions//
    public Action OnStartWalking;
    public Action OnStopWalking;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _walk.action.started += StartWalking;
        _walk.action.canceled += StopWalking;
        _jump.action.started += Jump;

        OnStartWalkingEvent.AddListener(() => OnStartWalking?.Invoke());
    }

    #region WalkingCoroutine
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
            _rb.velocity = new Vector2(direction.x * _speed * Time.fixedDeltaTime, _rb.velocity.y);

            if(_isFacingRight && direction.x < 0)
            {
                Flip();
            }
            else if(!_isFacingRight && direction.x > 0)
            {
                Flip();
            }

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
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        OnStopWalkingEvent?.Invoke();
    }
    #endregion

    void Flip()
    {
        transform.Rotate(new Vector3(0, 180 , 0));
        _isFacingRight = !_isFacingRight;
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _whatIsGround);

        if(_isGrounded)
        {
            _rb.AddForce(new Vector2(0, _jumpForce));
        }
    }

    private void OnDisable()
    {
        _walk.action.started -= StartWalking;
        _walk.action.canceled -= StopWalking;
        _jump.action.started -= Jump;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}