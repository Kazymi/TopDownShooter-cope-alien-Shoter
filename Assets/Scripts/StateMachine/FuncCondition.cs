using System;
using UnityEngine;

public class FuncCondition : ACondition
{
    private Func<bool> returnValue;

    public FuncCondition(Func<bool> returnValue)
    {
        this.returnValue = returnValue;
    }

    public override bool IsConditionSuccess()
    {
        return returnValue.Invoke();
    }
}

public class TemporaryCondition : ACondition
{
    private readonly float _time;

    private float currentTime;

    public TemporaryCondition(float time)
    {
        _time = time;
    }

    public override bool IsConditionSuccess()
    {
        return currentTime >= _time;
    }

    public override void OnStateEntered()
    {
        currentTime = 0;
    }

    public override void OnTick(float deltaTime)
    {
        currentTime += deltaTime;
    }
}