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
    private readonly int AnimatorActiveWeaponType = Animator.StringToHash("ActiveWeaponType");
    private readonly int AnimatorHit = Animator.StringToHash("Hit");
    private readonly int AnimatorDeath = Animator.StringToHash("Death");
    private readonly int AnimatorRestart = Animator.StringToHash("Restart");
    private readonly float RigMaxWeight = 1;

    [SerializeField] private Animator _animator;
    [SerializeField] private Rig _rig;

    private InputAction _moveAction;
    private int _gunLayer;
    private int _meleeLayer;

    void Awake()
    {
        _gunLayer = _animator.GetLayerIndex("UpperBody_Gun");
        _meleeLayer = _animator.GetLayerIndex("UpperBody_Melee");
    }

    private void Update()
    {
        _animator.SetFloat(AnimatorMoveForward, _moveAction.ReadValue<Vector2>().y);
        _animator.SetFloat(AnimatorMoveRight, _moveAction.ReadValue<Vector2>().x);
        _animator.SetBool(AnimatorIsMoving, _moveAction.ReadValue<Vector2>() != Vector2.zero);
    }

    public void Initialize(ThirdPersonActions actions)
    {
        _moveAction = actions.Player.Move;
    }

    public void Restart()
    {
        _animator.SetTrigger(AnimatorRestart);
    }

    public void OnAttack()
    {
        _animator.SetTrigger(AnimatorFire);
    }

    public void OnMeleeWeaponChosen()
    {
        _animator.SetLayerWeight(_gunLayer, 0);
        _animator.SetLayerWeight(_meleeLayer, 1);
        _animator.SetInteger(AnimatorActiveWeaponType, (int)WeaponType.Sword);
        _rig.weight = 0;
    }

    public void OnFireWeaponChosen()
    {
        _animator.SetLayerWeight(_gunLayer, 1);
        _animator.SetLayerWeight(_meleeLayer, 0);
        _animator.SetInteger(AnimatorActiveWeaponType, (int)WeaponType.Gun);
        _rig.weight = RigMaxWeight;
    }

    public void OnJump()
    {
        _animator.SetTrigger(AnimatorJump);
    }

    public void OnHit(Vector3 _, Vector3 __)
    {
        _animator.SetTrigger(AnimatorHit);
    }

    public void OnDeath()
    {
        _animator.SetTrigger(AnimatorDeath);
    }
}