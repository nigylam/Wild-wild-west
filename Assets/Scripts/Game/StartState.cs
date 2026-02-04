using UnityEngine;

public class StartState : GameState
{
    public StartState(GameStateMachine stateMachine, GameContext context) : base(stateMachine, context) {}

    public override void Enter()
    {
        Time.timeScale = 0;
        Context.HUD.Disable();
        Context.Actions.Disable();
        Context.Player.Disable();

        Context.Overlay.SetStartMenu();
        Context.Overlay.Restarted += StartGame;
    }

    public override void Exit()
    {
        Context.Overlay.Restarted -= StartGame;
    }

    private void StartGame()
    {
        StateMachine.ChangeState(GameStateType.Active);
    }
}
