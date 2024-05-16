public class CustomerAngryState : State
{
    private readonly CharacterAnimationController characterAnimationController;

    public CustomerAngryState(CharacterAnimationController characterAnimationController)
    {
        this.characterAnimationController = characterAnimationController;
    }

    public override void OnStateEnter()
    {
        characterAnimationController.SetBool(CharacterAnimationType.Angry, true);
    }

    public override void OnStateExit()
    {
        characterAnimationController.SetBool(CharacterAnimationType.Angry, false);
    }
}