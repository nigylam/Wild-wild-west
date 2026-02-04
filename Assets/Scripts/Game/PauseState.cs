using UnityEngine;

public class PauseState : GameState
{
    public PauseState(GameStateMachine stateMachine, GameContext context) : base(stateMachine, context) { }

    public override void Enter()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Context.Actions.Disable();
        Context.Player.Disable();
        Context.HUD.Disable();

        Context.Overlay.SetPauseMenu();
        Context.Overlay.Continued += Continue;
        Context.Overlay.Restarted += Restart;
    }

    public override void Exit()
    {
        Context.Overlay.Continued -= Continue;
        Context.Overlay.Restarted -= Restart;
    }

    private void Continue()
    {
        StateMachine.PopState();
    }

    private void Restart()
    {
        Context.Sound.Stop();
        Context.Spawner.Restart();
        Context.Player.Restart();
        StateMachine.ChangeState(GameStateType.Active);
    }
}
