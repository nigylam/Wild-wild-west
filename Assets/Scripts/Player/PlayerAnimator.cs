using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    private readonly int AnimatorMoveForward = Animator.StringToHash("MoveForward");
    private readonly int AnimatorMoveRight = Animator.StringToHash("MoveRight");
    private readonly int AnimatorIsMoving = Animator.StringToHash("IsMoving");
    private readonly int AnimatorFire = Animator.StringToHash("Attack");
    private readonly int AnimatorJump = Animator.StringToHash("Jump");
    private readonly int AnimatorChangeWeapon = Animator.StringToHash("ChangeWeapon");
    private readonly float RigMaxWeight = 1;

    [SerializeField] private Animator _animator;
    [SerializeField] private Rig _rig;

    private ThirdPersonActions _actions;
    private InputAction _moveAction;
    private bool _isMoving;

    private void Update()
    {
        _animator.SetFloat(AnimatorMoveForward, _moveAction.ReadValue<Vector2>().y);
        _animator.SetFloat(AnimatorMoveRight, _moveAction.ReadValue<Vector2>().x);
        SetMovingBool();
    }

    private void SetMovingBool()
    {
        bool isMoving = _moveAction.ReadValue<Vector2>() != Vector2.zero;

        if (_isMoving == isMoving)
            return;

        _isMoving = isMoving;
        _animator.SetBool(AnimatorIsMoving, _isMoving);
    }

    public void Initialize(ThirdPersonActions actions)
    {
        _actions = actions;
        _moveAction = _actions.Player.Move;
    }

    public void OnAttack()
    {
        _animator.SetTrigger(AnimatorFire);
    }

    public void OnMeleeWeaponChosen()
    {
        _animator.SetTrigger(AnimatorChangeWeapon);
        _rig.weight = 0;
    }

    public void OnFireWeaponChosen()
    {
        _animator.SetTrigger(AnimatorChangeWeapon);
        _rig.weight = RigMaxWeight;
    }

    public void OnJump()
    {
        _animator.SetTrigger(AnimatorJump);
    }
}
