public class NpcEatState : State
{
    private readonly NpcAnimationController npcAnimationController;
    private readonly PuduNpcStateMachine defaultNpcStateMachine;

    public NpcEatState(NpcAnimationController npcAnimationController, PuduNpcStateMachine defaultNpcStateMachine)
    {
        this.npcAnimationController = npcAnimationController;
        this.defaultNpcStateMachine = defaultNpcStateMachine;
    }

    public override void OnStateEnter()
    {
        npcAnimationController.SetBool(NpcAnimationType.Eat, true);
    }

    public override void OnStateExit()
    {
        npcAnimationController.SetBool(NpcAnimationType.Eat, false);
        defaultNpcStateMachine.UpdateEatPosition();
    }
}