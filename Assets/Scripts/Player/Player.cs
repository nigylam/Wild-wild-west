using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerAttacker))]
[RequireComponent(typeof(StepSound))]
[RequireComponent(typeof(Hitbox))]
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraRotator _cameraRotator;

    private PlayerInputReader _inputReader;
    private PlayerMover _mover;
    private Health _health;
    private PlayerAnimator _animator;
    private PlayerAttacker _attacker;
    private StepSound _sound;
    private Hitbox _hitbox;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    public event Action Dead;

    private void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    private void OnDisable()
    {
        _health.Dead -= OnDead;
        _attacker.Attack -= _animator.OnAttack;
        _attacker.MeleeWeaponChosen -= _animator.OnMeleeWeaponChosen;
        _attacker.FireWeaponChosen -= _animator.OnFireWeaponChosen;
        _mover.Jumped -= _animator.OnJump;
        _mover.Jumped -= _sound.OnJumpStarted;
        _mover.Landed -= _sound.OnLanded;
    }

    private void FixedUpdate()
    {
        _mover.Move(_inputReader.Input);
        _animator.ProcessMovingAnimations(_inputReader.Input);
    }

    public void Initialize(ThirdPersonActions inputActions)
    {
        _inputReader = GetComponent<PlayerInputReader>();
        _mover = GetComponent<PlayerMover>();
        _health = GetComponent<Health>();
        _animator = GetComponent<PlayerAnimator>();
        _attacker = GetComponent<PlayerAttacker>();
        _sound = GetComponent<StepSound>();
        _hitbox = GetComponent<Hitbox>();

        _inputReader.Initialize(inputActions);
        _mover.Initialize(_camera, _cameraRotator);
        _attacker.Initialize(_camera);
        _hitbox.Initialize(this);

        _inputReader.JumpPressed += _mover.Jump;
        _inputReader.AttackPressed += _attacker.OnAttack;
        _inputReader.ChangeWeaponPressed += _attacker.OnChangeWeapon;
        _health.Dead += OnDead;
        _attacker.Attack += _animator.OnAttack;
        _attacker.MeleeWeaponChosen += _animator.OnMeleeWeaponChosen;
        _attacker.FireWeaponChosen += _animator.OnFireWeaponChosen;
        _mover.Jumped += _animator.OnJump;
        _mover.Jumped += _sound.OnJumpStarted;
        _mover.Landed += _sound.OnLanded;
    }

    public void Restart()
    {
        _attacker.Restart();
        _health.Restart();
        _animator.Restart();
        _cameraRotator.Restart(_startRotation);
        _mover.Restart();
        transform.SetPositionAndRotation(_startPosition, Quaternion.Euler(0f, _startRotation.eulerAngles.y, 0f));
    }

    public void Enable()
    {
        _mover.enabled = true;
        _cameraRotator.enabled = true;
    }

    public void Disable()
    {
        _mover.enabled = false;
        _cameraRotator.enabled = false;
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _health.TakeDamage(damage);
        _animator.OnHit(hitPoint, hitNormal);
    }

    private void OnDead()
    {
        Dead?.Invoke();
        _animator.OnDeath();
    }
}