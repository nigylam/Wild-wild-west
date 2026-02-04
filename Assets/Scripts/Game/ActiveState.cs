using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveState : GameState
{
    private int _enemiesTotal;

    public ActiveState(GameStateMachine stateMachine, GameContext context) : base(stateMachine, context) { }

    public override void Enter()
    {
        Resume();
        StartGame();
    }

    public override void Resume()
    {
        AudioListener.pause = false;
        Time.timeScale = 1;

        Context.Actions.Enable();
        Context.Player.Enable();
        Context.HUD.Enable();
        Context.PauseAction.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Context.Spawner.EnemyKilled += OnEnemyKilled;
        Context.Player.Dead += OnPlayerDead;
        Context.PauseAction.PauseAction.Pause.performed += OnPause;
        Context.Spawner.RoundStarted += Context.Sound.PlayStartRound;
    }

    private void OnPlayerDead()
    {
        StateMachine.ChangeState(GameStateType.EndLose);
    }

    private void OnPause(InputAction.CallbackContext _)
    {
        StateMachine.PushState(GameStateType.Pause);
    }

    private void OnEnemyKilled()
    {
        if (--_enemiesTotal > 0)
            return;

        if (Context.Counter.Current >= Context.Rounds.RoundsCount)
        {
            StateMachine.ChangeState(GameStateType.EndWin);
            return;
        }

        Context.Sound.PlayEndRound();
        Context.Counter.Increase();
        Context.Rounds.ProcessRound();

        StartRound();
    }

    public override void Exit()
    {
        Context.Spawner.EnemyKilled -= OnEnemyKilled;
        Context.Player.Dead -= OnPlayerDead;
        Context.PauseAction.PauseAction.Pause.performed -= OnPause;
        Context.Spawner.RoundStarted -= Context.Sound.PlayStartRound;
    }

    private void StartGame()
    {
        Context.Rounds.StartSet();
        Context.Counter.Reset();
        StartRound();
    }

    private void StartRound()
    {
        Round round = Context.Rounds.CurrentRound;

        _enemiesTotal = round.EnemiesCount + round.BossesCount;

        Context.Spawner.StartRound(
            round.RoundLength,
            round.EnemiesCount,
            round.BossesCount,
            Context.Rounds.RoundDelay
        );
    }
}
