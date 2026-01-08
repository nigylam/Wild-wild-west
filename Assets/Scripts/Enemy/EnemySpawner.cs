using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Boss _bossPrefab;
    [SerializeField] private Transform _target;

    private float _waitStep = 0.1f;
    private float _spawnZonePositionOffset = 0.5f;
    private ObjectPool<Enemy> _enemyPool;
    private ObjectPool<Boss> _bossPool;
    private int _poolCapacity = 20;
    private int _poolMaxSize = 50;
    private WaitForSeconds _spawnWait;
    private Coroutine _spawnCoroutine;

    public event Action EnemyKilled;

    private void Awake()
    {
        _spawnWait = new WaitForSeconds(_waitStep);

        _enemyPool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_enemyPrefab),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );

        _bossPool = new ObjectPool<Boss>(
            createFunc: () => Instantiate(_bossPrefab),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    public void StartRound(float roundLength, int enemiesCount, int bossesCount, float startSpawnDelay)
    {
        float spawnRate = roundLength / enemiesCount;

        if(_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        _spawnCoroutine = StartCoroutine(RepeatingSpawn(spawnRate, enemiesCount, bossesCount, startSpawnDelay));
    }

    private IEnumerator RepeatingSpawn(float spawnRate, int enemiesCount, int bossesCount, float startSpawnDelay)
    {
        int startSpawnNumberIterations = Convert.ToInt32(startSpawnDelay / _waitStep);

        for (int c = 0; c < startSpawnNumberIterations; c++)
            yield return _spawnWait;

        for (int i = 0; i < enemiesCount; i++)
        {
            Spawn(EnemyType.Enemy);

            int numberIterations = Convert.ToInt32(spawnRate / _waitStep);

            if (enemiesCount - i <= bossesCount)
                Spawn(EnemyType.Boss);

            for (int a = 0; a < numberIterations; a++)
                yield return _spawnWait;
        }
    }

    private void Spawn(EnemyType enemyType)
    {
        Transform spawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count - 1)];

        Vector3 spawnPointPosition = spawnPoint.transform.position;

        Vector3 position = new
            (
                UnityEngine.Random.Range
                (
                    spawnPointPosition.x - _spawnZonePositionOffset,
                    spawnPointPosition.x + _spawnZonePositionOffset
                ),

                spawnPointPosition.y,

                UnityEngine.Random.Range
                (
                    spawnPointPosition.z - _spawnZonePositionOffset,
                    spawnPointPosition.z + _spawnZonePositionOffset
                )
             );

        if (enemyType == EnemyType.Enemy)
            SpawnEnemy(position);
        else
            SpawnBoss(position);
    }

    private void SpawnEnemy(Vector3 position)
    {
        Enemy enemy = _enemyPool.Get();
        enemy.Released += ReleaseEnemy;
        enemy.transform.SetParent(transform);
        enemy.transform.position = position;
        enemy.Initialize(_target);
    }

    private void SpawnBoss(Vector3 position)
    {
        Boss boss = _bossPool.Get();
        boss.Released += ReleaseBoss;
        boss.transform.SetParent(transform);
        boss.transform.position = position;
        boss.Initialize(_target);
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        enemy.Released -= ReleaseEnemy;
        _enemyPool.Release(enemy);
        EnemyKilled?.Invoke();
    }

    private void ReleaseBoss(Enemy boss)
    {
        boss.Released -= ReleaseBoss;
        _bossPool.Release(boss as Boss);
        EnemyKilled?.Invoke();
    }
}