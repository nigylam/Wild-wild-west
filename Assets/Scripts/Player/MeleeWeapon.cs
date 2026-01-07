using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class MeleeWeapon : Weapon
{
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");

    [SerializeField] private Animator _animator;

    private BoxCollider _collider;
    private HashSet<Hitbox> _hitThisSwing = new();

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    public override void Attack()
    {
        _animator.SetTrigger(AttackTrigger);
    }

    public void EnableDamage()
    {
        _hitThisSwing.Clear();
        _collider.enabled = true;
    }

    public void DisableDamage()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collider.enabled == false) 
            return;

        if (other.TryGetComponent(out Hitbox hitbox))
        {
            if (_hitThisSwing.Contains(hitbox)) 
                return;

            _hitThisSwing.Add(hitbox);
            hitbox.ApplyDamage(Damage, default);
        }
    }

}
