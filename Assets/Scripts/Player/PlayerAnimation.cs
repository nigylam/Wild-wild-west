using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    private readonly int AnimatorMoveForward = Animator.StringToHash("MoveForward");
    private readonly int AnimatorMoveRight = Animator.StringToHash("MoveRight");
    private readonly int AnimatorIsMoving = Animator.StringToHash("IsMoving");
    private readonly int AnimatorFire = Animator.StringToHash("Fire");
    private readonly int AnimatorJump = Animator.StringToHash("Jump");
    private readonly int AnimatorChangeWeapon = Animator.StringToHash("ChangeWeapon");

    [SerializeField] private Animator _animator;

    private ThirdPersonActions _actions;
    private InputAction _moveAction;

    private void Update()
    {
        _animator.SetFloat(AnimatorMoveForward, _moveAction.ReadValue<Vector2>().y);
        _animator.SetFloat(AnimatorMoveRight, _moveAction.ReadValue<Vector2>().x);
        _animator.SetBool(AnimatorIsMoving, _moveAction.ReadValue<Vector2>() != new Vector2(0, 0));
    }

    private void OnDisable()
    {
        _actions.Player.Attack.started -= OnFire;
        _actions.Player.Jump.started -= OnJump;
        _actions.Player.Changeweapon.performed -= OnChangeWeapon;
    }

    public void Initialize(ThirdPersonActions actions)
    {
        _actions = actions;
        _moveAction = _actions.Player.Move;
        _actions.Player.Attack.started += OnFire;
        _actions.Player.Jump.started += OnJump;
        _actions.Player.Changeweapon.performed += OnChangeWeapon;
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        _animator.SetTrigger(AnimatorFire);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _animator.SetTrigger(AnimatorJump);
    }

    private void OnChangeWeapon(InputAction.CallbackContext context)
    {
        _animator.SetTrigger(AnimatorChangeWeapon);
    }
}
