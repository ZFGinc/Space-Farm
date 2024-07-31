using System;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private string _nameSprint;
    [SerializeField] private string _nameMove;
    [SerializeField] private string _nameGrounded;
    [SerializeField] private string _nameJump;
    [Space]
    [SerializeField] private string _nameUsingID;
    [SerializeField] private string _nameUsing;

    private Animator _animator;
    private PlayerMovement _playerMovement;
    private TileUsager _tileUsager;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SubsribeMoving(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;

        _playerMovement.IsMove += IsMove;
        _playerMovement.IsSprint += IsSprint;
        _playerMovement.IsGround += IsGround;
        _playerMovement.IsJump += IsJump;
    }

    public void SubsribeUsager(TileUsager tileUsager)
    {
        _tileUsager = tileUsager;

        _tileUsager.Using += IsUsing;
    }

    private void OnDisable()
    {
        _playerMovement.IsMove -= IsMove;
        _playerMovement.IsSprint -= IsSprint;
        _playerMovement.IsGround -= IsGround;
        _playerMovement.IsJump -= IsJump;

        _tileUsager.Using -= IsUsing;
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

    private void IsUsing(int id)
    {
        _animator.SetInteger(_nameUsingID, id);
        _animator.SetTrigger(_nameUsing);
    }
}
