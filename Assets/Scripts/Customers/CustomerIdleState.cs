public class CustomerIdleState : State
{
    private readonly CharacterAnimationController characterAnimationController;

    public CustomerIdleState(CharacterAnimationController characterAnimationController)
    {
        this.characterAnimationController = characterAnimationController;
    }

    public override void OnStateEnter()
    {
        characterAnimationController.SetBool(CharacterAnimationType.Idle, true);
    }

    public override void OnStateExit()
    {
        characterAnimationController.SetBool(CharacterAnimationType.Idle, false);
    }
}