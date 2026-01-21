using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : MonoBehaviour
{
    private Health _health;
    private EnemyMover _mover;
    private EnemyAnimator _animator;
    private NavMeshAgent _agent;

    public event Action<Enemy> Dead;

    public void Initialize(Transform target, Transform parrent, Vector3 position, Quaternion rotation)
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<EnemyAnimator>();
        _health = GetComponent<Health>();
        _mover = GetComponent<EnemyMover>();
        _mover.Initialize(_agent, target);
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

    private void OnHit(Vector3 hitPoint)
    {
        _animator.OnHit();
    }

    private void OnDeathAnimationEnded()
    {
        _animator.DeathAnimationEnded -= OnDeathAnimationEnded;
        Dead?.Invoke(this);
    }
}
