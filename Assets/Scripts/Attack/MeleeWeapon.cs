using System;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private MeleeWeaponAnimationEventSender _eventSender;
    [SerializeField] private LayerMask _attackTargets;
    [SerializeField] private Collider _collider;

    private HashSet<Hitbox> _hitThisSwing = new();

    public event Action AttackCulmination;
    public event Action DamageDid;

    private void Awake()
    {
        _collider.enabled = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _eventSender.AttackHitEnable += EnableDamage;
        _eventSender.AttackHitDisable += DisableDamage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _eventSender.AttackHitEnable -= EnableDamage;
        _eventSender.AttackHitDisable -= DisableDamage;
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
            DamageDid?.Invoke();

            Vector3 hitPoint = other.ClosestPoint(_collider.transform.position);
            Vector3 hitNormal = (hitPoint - other.bounds.center).normalized;

            hitbox.ApplyDamage(Damage, hitPoint, hitNormal);
        }
    }

    private void EnableDamage()
    {
        _hitThisSwing.Clear();
        _collider.enabled = true;
        AttackCulmination?.Invoke();
    }

    private void DisableDamage()
    {
        _collider.enabled = false;
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}
