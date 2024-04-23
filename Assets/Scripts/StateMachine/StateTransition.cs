using System.Collections.Generic;
using UnityEngine;

public class StateTransition
{
    private State nextState;
    private ICondition condition;

    public State NextState => nextState;

    public bool IsConditionSuccess => condition.IsConditionSuccess();

    public StateTransition(State nextState, ICondition condition)
    {
        this.nextState = nextState;
        this.condition = condition;
    }
}

public class RandomStateTransition
{
    private List<State> nextStates;
    private ICondition condition;

    public State NextState => nextStates[Random.Range(0, nextStates.Count)];

    public bool IsConditionSuccess => condition.IsConditionSuccess();

    public RandomStateTransition(State firstState, ICondition condition)
    {
        nextStates = new List<State>();
        nextStates.Add(firstState);
        this.condition = condition;
    }

    public void AddState(State state)
    {
        nextStates.Add(state);
    }
}

public class TransitionWithMultiCondition
{
    private State nextState;
    private List<ICondition> condition;

    public State NextState => nextState;

    public bool IsConditionSuccess => IsConditionsSuccess();

    private bool IsConditionsSuccess()
    {
        foreach (var condition in condition)
        {
            if (condition.IsConditionSuccess() == false)
            {
                return false;
            }
        }

        return true;
    }

    public TransitionWithMultiCondition(State nextState, List<ICondition> condition)
    {
        this.nextState = nextState;
        this.condition = condition;
    }
}