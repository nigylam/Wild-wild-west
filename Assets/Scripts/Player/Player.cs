using UnityEngine;
using UnityEngine.InputSystem;

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

    private ThirdPersonActions _actions;
    private PlayerMover _mover;
    private Weapon _activeWeapon;

    private void Awake()
    {
        _actions = new ThirdPersonActions();
        _mover = GetComponent<PlayerMover>();
        _fireWeapon.Initialize(_camera);
        _mover.Initialize(_camera, _actions, _movementForce, _jumpForce, _maxSpeed);
        _activeWeapon = _fireWeapon;
        _meleeWeapon.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _actions.Player.Attack.started += OnAttack;
        _actions.Player.Changeweapon.started += OnChangeWeapon;
    }

    private void OnDisable()
    {
        _actions.Player.Attack.started -= OnAttack;
        _actions.Player.Changeweapon.started -= OnChangeWeapon;
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
}
