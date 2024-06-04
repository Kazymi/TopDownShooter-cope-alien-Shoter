using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class CustomerNPCStateMachine : MonoPooled
{
    [SerializeField] private CustomerUI customerUi;

    private Transform spawnPosition;
    private Transform tablePosition;
    private List<TableConfiguration> itemConfigurations;
    private CustomersTable customersTable;

    private StateMachine stateMachine;
    private CustomerConfiguration customerConfigurationParent;
    private CharacterAnimationController characterAnimationController;
    public bool IsItemsGrab;

    private Wallet wallet;

    private void Awake()
    {
        characterAnimationController = new CharacterAnimationController(GetComponent<Animator>());
    }

    private void Start()
    {
        wallet = ServiceLocator.GetService<Wallet>();
    }
    
    private void Update()
    {
        stateMachine.OnUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.OnFixedUpdate();
    }

    public override void Initialize()
    {
        base.Initialize();
        IsItemsGrab = false;
    }

    public void InitializeCustomer(List<TableConfiguration> itemConfigurations, Transform spawnPosition,
        Transform tablePosition, CustomersTable customersTable,CustomerConfiguration parent)
    {
        this.itemConfigurations = itemConfigurations;
        this.tablePosition = tablePosition;
        this.spawnPosition = spawnPosition;
        this.customersTable = customersTable;
        customerConfigurationParent = parent;
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        var agent = GetComponent<NavMeshAgent>();
        var customerIdleState = new CustomerIdleState(characterAnimationController);
        var waitState = new CustomerIdleState(characterAnimationController);
        var moveToTableState = new CustomerWalkState(agent, characterAnimationController, tablePosition.position);
        var moveToSpawnState = new CustomerWalkState(agent, characterAnimationController, spawnPosition.position);
        var angryState = new CustomerAngryState(characterAnimationController);

        customerIdleState.AddTransition(new StateTransition(moveToTableState, new FuncCondition(() => true)));

        moveToTableState.AddTransition(new StateTransition(waitState,
            new FuncCondition(() =>
            {
                if (moveToTableState.IsOnPosition)
                {
                    customersTable.TableInitialize(itemConfigurations, this);
                    customerUi.StartSlider(20);
                    return true;
                }

                return false;
            })));

        waitState.AddTransition(new StateTransition(angryState, new TemporaryCondition(20)));
        waitState.AddTransition(new StateTransition(moveToSpawnState, new FuncCondition(() =>
        {
            if (IsItemsGrab)
            {
                customerUi.DisableImage();
                wallet.AddMoney(100);
            }

            return IsItemsGrab;
        })));

        angryState.AddTransition(new StateTransition(moveToSpawnState, new FuncCondition(() =>
        {
            if (IsItemsGrab)
            {
                customerUi.DisableImage();
                wallet.AddMoney(50);
            }

            return IsItemsGrab;
        })));

        moveToSpawnState.AddTransition(new StateTransition(new State(), new FuncCondition(() =>
        {
            if (moveToSpawnState.IsOnPosition == false)
            {
                return false;
            }

            customerConfigurationParent.SpawnCustomer();
            ReturnToPool();
            return true;
        })));
        stateMachine = new StateMachine(customerIdleState);
    }
}