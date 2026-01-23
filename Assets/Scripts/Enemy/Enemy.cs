using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : MonoBehaviour
{
    private EffectSpawner _hitEffectSpawner;
    private Health _health;
    private EnemyMover _mover;
    private EnemyAnimator _animator;
    private NavMeshAgent _agent;

    public event Action<Enemy> Dead;

    public void Initialize(Transform target, Transform parrent, Vector3 position, Quaternion rotation, EffectSpawner hitEffectSpawner)
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<EnemyAnimator>();
        _health = GetComponent<Health>();
        _mover = GetComponent<EnemyMover>();
        _mover.Initialize(_agent, target);
        _hitEffectSpawner = hitEffectSpawner;
        transform.SetParent(parrent);
        transform.position = position;
        transform.rotation = rotation;
        _health.Dead += OnDead;
        _mover.Attack += _animator.OnAttack;
        _health.Hited += OnHit;
        _mover.Stop += _animator.OnStop;
        _mover.StartMoving += _animator.OnStartMoving;        
    }

    private void OnDead()
    {
        _animator.OnDeath();
        _animator.DeathAnimationEnded += OnDeathAnimationEnded;
        _health.Dead -= OnDead;
        _mover.Attack -= _animator.OnAttack;
        _health.Hited -= OnHit;
        _mover.Stop -= _animator.OnStop;
        _mover.StartMoving -= _animator.OnStartMoving;
        _mover.Disable();
    }

    private void OnHit(Vector3 hitPoint, Vector3 hitNormal)
    {
        _animator.OnHit();
        _hitEffectSpawner.Spawn(hitPoint, Quaternion.LookRotation(hitNormal), transform.parent);
    }

    private void OnDeathAnimationEnded()
    {
        _animator.DeathAnimationEnded -= OnDeathAnimationEnded;
        Dead?.Invoke(this);
    }
}
