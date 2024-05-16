using UnityEngine;
using UnityEngine.AI;

public class NpcWalkState : State
{
    private readonly NpcAnimationController npcAnimationController;
    private readonly NavMeshAgent navMeshAgent;

    private Vector3 endPosition;
    private Transform endTransform;

    private const float stopDistance = 1;

    public bool IsOnPosition => CheckPosition();

    private bool CheckPosition()
    {
        if (endTransform)
        {
            var isOnPosition = Vector3.Distance(navMeshAgent.transform.position, endTransform.position) <= stopDistance;
            return isOnPosition;
        }
        else
        {
            var isOnPosition = Vector3.Distance(navMeshAgent.transform.position, endPosition) <= stopDistance;
            return isOnPosition;
        }
    }


    public NpcWalkState(NpcAnimationController npcAnimationController, NavMeshAgent navMeshAgent)
    {
        this.npcAnimationController = npcAnimationController;
        this.navMeshAgent = navMeshAgent;
    }

    public void SetPosition(Vector3 endPosition)
    {
        this.endPosition = endPosition;
    }

    public void SetPosition(Transform endTransform)
    {
        this.endTransform = endTransform;
    }

    public override void OnUpdate(float deltaTime)
    {
        navMeshAgent.SetDestination(endTransform == null ? endPosition : endTransform.position);
    }

    public override void OnStateEnter()
    {
        navMeshAgent.enabled = true;
        npcAnimationController.SetBool(NpcAnimationType.Walk, true);
    }

    public override void OnStateExit()
    {
        navMeshAgent.enabled = false;
        endPosition = Vector3.zero;
        endTransform = null;

        npcAnimationController.SetBool(NpcAnimationType.Walk, false);
    }
}