using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IControllable
{
    public event Action<bool> LockMove, IsMove, IsSprint, IsGround;
    public event Action IsJump;

    [SerializeField, Range(1, 10)] private float _speedMovement = 5f;
    [SerializeField, Range(1, 10)] private float _sprintMultiplayer = 1.5f;
    [SerializeField, Range(1, 5)] private float _jumpHeight = 3f;
    [Space]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheckerPivot;
    [Space]
    [SerializeField] private PlayerAnimator _playerAnimator;

    private CharacterController _characterController;
    private ControlScheme _currentControlScheme;
    private Vector3 _moveDirection;
    private Vector3 _cursorDirection;
    private float _velocity;
    private bool _isGrounded;
    private bool _isSprint;
    private bool _isLockMove = false;

    private float CurrentSpeed => _speedMovement * (_isSprint ? _sprintMultiplayer : 1);

    private const float Gravity = -30f;
    private const float RadiusChecker = 0.4f;
    private const float SpeedRotation = 500f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _playerAnimator.SubsribeMoving(this);
    }

    private void FixedUpdate()
    {
        _isGrounded = IsOnTheGround();
        IsGround?.Invoke(_isGrounded);

        MoveInternal();
        RotateInternal();
        GravityInternal();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheckerPivot.position, RadiusChecker);
    }

    private void RotateInternal()
    {
        Quaternion? rotationDirection = null;
        if (_currentControlScheme == ControlScheme.KeyboardMouse)
            rotationDirection = RotateWithMouse();
        else
            rotationDirection = RotateWithStick();

        if (rotationDirection == null || _isLockMove) return;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDirection.Value, Time.fixedDeltaTime * SpeedRotation);
    }

    private Quaternion? RotateWithMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100);

        Vector3 lookDiraction = Vector3.zero;

        if (hit.point == Vector3.zero)
        {
            lookDiraction = _moveDirection;

            if (_moveDirection.sqrMagnitude == 0) return null;
        }
        else
        {
            lookDiraction.x = hit.point.x - transform.position.x;
            lookDiraction.z = hit.point.z - transform.position.z;
        }

        return Quaternion.LookRotation(lookDiraction, Vector3.up);
    }

    private Quaternion? RotateWithStick()
    {
        if (_cursorDirection.sqrMagnitude == 0)
        {
            if (_moveDirection.sqrMagnitude == 0)
            {
                return null;
            }
            return Quaternion.LookRotation(_moveDirection, Vector3.up);
        }
        return Quaternion.LookRotation(_cursorDirection, Vector3.up);
    }

    private void MoveInternal()
    {
        if (_moveDirection.sqrMagnitude == 0 || _isLockMove)
        {
            IsMove?.Invoke(false);
            return;
        }

        IsMove?.Invoke(true);
        _characterController.Move(_moveDirection * CurrentSpeed * Time.fixedDeltaTime);

    }

    private void GravityInternal()
    {
        if (!_isGrounded) _velocity += Gravity * Time.fixedDeltaTime;

        _characterController.Move(Vector3.up * _velocity * Time.fixedDeltaTime);
    }

    private bool IsOnTheGround()
    {
        return Physics.CheckSphere(_groundCheckerPivot.position, RadiusChecker, _groundLayer);
    }

    private IEnumerator LockMoveByTime(float time)
    {
        _isLockMove = true;
        LockMove?.Invoke(true);

        yield return new WaitForSeconds(time);

        _isLockMove = false;
        LockMove?.Invoke(false);
    }

    public void SwitchControlScheme(ControlScheme scheme)
    {
        _currentControlScheme = scheme;
    }

    public void Move(Vector3 direction)
    {
        _moveDirection = direction;
    }

    public void Jump()
    {
        if (!_isGrounded) return;

        IsJump?.Invoke();
        _velocity = Mathf.Sqrt(_jumpHeight * -2 * Gravity);
    }

    public void Sprint(bool sprint)
    {
        _isSprint = sprint;
        IsSprint?.Invoke(_isSprint);
    }

    public void CursorMove(Vector3 direction)
    {
        _cursorDirection = direction;
    }

    public void LockMoving(float time)
    {
        StartCoroutine(LockMoveByTime(time));
    }
}
