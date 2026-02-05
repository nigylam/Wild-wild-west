using UnityEngine;

[RequireComponent(typeof(GameSound))]
[RequireComponent(typeof(RoundCounter))]
public class Bootstraper : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] private OverlayMenu _overlay;
    [SerializeField] private RoundSet _roundSet;
    [SerializeField] private HUD _hud;

    private GameStateMachine _stateMachine;
    private GameContext _context;

    private void Awake()
    {
        var sound = GetComponent<GameSound>();
        var counter = GetComponent<RoundCounter>();
        var playerInputActions = new ThirdPersonActions();
        var pauseInputAction = new PauseActions();

        _player.Initialize(playerInputActions);
        _overlay.Initialize(pauseInputAction);
        _hud.Initialize(counter);

        _context = new GameContext
        (
            _player,
            _spawner,
            _overlay,
            sound,
            _roundSet,
            counter,
            playerInputActions,
            pauseInputAction,
            _hud
        );

        _stateMachine = new GameStateMachine(_context);
        _stateMachine.ChangeState(GameStateType.Start);
    }
}
