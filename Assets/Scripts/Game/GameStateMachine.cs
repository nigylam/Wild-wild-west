using System.Collections.Generic;

public class GameStateMachine
{
    private Stack<GameState> _statesStack = new();
    private Dictionary<GameStateType, GameState> _states;

    public GameStateMachine(GameContext context) 
    {
        _states = new()
        {
            {GameStateType.Start, new StartState(this, context) },
            {GameStateType.Active, new ActiveState(this, context) },
            {GameStateType.Pause, new PauseState(this, context) },
            {GameStateType.EndWin, new EndState(this, context, true) },
            {GameStateType.EndLose, new EndState(this, context, false) },
        };
    }

    public void ChangeState(GameStateType stateType)
    {
        GameState state = _states[stateType];

        while (_statesStack.Count > 0)
        {
            _statesStack.Pop().Exit();
        }

        _statesStack.Push(state);
        state.Enter();
    }

    public void PushState(GameStateType stateType)
    {
        GameState state = _states[stateType];
        _statesStack.Push(state);
        state.Enter();
    }

    public void PopState()
    {
        if (_statesStack.Count == 0)
            return;

        _statesStack.Pop().Exit();

        if (_statesStack.Count > 0)
            _statesStack.Peek().Resume();
    }
}
