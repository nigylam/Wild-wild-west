using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private Transform _target;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private EnemyPool _bossPool;

    private float _waitStep = 0.1f;
    private float _spawnZonePositionOffset = 0.5f;
    private WaitForSeconds _spawnWait;
    private Coroutine _spawnCoroutine;

    public event Action EnemyKilled;

    private void Awake()
    {
        _spawnWait = new WaitForSeconds(_waitStep);
    }

    private void OnEnable()
    {
        _enemyPool.EnemyKilled += OnEnemyKilled;
        _bossPool.EnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        _enemyPool.EnemyKilled -= OnEnemyKilled;
        _bossPool.EnemyKilled -= OnEnemyKilled;
    }

    public void StartRound(float roundLength, int enemiesCount, int bossesCount, float startSpawnDelay)
    {
        float spawnRate = roundLength / enemiesCount;

        if(_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        _spawnCoroutine = StartCoroutine(RepeatingSpawn(spawnRate, enemiesCount, bossesCount, startSpawnDelay));
    }

    public void Restart()
    {
        StopCoroutine(_spawnCoroutine);
        _enemyPool.Restart();
        _bossPool.Restart();
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
            _enemyPool.Spawn(position, _target);
        else
            _bossPool.Spawn(position, _target);
    }

    private void OnEnemyKilled() => EnemyKilled?.Invoke();
}