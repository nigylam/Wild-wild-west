using UnityEngine;

[RequireComponent(typeof(RoundCounter))]
public class Game : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int _roundsCount;
    [SerializeField] private float _roundStartDelay;
    [SerializeField] private int _roundStartEnemies;
    [SerializeField] private float _roundStartLength;
    [SerializeField] private int _roundEnemiesIncrement;
    [SerializeField] private float _roundLengthIncrement;
    [SerializeField] private int _bossesCount;

    [Header("Links")]
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private Player _player;
    [SerializeField] private OverlayMenu _overlay;
    [SerializeField] private GameObject _HUD;
    [SerializeField] private Bar _roundBar;

    private RoundCounter _roundCounter;
    private int _roundEnemiesCount;
    private float _roundLength;
    private int _enemiesTotal;

    private void Awake()
    {
        _roundCounter = GetComponent<RoundCounter>();
        _roundBar.Initialize(_roundCounter);
    }

    private void OnEnable()
    {
        _enemySpawner.EnemyKilled += OnEnemyKilled;
        _player.Dead += OnPlayerDead;
        _overlay.Restarted += Restart;
    }

    private void Start()
    {
        StartGame();
    }

    private void OnDisable()
    {
        _enemySpawner.EnemyKilled -= OnEnemyKilled;
        _player.Dead -= OnPlayerDead;
        _overlay.Restarted -= Restart;
    }

    private void StartGame()
    {
        _overlay.gameObject.SetActive(false);
        _HUD.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _roundCounter.Reset();
        _roundEnemiesCount = _roundStartEnemies;
        _roundLength = _roundStartLength;
        _enemiesTotal = _roundEnemiesCount + _bossesCount;
        _enemySpawner.StartRound(_roundLength, _roundEnemiesCount, _bossesCount, _roundStartDelay);
    }

    private void OnEnemyKilled()
    {
        if (--_enemiesTotal == 0)
            ProcessRounds();
    }

    private void ProcessRounds()
    {
        if (_roundCounter.Current >= _roundsCount)
            return;

        _roundCounter.Increase();
        _roundEnemiesCount += _roundEnemiesIncrement;
        _roundLength += _roundLengthIncrement;
        _enemiesTotal = _roundEnemiesCount + _bossesCount;
        _enemySpawner.StartRound(_roundLength, _roundEnemiesCount, _bossesCount, _roundStartDelay);
    }

    private void OnPlayerDead()
    {
        _overlay.gameObject.SetActive(true);
        _HUD.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Restart()
    {
        _enemySpawner.Restart();
        StartGame();
        _player.Restart();
    }
}
