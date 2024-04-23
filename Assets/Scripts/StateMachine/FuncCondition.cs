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

public class TemporaryCondition : ICondition
{
    private readonly float _time;

    public TemporaryCondition(float time)
    {
        _time = time;
    }
    public bool IsConditionSuccess()
    {
        
    }
    
    
}