using System.Collections;
using System.Collections.Generic;

public abstract class State
{
    public List<StateTransition> StateTransitions = new List<StateTransition>();

    public virtual void OnStateEnter()
    {
    }

    public virtual void OnStateExit()
    {
    }

    public virtual void OnUpdate(float deltaTime)
    {
    }

    public virtual void OnFixedUpdate(float fixedDeltaTime)
    {
    }


    public void AddTransition(StateTransition stateTransition)
    {
        StateTransitions.Add(stateTransition);
    }

    public void RemoveTransition(StateTransition stateTransition)
    {
        StateTransitions.Remove(stateTransition);
    }
}