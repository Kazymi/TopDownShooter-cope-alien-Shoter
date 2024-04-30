using System.Collections.Generic;
using UnityEngine;

public class StateTransition : AStateTransition
{
    private State nextState;

    public StateTransition(State nextState, ICondition condition)
    {
        this.nextState = nextState;
        this.condition = condition;
    }

    public override State NextState => nextState;
    public override bool IsConditionSuccess => condition.IsConditionSuccess();
}

public class RandomStateTransition : AStateTransition
{
    private List<State> nextStates;
    private ICondition condition;

    public override State NextState => nextStates[Random.Range(0, nextStates.Count)];
    public override bool IsConditionSuccess => condition.IsConditionSuccess();

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

public class TransitionWithMultiCondition : ITransition
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

    public void OnStateEntered()
    {
        foreach (var condition in condition)
        {
            condition.OnStateEntered();
        }
    }

    public void OnStateExited()
    {
        foreach (var condition in condition)
        {
            condition.OnStateExited();
        }
    }

    public void OnTick(float deltaTime)
    {
        foreach (var condition in condition)
        {
            condition.OnTick(deltaTime);
        }
    }
}

public interface ITransition
{
    State NextState { get; }
    bool IsConditionSuccess { get; }
    void OnStateEntered();
    void OnStateExited();
    void OnTick(float deltaTime);
}

public abstract class AStateTransition : ITransition
{
    public abstract State NextState { get; }
    public abstract bool IsConditionSuccess { get; }

    protected ICondition condition;

    public void OnStateEntered()
    {
        condition.OnStateEntered();
    }

    public void OnStateExited()
    {
        condition.OnStateExited();
    }

    public void OnTick(float deltaTime)
    {
        condition.OnTick(deltaTime);
    }
}