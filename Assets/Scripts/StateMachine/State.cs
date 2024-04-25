using System.Collections;
using System.Collections.Generic;

public abstract class State
{
    public List<ITransition> StateTransitions = new List<ITransition>();

    public virtual void OnStateEnter()
    {
    }

    public virtual void OnStateExit()
    {
        foreach (var VARIABLE in StateTransitions)
        {
            
        }
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