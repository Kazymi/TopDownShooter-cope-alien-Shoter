using System;

public class FuncCondition : ICondition
{
    private Func<bool> returnValue;

    public FuncCondition(Func<bool> returnValue)
    {
        this.returnValue = returnValue;
    }

    public bool IsConditionSuccess()
    {
        return returnValue.Invoke();
    }
}