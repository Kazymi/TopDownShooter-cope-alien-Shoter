using UnityEngine;
using UnityEngine.AI;

public class CustomerWalkState : State
{
    private readonly NavMeshAgent navMeshAgent;
    private readonly CharacterAnimationController characterAnimationController;
    private readonly Vector3 movePosition;

    public bool IsOnPosition => Vector3.Distance(navMeshAgent.transform.position, movePosition) < 1.2f;

    public CustomerWalkState(NavMeshAgent navMeshAgent, CharacterAnimationController characterAnimationController,
        Vector3 movePosition)
    {
        this.navMeshAgent = navMeshAgent;
        this.characterAnimationController = characterAnimationController;
        this.movePosition = movePosition;
    }

    public override void OnStateEnter()
    {
        navMeshAgent.enabled = true;
        characterAnimationController.SetBool(CharacterAnimationType.Walk, true);
    }

    public override void OnStateExit()
    {
        navMeshAgent.enabled = false;
        characterAnimationController.SetBool(CharacterAnimationType.Walk, false);
    }

    public override void OnUpdate(float deltaTime)
    {
        navMeshAgent.SetDestination(movePosition);
    }
}