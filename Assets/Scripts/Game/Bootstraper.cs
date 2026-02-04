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

        var actions = new ThirdPersonActions();
        var pause = new PauseActions();

        _player.Initialize(actions);
        _hud.Initialize(counter, _player);
        _overlay.Initialize(pause);

        _context = new GameContext
        (
            _player,
            _spawner,
            _overlay,
            sound,
            _roundSet,
            counter,
            actions,
            pause,
            _hud
        );

        _stateMachine = new GameStateMachine(_context);
        _stateMachine.ChangeState(GameStateType.Start);
    }
}
