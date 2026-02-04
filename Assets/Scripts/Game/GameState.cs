public abstract class GameState
{
    protected GameStateMachine StateMachine;
    protected GameContext Context;

    public GameState(GameStateMachine stateMachine, GameContext context)
    {
        StateMachine = stateMachine;
        Context = context;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Resume() { }
}
