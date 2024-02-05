using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
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
    [SerializeField] float _accelerationTime;
    [SerializeField] float _decelerationTime;
    bool _isFacingRight;
    public Vector2 HorizontalInput => _walk.action.ReadValue<Vector2>();
    public bool IsWalking => HorizontalInput.x != 0;

    [Header("Jump Fields")]
    [Space]
    //Jump
    [SerializeField] float _jumpForce;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckRadius;
    bool _isGrounded;
    [SerializeField] LayerMask _whatIsGround;

    [Header("Components References")]
    [Space]
    //References//
    [SerializeField] Rigidbody2D _rb;

    //InputReferences//
    [Header("InputAction References")]
    [Space]
    [SerializeField] InputActionReference _walk;
    [SerializeField] InputActionReference _jump;

    //Events//
    [Header("Events")]
    [Space]
    public UnityEvent OnStartWalkingEvent;
    public UnityEvent OnStopWalkingEvent;

    //Actions//
    public event Action OnStartWalking;
    public event Action OnStopWalking;

    public event Action<bool> OnFacingDirectionChanged;

    private void Reset()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        OnStartWalkingEvent.AddListener(() => OnStartWalking?.Invoke());
        OnStopWalkingEvent.AddListener(() => OnStopWalking?.Invoke());
    }

    private void FixedUpdate()
    {
        Walk();

        FaceWalkingDirection();
    }

    void StartWalking(InputAction.CallbackContext ctx) => OnStartWalkingEvent?.Invoke();
    void StopWalking(InputAction.CallbackContext ctx) => OnStopWalkingEvent?.Invoke();

    void Walk()
    {
        if (IsWalking)
        {
            float acceleration = (_speed / _accelerationTime) * Mathf.Sign(HorizontalInput.x);

            Vector2 accelerationForce;
            if ((HorizontalInput.x > 0 && _rb.velocity.x >= _speed) || (HorizontalInput.x < 0) && _rb.velocity.x <= -_speed) accelerationForce = Vector2.zero;
            else accelerationForce = acceleration * Vector2.right;

            _rb.AddForce(accelerationForce);

            return;
        }

        if (Mathf.Abs(_rb.velocity.x) < 0.03f) return;
 
        float deceleration = (_speed / _decelerationTime) * Mathf.Sign(_rb.velocity.x) * -1;
        Vector2 decelerationForce = deceleration * Vector2.right;
        _rb.AddForce(decelerationForce);
    }  

    void FaceWalkingDirection()
    {
        if (_isFacingRight && HorizontalInput.x < 0)
        {
            Flip();
        }
        else if (!_isFacingRight && HorizontalInput.x > 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        transform.Rotate(new Vector3(0, 180 , 0));
        _isFacingRight = !_isFacingRight;
        OnFacingDirectionChanged?.Invoke(_isFacingRight);
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _whatIsGround);

        if(_isGrounded)
        {
            _rb.AddForce(new Vector2(0, _jumpForce));
        }
    }

    private void OnEnable()
    {
        _walk.action.started += StartWalking;
        _walk.action.canceled += StopWalking;
        _jump.action.started += Jump;
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
        if(_groundCheckRadius > 0) Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}