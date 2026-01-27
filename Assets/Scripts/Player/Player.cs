using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerMover))]
public class Player : MonoBehaviour
{
    [Header("Parametres")]
    [SerializeField] private float _movementForce = 1f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _maxSpeed = 10f;

    [Header("Links")]
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraRotator _cameraRotator;
    [SerializeField] private Bar _healthBar;

    private ThirdPersonActions _actions;
    private PlayerMover _mover;
    private Health _health;
    private PlayerAnimator _animator;
    private PlayerAttacker _attacker;
    private StepSound _sound;
    private Vector3 _startPosition;

    public event Action Dead;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _actions = new ThirdPersonActions();
        _mover = GetComponent<PlayerMover>();
        _animator = GetComponent<PlayerAnimator>();
        _attacker = GetComponent<PlayerAttacker>();
        _sound = GetComponent<StepSound>();

        _attacker.Initialize(_actions, _camera);
        _mover.Initialize(_camera, _cameraRotator, _actions, _movementForce, _jumpForce, _maxSpeed);
        _animator.Initialize(_actions);
        _healthBar.Initialize(_health);
    }

    private void OnEnable()
    {
        _health.Dead += OnDead;
        _attacker.Attack += _animator.OnAttack;
        _attacker.MeleeWeaponChosen += _animator.OnMeleeWeaponChosen;
        _attacker.FireWeaponChosen += _animator.OnFireWeaponChosen;
        _mover.Jumped += _animator.OnJump;
        _mover.Jumped += _sound.OnJumpStarted;
        _mover.Landed += _sound.OnLanded;
        _mover.Landed += _animator.OnLanded;
    }

    private void Start()
    {
        _startPosition = transform.position;
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
        _mover.Landed -= _animator.OnLanded;
    }

    public void Restart()
    {
        _mover.enabled = true;
        _cameraRotator.enabled = true;
        _health.Restart();
        _healthBar.Initialize(_health);
        transform.position = _startPosition;
    }

    public void DisableControl()
    {
        _mover.enabled = false;
        _cameraRotator.enabled = false;
    }

    private void OnDead()
    {
        Dead?.Invoke();
    }
}