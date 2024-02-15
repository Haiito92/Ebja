using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimatorBinding : MonoBehaviour
{
    //Animator Params
    [SerializeField, AnimatorParam(nameof(_anim), AnimatorControllerParameterType.Bool)] string _isWalkingParam;
    [SerializeField, AnimatorParam(nameof(_anim), AnimatorControllerParameterType.Float)] string _isFacingRightParam;

    [SerializeField, AnimatorParam(nameof(_anim), AnimatorControllerParameterType.Bool)] string _isGroudedParam;
    [SerializeField, AnimatorParam(nameof(_anim), AnimatorControllerParameterType.Bool)] string _isJumpingParam;
    [SerializeField, AnimatorParam(nameof(_anim), AnimatorControllerParameterType.Float)] string _verticalSpeedParam;

    //Refs to components
    [Header("Refs to components")]
    [Space]
    [SerializeField] Animator _anim;
    [Space]
    [SerializeField] PlayerMovement _playerMovement;

    private void Reset()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _anim.SetBool(_isGroudedParam, _playerMovement.IsGrounded);
        if (!_playerMovement.IsGrounded)
        {
            _anim.SetFloat(_verticalSpeedParam, _playerMovement.VerticalSpeed);
        }
    }

    void SetIsWalkingParam() => _anim.SetBool(_isWalkingParam, _playerMovement.IsWalking);

    void SetIsFacingRightParam(bool isFacingRight)
    {
        if (!isFacingRight) _anim.SetFloat(_isFacingRightParam, 0);
        else _anim.SetFloat(_isFacingRightParam, 1);
    }

    void SetIsJumpingParam(bool isJumping) => _anim.SetBool(_isJumpingParam, isJumping);

    private void OnEnable()
    {
        _playerMovement.OnStartWalking += SetIsWalkingParam;
        _playerMovement.OnStopWalking += SetIsWalkingParam;

        _playerMovement.OnFacingDirectionChanged += SetIsFacingRightParam;

        _playerMovement.OnJumpingCheck += SetIsJumpingParam;
    }

    private void OnDisable()
    {
        _playerMovement.OnStartWalking -= SetIsWalkingParam;
        _playerMovement.OnStopWalking -= SetIsWalkingParam;

        _playerMovement.OnFacingDirectionChanged -= SetIsFacingRightParam;

        _playerMovement.OnJumpingCheck -= SetIsJumpingParam;
    }
}
