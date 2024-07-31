using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITileUsager))]
[RequireComponent(typeof(IControllable))]
//[RequireComponent(typeof(IInventory))]
//[RequireComponent(typeof(IShootingController))]
public class ActionController : MonoBehaviour
{
    [SerializeField] private ITileUsager _tileUsager;
    [SerializeField] private IControllable _controllable;

    private bool _isLockMove = false;
    private int _lastUsingId = 0;

    private void Awake()
    {
        _tileUsager = GetComponent<ITileUsager>();
        _controllable = GetComponent<IControllable>();
    }

    private void OnEnable()
    {
        _controllable.LockMove += OnLockMove;
    }

    private void OnDisable()
    {
        _controllable.LockMove -= OnLockMove;
    }

    private void OnLockMove(bool isMove)
    {
        _isLockMove = isMove;
    }

    public void GrabOrRelese()
    {

    }

    public void Usage()
    {
        if (_isLockMove) return;

        //����� ���� ���������� � ����������� �� �������� �������� � �������

        _controllable.LockMoving(2f); // ����� ����� ���� ����� ����������
        _tileUsager.Usage(_lastUsingId);// ����� ����������������� �����
    }

    public void Shooting(bool value)
    {

    }
}
