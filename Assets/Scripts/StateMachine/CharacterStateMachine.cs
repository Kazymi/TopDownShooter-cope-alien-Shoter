using Cinemachine;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera walkCamera;
    [SerializeField] private CinemachineVirtualCamera runCamera;
    
    [SerializeField] private InputController inputController;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private float speedRotate;

    [SerializeField] private string currentState;

    public CharacterAnimationController CharacterAnimationController { get; private set; }
    private StateMachine _stateMachine;

    private void Awake()
    {
        InitializeStateMachine();
    }

    private void Update()
    {
        _stateMachine.OnUpdate();
        currentState = _stateMachine.CurrentState.ToString();
        walkCamera.Priority = inputController.IsRunning ? 0 : 1;
        runCamera.Priority = inputController.IsRunning ? 1 : 0;
    }

    private void FixedUpdate()
    {
        _stateMachine.OnFixedUpdate();
    }

    private void InitializeStateMachine()
    {
        var characterAnimatorController = new CharacterAnimationController(animator);
        CharacterAnimationController = characterAnimatorController;
        var idleState = new CharacterIdleState(characterAnimatorController);
        var moveState =
            new CharacterWalkState(characterAnimatorController, speed, speedRotate, rigidbody, inputController);
        var runState =
            new CharacterRunState(characterAnimatorController, speed, speedRotate, rigidbody, inputController);


        idleState.AddTransition(new StateTransition(moveState,
            new FuncCondition(() => inputController.MoveDirection != Vector3.zero)));

        moveState.AddTransition(new StateTransition(idleState,
            new FuncCondition(() => inputController.MoveDirection == Vector3.zero)));

        moveState.AddTransition(new StateTransition(runState, new FuncCondition(() => inputController.IsRunning)));

        runState.AddTransition(new StateTransition(idleState,
            new FuncCondition(() => inputController.MoveDirection == Vector3.zero)));
        runState.AddTransition(new StateTransition(moveState,
            new FuncCondition(() => inputController.IsRunning == false)));

        _stateMachine = new StateMachine(idleState);
    }
}

public class CharacterTriggerAnimationState : State
{
    private readonly CharacterAnimationController _characterAnimationController;
    private readonly CharacterAnimationType _trigger;
    private readonly CharacterAnimationType _boolValue;

    public CharacterTriggerAnimationState(CharacterAnimationController characterAnimationController,
        CharacterAnimationType trigger, CharacterAnimationType boolValue)
    {
        _characterAnimationController = characterAnimationController;
        _trigger = trigger;
        _boolValue = boolValue;
    }

    public override void OnStateEnter()
    {
        _characterAnimationController.SetTrigger(_trigger);
        _characterAnimationController.SetBool(_boolValue, true);
    }

    public override void OnStateExit()
    {
        _characterAnimationController.SetBool(_boolValue, false);
    }
}