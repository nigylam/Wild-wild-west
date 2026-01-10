using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private FireWeapon _fireWeapon;
    [SerializeField] private MeleeWeapon _meleeWeapon;
    [SerializeField] private CameraRotator _cameraRotator;

    private ThirdPersonActions _actions;
    private PlayerMover _mover;
    private Weapon _activeWeapon;
    private Health _health;
    private Vector3 _startPosition;

    public event Action Dead;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _actions = new ThirdPersonActions();
        _mover = GetComponent<PlayerMover>();
        _fireWeapon.Initialize(_camera);
        _mover.Initialize(_camera, _cameraRotator, _actions, _movementForce, _jumpForce, _maxSpeed);
        _activeWeapon = _fireWeapon;
        _meleeWeapon.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _actions.Player.Attack.started += OnAttack;
        _actions.Player.Changeweapon.started += OnChangeWeapon;
        _health.Dead += OnDead;
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnDisable()
    {
        _actions.Player.Attack.started -= OnAttack;
        _actions.Player.Changeweapon.started -= OnChangeWeapon;
        _health.Dead -= OnDead;
    }

    public void Restart()
    {
        _mover.enabled = true;
        _cameraRotator.enabled = true;
        _health.Restart();
        transform.position = _startPosition;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        _activeWeapon.Attack();
    }

    private void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (_activeWeapon == _fireWeapon)
            SwitchWeapon(_meleeWeapon);
        else
            SwitchWeapon(_fireWeapon);
    }

    private void SwitchWeapon(Weapon weapon)
    {
        _activeWeapon.gameObject.SetActive(false);
        _activeWeapon = weapon;
        _activeWeapon.gameObject.SetActive(true);
    }

    private void OnDead()
    {
        _mover.enabled = false;
        _cameraRotator.enabled = false;
        Dead?.Invoke();
    }
}