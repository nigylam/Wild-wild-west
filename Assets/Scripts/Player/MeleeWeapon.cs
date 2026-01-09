using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class MeleeWeapon : Weapon
{
    private readonly int AttackTrigger = Animator.StringToHash("Attack");

    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _attackTargets;

    private BoxCollider _collider;
    private HashSet<Hitbox> _hitThisSwing = new();

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
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

    public override void Attack()
    {
        if (CanAttack == false)
            return;

        base.Attack();
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

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}
