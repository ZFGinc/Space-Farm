using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public enum ControlScheme
{
    KeyboardMouse, Gamepad
}

[RequireComponent(typeof(IControllable))]
public class InputBus : MonoBehaviour
{
    private MainInputMap _mainInputMap;
    private IControllable _controllable;
    private ControlScheme _currentInputDevice = ControlScheme.KeyboardMouse;

    private void Start()
    {
        Initialize();
        SubscripbeInputActions();
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

        if (_controllable == null)
        {
            throw new Exception("Чет какая-то хуйня получается");
        }
    }

    private void OnDestroy()
    {
        UnscripbeInputActions();
    }

    private void OnDisable()
    {
        UnscripbeInputActions();
    }

    private void SubscripbeInputActions()
    {
        _mainInputMap.Player.Jump.performed += OnJumpPerformed;
        _mainInputMap.Player.Sprint.performed += OnSprintPerformed;
        _mainInputMap.Player.Sprint.canceled += OnSprintCanceled;
    }

    private void UnscripbeInputActions()
    {
        _mainInputMap.Player.Jump.performed -= OnJumpPerformed;
        _mainInputMap.Player.Sprint.performed -= OnSprintPerformed;
        _mainInputMap.Player.Sprint.canceled -= OnSprintCanceled;
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
}
