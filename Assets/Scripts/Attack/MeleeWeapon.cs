using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class MeleeWeapon : Weapon
{
    private readonly int AttackTrigger = Animator.StringToHash("Attack");

    [SerializeField] private MeleeWeaponAnimationEventSender _eventSender;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _attackTargets;

    private BoxCollider _collider;
    private HashSet<Hitbox> _hitThisSwing = new();

    public event Action AttackStarted;
    public event Action AttackEnded;

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _eventSender.AttackHitEnable += EnableDamage;
        _eventSender.AttackHitDisable += DisableDamage;
        _eventSender.AttackStarted += OnAttackStarted;
        _eventSender.AttackEnded += OnAttackEnded;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _eventSender.AttackHitEnable -= EnableDamage;
        _eventSender.AttackHitDisable -= DisableDamage;
        _eventSender.AttackStarted -= OnAttackStarted;
        _eventSender.AttackEnded -= OnAttackEnded;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collider.enabled == false) 
            return;

        if (IsInLayerMask(other.gameObject, _attackTargets) == false)
            return;

        if (other.TryGetComponent(out Hitbox hitbox))
        {
            if (_hitThisSwing.Contains(hitbox)) 
                return;

            _hitThisSwing.Add(hitbox);
            hitbox.ApplyDamage(Damage, default);
        }
    }

    protected override void Attack()
    {
        if (CanAttack == false)
            return;

        base.Attack();
        _animator.SetTrigger(AttackTrigger);
    }

    private void EnableDamage()
    {
        _hitThisSwing.Clear();
        _collider.enabled = true;
    }

    private void DisableDamage()
    {
        _collider.enabled = false;
    }

    private void OnAttackStarted()
    {
        AttackStarted?.Invoke();
    }

    private void OnAttackEnded()
    {
        AttackEnded?.Invoke();
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}
