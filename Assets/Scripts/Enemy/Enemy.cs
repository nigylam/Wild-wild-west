using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyMover))]
public class Enemy : MonoBehaviour
{
    private Health _health;
    private EnemyMover _enemyMover;

    public event Action<Enemy> Dead;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.Dead += OnDead;
    }

    private void OnDisable()
    {
        _health.Dead -= OnDead;
    }

    public void Initialize(Transform target, Transform parrent, Vector3 position, Quaternion rotation)
    {
        _enemyMover = GetComponent<EnemyMover>();
        _enemyMover.Initialize(target);
        transform.SetParent(parrent);
        transform.position = position;
        transform.rotation = rotation;
    }

    private void OnDead()
    {
        Dead?.Invoke(this);
    }
}
