using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class DefaultNpcStateMachine : Damageable
{
    [SerializeField] private Transform floorPosition;
    [SerializeField] private Vector2 size;

    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private NpcWalkState walkState;
    private NpcRunState runState;
    private NpcDeadState deadState;

    private Vector3 startPosition;

    private Transform player;

    private StateMachine stateMachine;

    private void Update()
    {
        stateMachine.OnUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.OnFixedUpdate();
    }

    protected override void HealthUpdated()
    {
        if (stateMachine.CurrentState == deadState)
        {
            return;
        }

        ItemSpawner.Instance.SpawnEffect(EffectType.Hit, transform.position);
        UpdateRunState();
        if (currentHealth > 0)
        {
            return;
        }

        stateMachine.SetState(deadState);
    }

    public override void Initialize()
    {
        base.Initialize();
        startPosition = floorPosition.position;
        InitializeStateMachine();
    }

    private void UpdateRunState()
    {
        player ??= GameObject.FindGameObjectWithTag("Player").transform;
        var endPosition = transform.position + player.forward * 5;
        runState.SetPosition(endPosition);
        stateMachine.SetState(runState);
    }

    private void InitializeStateMachine()
    {
        var npcAnimatorController = new NpcAnimationController(animator);

        deadState = new NpcDeadState(npcAnimatorController, this,transform);
        var idleState = new NpcIdleState(npcAnimatorController);
        walkState = new NpcWalkState(npcAnimatorController, navMeshAgent);
        runState = new NpcRunState(npcAnimatorController, navMeshAgent);
        var eatState = new NpcEatState(npcAnimatorController, this);

        runState.AddTransition(new StateTransition(idleState, new FuncCondition(() => runState.IsOnPosition)));

        idleState.AddTransition(new StateTransition(walkState, new FuncCondition(() =>
        {
            UpdateEatPosition();
            return true;
        })));

        eatState.AddTransition(new StateTransition(walkState, new TemporaryCondition(5)));

        walkState.AddTransition(new StateTransition(eatState, new FuncCondition(() => walkState.IsOnPosition)));
        stateMachine = new StateMachine(idleState);
    }

    public void UpdateEatPosition()
    {
        var randomX = Random.Range(-size.x / 2f, size.x / 2f);
        var randomZ = Random.Range(-size.y / 2f, size.y / 2f);
        var newPosition = startPosition + new Vector3(randomX, 0, randomZ);
        walkState.SetPosition(newPosition);
    }


    private void OnDrawGizmos()
    {
        if (floorPosition == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(floorPosition.position, new Vector3(size.x, 0.2f, size.y));
    }
}

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

public class NpcDeadState : State
{
    private readonly NpcAnimationController npcAnimationController;
    private readonly IPooled pooled;
    private readonly Transform npc;

    private const float deadTime = 1f;
    private float currentTime;

    public NpcDeadState(NpcAnimationController npcAnimationController, IPooled pooled, Transform npc)
    {
        this.npcAnimationController = npcAnimationController;
        this.pooled = pooled;
        this.npc = npc;
    }

    public override void OnUpdate(float deltaTime)
    {
        currentTime -= deltaTime;
        if (currentTime <= 0)
        {
            npc.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                npc.transform.localScale = Vector3.one;
                pooled.ReturnToPool();
            });
        }
    }

    public override void OnStateEnter()
    {
        currentTime = deadTime;
        npcAnimationController.SetTrigger(NpcAnimationType.Dead);
    }
}

public class NpcEatState : State
{
    private readonly NpcAnimationController npcAnimationController;
    private readonly DefaultNpcStateMachine defaultNpcStateMachine;

    public NpcEatState(NpcAnimationController npcAnimationController, DefaultNpcStateMachine defaultNpcStateMachine)
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

public class NpcRunState : State
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


    public NpcRunState(NpcAnimationController npcAnimationController, NavMeshAgent navMeshAgent)
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
        navMeshAgent.speed += 1.5f;
        navMeshAgent.enabled = true;
        npcAnimationController.SetBool(NpcAnimationType.Run, true);
    }

    public override void OnStateExit()
    {
        navMeshAgent.speed -= 1.5f;
        navMeshAgent.enabled = false;
        endPosition = Vector3.zero;
        endTransform = null;

        npcAnimationController.SetBool(NpcAnimationType.Run, false);
    }
}