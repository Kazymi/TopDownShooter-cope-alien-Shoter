public interface ICondition
{
    bool IsConditionSuccess();
    void OnStateEntered();
    void OnStateExited();
    void OnTick(float deltaTime);
}