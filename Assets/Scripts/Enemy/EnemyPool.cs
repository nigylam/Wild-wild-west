using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;

    private ObjectPool<Enemy> _pool;
    private List<Enemy> _activeEnemies = new();
    private int _poolCapacity = 20;
    private int _poolMaxSize = 50;

    public event Action EnemyKilled;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_enemyPrefab),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    public Enemy Spawn(Vector3 position, Transform target)
    {
        Enemy enemy = _pool.Get();
        enemy.Dead += OnEnemyDead;
        enemy.transform.SetParent(transform);
        enemy.transform.position = position;
        enemy.Initialize(target);
        TryAddToActiveList(enemy);
        return enemy;
    }

    public void Restart()
    {
        while(_activeEnemies.Count > 0)
        {
            ReleaseEnemy(_activeEnemies[0]);
        }

        _activeEnemies.Clear();
    }

    private void OnEnemyDead(Enemy enemy)
    {
        ReleaseEnemy(enemy);
        EnemyKilled?.Invoke();
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        if (TryRemoveFromActiveList(enemy) == false)
            return;

        enemy.Dead -= OnEnemyDead;
        _pool.Release(enemy);
    }

    private bool TryAddToActiveList(Enemy enemy)
    {
        if(_activeEnemies.Contains(enemy)) 
            return false;

        _activeEnemies.Add(enemy);
        return true;
    }

    private bool TryRemoveFromActiveList(Enemy enemy)
    {
        if (_activeEnemies.Contains(enemy) == false)
            return false;

        _activeEnemies.Remove(enemy);
        return true;
    }
}
