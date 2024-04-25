using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private float speedRotate;

    [SerializeField] private string currentState;

    private StateMachine _stateMachine;

    private void Awake()
    {
        InitializeStateMachine();
    }

    private void Update()
    {
        _stateMachine.OnUpdate();
        currentState = _stateMachine.CurrentState.ToString();
    }

    private void FixedUpdate()
    {
        _stateMachine.OnFixedUpdate();
    }

    private void InitializeStateMachine()
    {
        var characterAnimatorController = new CharacterAnimationController(animator);

        var idleState = new CharacterIdleState(characterAnimatorController);
        var moveState =
            new CharacterWalkState(characterAnimatorController, speed, speedRotate, rigidbody, inputController);
        var rollingState =
            new CharacterTriggerAnimationState(characterAnimatorController, CharacterAnimationType.IsRolling,
                CharacterAnimationType.Rolling);

        //состояние.добавлениеПерехода(переход(состонияние, условие)

        idleState.AddTransition(new StateTransition(moveState,
            new FuncCondition(() => inputController.MoveDirection != Vector3.zero)));

        moveState.AddTransition(new StateTransition(idleState,
            new FuncCondition(() => inputController.MoveDirection == Vector3.zero)));
        moveState.AddTransition(new StateTransition(rollingState, new FuncCondition(() => inputController.IsRolling)));

        rollingState.AddTransition(new StateTransition(idleState,
            new AnimationTransitionCondition(characterAnimatorController, "Rolling")));

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