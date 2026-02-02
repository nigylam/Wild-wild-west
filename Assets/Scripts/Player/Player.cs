using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerMover))]
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraRotator _cameraRotator;
    [SerializeField] private Bar _healthBar;

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

    public void Initialize(ThirdPersonActions actions)
    {
        _health = GetComponent<Health>();
        _mover = GetComponent<PlayerMover>();
        _animator = GetComponent<PlayerAnimator>();
        _attacker = GetComponent<PlayerAttacker>();
        _sound = GetComponent<StepSound>();
        _hitbox = GetComponent<Hitbox>();

        _attacker.Initialize(actions, _camera);
        _mover.Initialize(_camera, _cameraRotator, actions);
        _animator.Initialize(actions);
        _healthBar.Initialize(_health);
        _hitbox.Initialize(this);

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
        _health.Restart();
        _animator.Restart();
        _healthBar.Initialize(_health);
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