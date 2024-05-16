using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ItemDropper))]
public class PuduNpcStateMachine : Damageable
{
    [SerializeField] private Transform floorPosition;
    [SerializeField] private Vector2 size;

    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private NpcWalkState walkState;
    private NpcRunState runState;
    private NpcDeadState deadState;

    private ItemDropper itemDropper;

    private Vector3 startPosition;

    private Transform player;

    private StateMachine stateMachine;
    private List<float> damagePercent = new List<float>();

    private void Update()
    {
        stateMachine.OnUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.OnFixedUpdate();
    }

    protected override void OnAwake()
    {
        itemDropper = GetComponent<ItemDropper>();
    }

    protected override void HealthUpdated()
    {
        if (stateMachine.CurrentState == deadState)
        {
            return;
        }
        
        var currentPercent = (float) currentHealth / maxHealth;
        currentPercent *= 100f;
        UpdatePercent(currentPercent);

        ItemSpawner.Instance.SpawnEffect(EffectType.Hit, transform.position);
        UpdateRunState();
        if (currentHealth > 0)
        {
            return;
        }

        stateMachine.SetState(deadState);
    }


    private void UpdatePercent(float currentPercent)
    {
        for (int i = 0; i < damagePercent.Count; i++)
        {
            if (damagePercent[i] >= currentPercent)
            {
                itemDropper.DropItem();
                damagePercent.Remove(damagePercent[i]);
                if (damagePercent.Count != 0)
                {
                    UpdatePercent(currentPercent);
                }

                break;
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        damagePercent = new List<float> {50f, 0f};
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

    protected virtual void InitializeStateMachine()
    {
        var npcAnimatorController = new NpcAnimationController(animator);

        deadState = new NpcDeadState(npcAnimatorController, this, transform);
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