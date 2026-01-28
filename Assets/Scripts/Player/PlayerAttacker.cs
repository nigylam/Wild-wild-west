using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private FireWeapon _fireWeapon;
    [SerializeField] private MeleeWeapon _meleeWeapon;
    [SerializeField] private MeleeWeaponAnimationEventSender _meleeWeaponAnimationEventSender;
    [SerializeField] private MeleeWeaponSound _meleeWeaponSound;

    private Weapon _activeWeapon;
    private ThirdPersonActions _actions;
    private bool _canSwitchWeapon = true;

    public event Action Attack;
    public event Action MeleeWeaponChosen;
    public event Action FireWeaponChosen;

    private void OnEnable()
    {
        _meleeWeapon.AttackStarted += OnAttackStarted;
        _meleeWeapon.AttackEnded += OnAttackEnded;
        _meleeWeapon.AttackCulmination += _meleeWeaponSound.PlayAttackSound;
        _meleeWeapon.DamageDid += _meleeWeaponSound.PlayDamageSound;
        _fireWeapon.AttackStarted += OnAttackStarted;
        _fireWeapon.ShotEnded += OnAttackEnded;
    }


    private void OnDisable()
    {
        _actions.Player.Attack.started -= OnAttack;
        _actions.Player.Changeweapon.started -= OnChangeWeapon;
        _meleeWeapon.AttackCulmination -= _meleeWeaponSound.PlayAttackSound;
        _meleeWeapon.DamageDid -= _meleeWeaponSound.PlayDamageSound; 
        _fireWeapon.AttackStarted -= OnAttackStarted;
        _fireWeapon.ShotEnded -= OnAttackEnded;
    }

    public void Initialize(ThirdPersonActions actions, Camera camera)
    {
        _fireWeapon.Initialize(camera);
        _activeWeapon = _fireWeapon;
        _meleeWeapon.gameObject.SetActive(false);
        _actions = actions;
        _actions.Player.Attack.started += OnAttack;
        _actions.Player.Changeweapon.started += OnChangeWeapon;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (_activeWeapon.TryAttack())
            Attack?.Invoke();
    }

    private void OnAttackStarted()
    {
        _canSwitchWeapon = false;
    }

    private void OnAttackEnded()
    {
        _canSwitchWeapon = true;
    }

    private void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (_canSwitchWeapon == false)
            return;

        if (_activeWeapon == _fireWeapon)
        {
            SwitchWeapon(_meleeWeapon);
            MeleeWeaponChosen?.Invoke();
        }
        else
        {
            SwitchWeapon(_fireWeapon);
            FireWeaponChosen?.Invoke();
        }
    }

    private void SwitchWeapon(Weapon weapon)
    {
        _activeWeapon.gameObject.SetActive(false);
        _activeWeapon = weapon;
        _activeWeapon.gameObject.SetActive(true);
    }
}
