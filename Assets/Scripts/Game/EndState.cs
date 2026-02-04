using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EndState : GameState
{
    private bool _isWin;

    public EndState(GameStateMachine stateMachine, GameContext context, bool isWin) : base(stateMachine, context)
    {
        _isWin = isWin;
    }

    public override void Enter()
    {
        Time.timeScale = 0;

        Context.Actions.Disable();
        Context.Player.Disable();
        Context.HUD.Disable();

        if (_isWin)
        {
            Context.Sound.PlayWinGame();
            Context.Overlay.SetWinMenu();
        }
        else
        {
            Context.Sound.PlayLoseGame();
            Context.Overlay.SetLoseMenu();
        }

        Context.Overlay.Restarted += Restart;
    }

    public override void Exit()
    {
        Context.Spawner.Restart();
        Context.Player.Restart();
        Context.Overlay.Restarted -= Restart;
    }

    private void Restart()
    {
        StateMachine.ChangeState(GameStateType.Start);
    }
}
