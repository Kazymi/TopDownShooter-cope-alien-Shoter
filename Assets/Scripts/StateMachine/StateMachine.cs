using UnityEngine;

public class StateMachine
{
    private State currentState;

    public State CurrentState => currentState;

    public void OnUpdate()
    {
        var deltaTime = Time.deltaTime;
        currentState.OnUpdate(deltaTime);

        foreach (var stateTransition in currentState.StateTransitions)
        {
            stateTransition.OnTick(deltaTime);
        }

        foreach (var stateTransition in currentState.StateTransitions)
        {
            if (stateTransition.IsConditionSuccess)
            {
                SetState(stateTransition.NextState);
            }
        }
    }

    public void OnFixedUpdate()
    {
        var fixedDeltaTime = Time.fixedDeltaTime;
        currentState.OnFixedUpdate(fixedDeltaTime);
    }

    public void SetState(State nextState)
    {
        currentState.OnStateExit();
        foreach (var stateTransition in currentState.StateTransitions)
        {
            stateTransition.OnStateExited();
        }

        currentState = nextState;
        currentState.OnStateEnter();
        foreach (var stateTransition in currentState.StateTransitions)
        {
            stateTransition.OnStateEntered();
        }
    }

    public StateMachine(State defaultState)
    {
        currentState = defaultState;
        currentState.OnStateEnter();
    }
}