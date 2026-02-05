using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private ThirdPersonActions _inputActions;
    private InputAction _moveAction;
    private bool _isInitialized = false;

    public Vector2 Input {  get; private set; }

    public event Action JumpPressed;
    public event Action AttackPressed;
    public event Action ChangeWeaponPressed;

    private void OnEnable()
    {
        if (_inputActions != null)
            _inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        _inputActions.Player.Jump.performed -= OnJump;
    }

    private void Update()
    {
        if (_isInitialized == false)
            return;

        Input = _moveAction.ReadValue<Vector2>();
    }

    public void Initialize(ThirdPersonActions inputActions) 
    {
        _inputActions = inputActions;
        _moveAction = _inputActions.Player.Move;
        _inputActions.Player.Jump.performed += OnJump;
        _inputActions.Player.Attack.performed += OnAttackPressed;
        _inputActions.Player.Changeweapon.performed += OnChangeWeaponPressed;
        _isInitialized = true;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        JumpPressed?.Invoke();
    }

    private void OnAttackPressed(InputAction.CallbackContext context)
    {
        AttackPressed?.Invoke();
    }

    private void OnChangeWeaponPressed(InputAction.CallbackContext context)
    {
        ChangeWeaponPressed?.Invoke();
    }
}
