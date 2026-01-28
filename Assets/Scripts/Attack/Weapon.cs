using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float Damage;
    [SerializeField] private float _cooldownTime;

    private Coroutine _cooldown;
    protected bool CanAttack = true;

    public event Action AttackStarted;
    public event Action AttackEnded;

    protected virtual void OnEnable()
    {
        CanAttack = true;
    }

    protected virtual void OnDisable()
    {
        CanAttack = true;

        if (_cooldown != null)
            StopCoroutine(_cooldown);
    }

    public bool TryAttack()
    {
        if(CanAttack)
        {
            Attack();
            return true;
        }

        return false;
    }

    protected virtual void Attack()
    {
        AttackStarted?.Invoke();
        StartCooldown();
    }

    private void StartCooldown()
    {
        CanAttack = false;

        if (_cooldown != null)
            StopCoroutine(_cooldown);

        _cooldown = StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        float time = 0;

        while (time < _cooldownTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        CanAttack = true;
        AttackEnded?.Invoke();
    }
}
