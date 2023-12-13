using System.Collections.Generic;

public abstract class StateMachine
{
    public eStateType previousStateType { get; private set; }
    public eStateType currentStateType { get; private set; }
    public IState CurrentState { get => _currentState; }

    protected IState _currentState;
    public Dictionary<eStateType, IState> States { get; private set; } = new Dictionary<eStateType, IState>();

    public virtual void ChangeState(eStateType newState)
    {
        if (States.ContainsKey(newState))
        {
            _currentState?.Exit();

            previousStateType = currentStateType;
            currentStateType = newState;

            _currentState = States[newState];

            _currentState?.Enter();
        }
    }

    public void Update()
    {
        _currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        _currentState?.PhysicsUpdate();
    }

    public virtual void AddState(eStateType statetype, IState state)
    {
        States.Add(statetype, state);
    }

    public void PlaySound()
    {
        _currentState?.PlaySound();
    }
}