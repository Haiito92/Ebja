using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimatorBinding : MonoBehaviour
{

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

    void SetIsWalkingParam() => _anim.SetBool("IsWalking", _playerMovement.IsWalking);

    void SetIsFacingRightParam(bool isFacingRight)
    {
        if (!isFacingRight) _anim.SetFloat("IsFacingRight", 0);
        else _anim.SetFloat("IsFacingRight", 1);
    }

    private void OnEnable()
    {
        _playerMovement.OnStartWalking += SetIsWalkingParam;
        _playerMovement.OnStopWalking += SetIsWalkingParam;

        _playerMovement.OnFacingDirectionChanged += SetIsFacingRightParam;
    }

    private void OnDisable()
    {
        _playerMovement.OnStartWalking -= SetIsWalkingParam;
        _playerMovement.OnStopWalking -= SetIsWalkingParam;

        _playerMovement.OnFacingDirectionChanged -= SetIsFacingRightParam;
    }
}
