using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private StepSound _stepSound;
    [SerializeField] private List<Hitbox> _hitboxes;

    private EffectSpawner _hitEffectSpawner;
    private Health _health;
    private EnemyMover _mover;
    private EnemyAnimator _animator;
    private EnemySound _sound;
    private NavMeshAgent _agent;

    public event Action<Enemy> Dead;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<EnemyAnimator>();
        _health = GetComponent<Health>();
        _mover = GetComponent<EnemyMover>();
        _sound = GetComponent<EnemySound>();
    }

    public void Initialize(Transform target, Transform parrent, Vector3 position, Quaternion rotation, EffectSpawner hitEffectSpawner)
    {
        _mover.Initialize(_agent, target);
        _hitEffectSpawner = hitEffectSpawner;
        transform.SetParent(parrent);
        transform.SetPositionAndRotation(position, rotation);

        _health.Dead += OnDead;
        _mover.Attack += _animator.OnAttack;
        _mover.Attack += _sound.OnAttack;
        _mover.Landed += _stepSound.OnLanded;
        _mover.Stop += _animator.OnStop;
        _mover.StartMoving += _animator.OnStartMoving;

        foreach (var hitbox in _hitboxes)
            hitbox.Initialize(this);
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _health.TakeDamage(damage);
        _animator.OnHit();
        _sound.OnHit();
        _hitEffectSpawner.Spawn(hitPoint, Quaternion.LookRotation(hitNormal), transform.parent);
    }

    private void OnDead()
    {
        _animator.OnDeath();
        _animator.DeathAnimationEnded += OnDeathAnimationEnded;

        _health.Dead -= OnDead;
        _mover.Attack -= _animator.OnAttack;
        _mover.Attack -= _sound.OnAttack;
        _mover.Landed -= _stepSound.OnLanded;
        _mover.Stop -= _animator.OnStop;
        _mover.StartMoving -= _animator.OnStartMoving;

        _mover.Disable();
    }

    private void OnDeathAnimationEnded()
    {
        _animator.DeathAnimationEnded -= OnDeathAnimationEnded;
        Dead?.Invoke(this);
    }
}
