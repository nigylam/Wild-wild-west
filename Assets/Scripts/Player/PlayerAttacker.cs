using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private FireWeapon _fireWeapon;
    [SerializeField] private MeleeWeapon _meleeWeapon;
    [SerializeField] private MeleeWeaponAnimationEventSender _meleeWeaponAnimationEventSender;

    private Weapon _activeWeapon;
    private ThirdPersonActions _actions;
    private bool _canSwitchWeapon = true;

    public event Action Attack;
    public event Action ChangeWeapon;

    private void OnEnable()
    {
        _meleeWeapon.AttackStarted += OnMeleeAttackStarted;
        _meleeWeapon.AttackEnded += OnMeleeAttackEnded;
    }

    private void OnMeleeAttackStarted()
    {
        _canSwitchWeapon = false;
    }

    private void OnMeleeAttackEnded()
    {
        _canSwitchWeapon = true;
    }

    private void OnDisable()
    {
        _actions.Player.Attack.started -= OnAttack;
        _actions.Player.Changeweapon.started -= OnChangeWeapon;
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

    private void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (_canSwitchWeapon == false)
            return;

        if (_activeWeapon == _fireWeapon)
            SwitchWeapon(_meleeWeapon);
        else
            SwitchWeapon(_fireWeapon);

        ChangeWeapon?.Invoke();
    }

    private void SwitchWeapon(Weapon weapon)
    {
        _activeWeapon.gameObject.SetActive(false);
        _activeWeapon = weapon;
        _activeWeapon.gameObject.SetActive(true);
    }
}
