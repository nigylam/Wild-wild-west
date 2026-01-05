using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private int _roundsCount;
    [SerializeField] private float _roundStartDelay;
    [SerializeField] private int _roundStartEnemies;
    [SerializeField] private float _roundStartLength;
    [SerializeField] private int _roundEnemiesIncrement;
    [SerializeField] private float _roundLengthIncrement;
    [SerializeField] private int _bossesCount;

    [SerializeField] private EnemySpawner _enemySpawner;

    private int _roundEnemiesCount;
    private float _roundLength;
    private int _enemiesTotal;
    private int _currentRound = 1;

    private void OnEnable()
    {
        _enemySpawner.EnemyKilled += OnEnemyKilled;
    }

    private void Start()
    {
        _roundEnemiesCount = _roundStartEnemies;
        _roundLength = _roundStartLength;
        _enemiesTotal = _roundEnemiesCount + _bossesCount;
        _enemySpawner.StartRound(_roundLength, _roundEnemiesCount, _bossesCount, _roundStartDelay);
    }

    private void OnDisable()
    {
        _enemySpawner.EnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled()
    {
        if (--_enemiesTotal == 0)
            ProcessRounds();
    }

    private void ProcessRounds()
    {
        if (_currentRound >= _roundsCount)
            return;

        _currentRound++;
        _roundEnemiesCount += _roundEnemiesIncrement;
        _roundLength += _roundLengthIncrement;
        _enemiesTotal = _roundEnemiesCount +_bossesCount;
        _enemySpawner.StartRound(_roundLength, _roundEnemiesCount, _bossesCount, _roundStartDelay);
    }
}
