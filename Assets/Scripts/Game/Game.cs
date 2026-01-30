using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GameSound))]
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
    [SerializeField] private int _bossesStartCount;
    [SerializeField] private int _bossesIncrement;

    [Header("Links")]
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private Player _player;
    [SerializeField] private OverlayMenu _overlay;
    [SerializeField] private GameObject _HUD;
    [SerializeField] private Bar _roundBar;

    private ThirdPersonActions _actions;
    private PauseActions _pauseActions;
    private GameSound _sound;
    private RoundCounter _roundCounter;
    private int _roundEnemiesCount;
    private int _roundBossesCount;
    private float _roundLength;
    private int _enemiesTotal;
    private GameState _gameState;

    private void Awake()
    {
        _sound = GetComponent<GameSound>();
        _roundCounter = GetComponent<RoundCounter>();
        _roundBar.Initialize(_roundCounter);
        _actions = new ThirdPersonActions();
        _pauseActions = new PauseActions();
        _player.Initialize(_actions);
        _overlay.Initialize(_pauseActions);
    }

    private void OnEnable()
    {
        _enemySpawner.EnemyKilled += OnEnemyKilled;
        _enemySpawner.RoundStarted += _sound.PlayStartRound;
        _player.Dead += OnPlayerDead;
        _pauseActions.PauseAction.Pause.performed += OnPause;
        _pauseActions.Enable();
    }

    private void Start()
    {
        Time.timeScale = 0f;
        _HUD.SetActive(false);
        _actions.Disable();
        _player.Disable();
        _overlay.SetStartMenu();
        _overlay.Restarted += StartGame;
        _gameState = GameState.NotStarted;
    }

    private void OnDisable()
    {
        _enemySpawner.EnemyKilled -= OnEnemyKilled;
        _enemySpawner.RoundStarted -= _sound.PlayStartRound;
        _player.Dead -= OnPlayerDead;
        _pauseActions.PauseAction.Pause.performed -= OnPause;
        _pauseActions.Disable();
    }

    private void StartGame()
    {
        AudioListener.pause = false;
        _actions.Enable();
        _gameState = GameState.Active;
        _HUD.SetActive(true);
        _overlay.Restarted -= StartGame;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        _player.Enable();
        _roundCounter.Reset();
        _roundEnemiesCount = _roundStartEnemies;
        _roundBossesCount = _bossesStartCount;
        _roundLength = _roundStartLength;
        _enemiesTotal = _roundEnemiesCount + _roundBossesCount;
        _enemySpawner.StartRound(_roundLength, _roundEnemiesCount, _roundBossesCount, _roundStartDelay);
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (_gameState == GameState.Active)
            Pause();
    }

    private void Pause()
    {
        _gameState = GameState.Paused;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        _actions.Disable();
        _player.Disable();
        _overlay.SetPauseMenu();
        _HUD.SetActive(false);
        _overlay.Continued += Continue;
        _overlay.Restarted += Restart;
    }

    private void Continue()
    {
        _actions.Enable();
        _gameState = GameState.Active;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _HUD.SetActive(true);
        Time.timeScale = 1f;
        _player.Enable();
        _overlay.Continued -= Continue;
        _overlay.Restarted -= Restart;
    }

    private void Restart()
    {
        _overlay.Continued -= Continue;
        _overlay.Restarted -= Restart;
        _sound.Stop();
        _enemySpawner.Restart();
        StartGame();
        _player.Restart();
        _player.Enable();
    }

    private void OnEnemyKilled()
    {
        if (--_enemiesTotal == 0)
            ProcessRounds();
    }

    private void ProcessRounds()
    {
        if (_roundCounter.Current >= _roundsCount)
        {
            End(true);
            _sound.PlayWinGame();
            return;
        }

        _sound.PlayEndRound();
        _roundCounter.Increase();
        _roundEnemiesCount += _roundEnemiesIncrement;
        _roundBossesCount += _bossesIncrement;
        _roundLength += _roundLengthIncrement;
        _enemiesTotal = _roundEnemiesCount + _roundBossesCount;
        _enemySpawner.StartRound(_roundLength, _roundEnemiesCount, _roundBossesCount, _roundStartDelay);
    }

    private void OnPlayerDead()
    {
        End(false);
        _sound.PlayLoseGame();
    }

    private void End(bool isWin)
    {
        _player.Disable();
        _actions.Disable();
        _gameState = GameState.NotStarted;

        if (isWin)
            _overlay.SetWinMenu();
        else
            _overlay.SetLoseMenu();

        _overlay.Restarted += Restart;
        _HUD.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
