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