public class NpcIdleState : State
{
    private readonly NpcAnimationController npcAnimationController;

    public NpcIdleState(NpcAnimationController npcAnimationController)
    {
        this.npcAnimationController = npcAnimationController;
    }

    public override void OnStateEnter()
    {
        npcAnimationController.SetBool(NpcAnimationType.Idle, true);
    }

    public override void OnStateExit()
    {
        npcAnimationController.SetBool(NpcAnimationType.Idle, false);
    }
}