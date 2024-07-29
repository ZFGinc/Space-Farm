using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private string _nameSprint;
    [SerializeField] private string _nameMove;
    [SerializeField] private string _nameGrounded;
    [SerializeField] private string _nameJump;

    private Animator _animator;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    public void Subsribe(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;

        _playerMovement.IsMove += IsMove;
        _playerMovement.IsSprint += IsSprint;
        _playerMovement.IsGround += IsGround;
        _playerMovement.IsJump += IsJump;
    }

    private void OnDisable()
    {
        _playerMovement.IsMove -= IsMove;
        _playerMovement.IsSprint -= IsSprint;
        _playerMovement.IsGround -= IsGround;
        _playerMovement.IsJump -= IsJump;
    }

    private void IsMove(bool value)
    {
        _animator.SetBool(_nameMove, value);
    }

    private void IsSprint(bool value)
    {
        _animator.SetBool(_nameSprint, value);
    }

    private void IsGround(bool value)
    {
        _animator.SetBool(_nameGrounded, value);
    }

    private void IsJump()
    {
        _animator.SetTrigger(_nameJump);
    }
}
