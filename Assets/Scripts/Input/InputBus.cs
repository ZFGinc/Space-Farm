using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlScheme
{
    KeyboardMouse, Gamepad
}

[RequireComponent(typeof(IControllable))]
public class InputBus : MonoBehaviour
{
    private MainInputMap _mainInputMap;
    private ActionController _actionController;
    private IControllable _controllable;

    private ControlScheme _currentInputDevice = ControlScheme.KeyboardMouse;

    private void Start()
    {
        Initialize();
        SubscripbeInputs();
    }

    private void Update()
    {
        ReadMovement();
        ReadRotation();

        CheckInputDevice();
    }

    private void Initialize()
    {
        _mainInputMap = new MainInputMap();
        _mainInputMap.Enable();

        _controllable = GetComponent<IControllable>();
        _actionController = GetComponent<ActionController>();

        if (_controllable == null)
        {
            throw new Exception("��� �����-�� ����� ����������");
        }
    }

    private void OnDisable()
    {
        UnscripbeInputs();
    }

    private void SubscripbeInputs()
    {
        //Moving
        _mainInputMap.Player.Jump.performed += OnJumpPerformed;
        _mainInputMap.Player.Sprint.performed += OnSprintPerformed;
        _mainInputMap.Player.Sprint.canceled += OnSprintCanceled;
        //

        //Actions
        _mainInputMap.Actions.GrabOrReleaseObject.performed += OnGrabOrReleaseObjectPerformed;    
        _mainInputMap.Actions.Usage.performed += OnUsagePerformed;
        _mainInputMap.Actions.Shooting.performed += OnShootingPerformed;
        _mainInputMap.Actions.Shooting.canceled += OnShootingCanceled;
        //

        //Inventory

        //

        //Building

        //
    }

    private void UnscripbeInputs()
    {
        //Moving
        _mainInputMap.Player.Jump.performed -= OnJumpPerformed;
        _mainInputMap.Player.Sprint.performed -= OnSprintPerformed;
        _mainInputMap.Player.Sprint.canceled -= OnSprintCanceled;
        //

        //Actions
        _mainInputMap.Actions.GrabOrReleaseObject.performed -= OnGrabOrReleaseObjectPerformed;
        _mainInputMap.Actions.Usage.performed -= OnUsagePerformed;
        _mainInputMap.Actions.Shooting.performed -= OnShootingPerformed;
        _mainInputMap.Actions.Shooting.canceled -= OnShootingCanceled;
        //

        //Inventory

        //

        //Building

        //
    }

    private void CheckInputDevice()
    {
        InputSystem.onEvent +=
           (eventPtr, device) =>
           {
               if (device is Keyboard || device is Mouse)
               {
                   SwitchControlScheme(ControlScheme.KeyboardMouse);
               }
               else
               {
                   SwitchControlScheme(ControlScheme.Gamepad);
               }
           };
    }

    private void SwitchControlScheme(ControlScheme scheme)
    {
        if(_currentInputDevice == scheme) return;

        _currentInputDevice = scheme;
        _controllable.SwitchControlScheme(_currentInputDevice);
    }

    private void ReadMovement()
    {
        Vector2 inputDirection = _mainInputMap.Player.Movement.ReadValue<Vector2>();
        Vector3 direction = new Vector3(inputDirection.x, 0, inputDirection.y);

        _controllable.Move(direction);
    }

    private void ReadRotation()
    {
        Vector2 inputDirection = _mainInputMap.Player.Rotation.ReadValue<Vector2>();
        Vector3 direction = new Vector3(inputDirection.x, 0, inputDirection.y);

        _controllable.CursorMove(direction);
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        _controllable.Jump();
    }

    private void OnSprintPerformed(InputAction.CallbackContext obj)
    {
        _controllable.Sprint(true);
    }

    private void OnSprintCanceled(InputAction.CallbackContext obj)
    {
        _controllable.Sprint(false);
    }

    private void OnGrabOrReleaseObjectPerformed(InputAction.CallbackContext obj)
    {
        _actionController.GrabOrRelese();
    }

    private void OnUsagePerformed(InputAction.CallbackContext obj)
    {
        _actionController.Usage();
    }

    private void OnShootingPerformed(InputAction.CallbackContext obj)
    {
        _actionController.Shooting(true);
    }

    private void OnShootingCanceled(InputAction.CallbackContext obj)
    {
        _actionController.Shooting(false);
    }
}
