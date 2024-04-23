using UnityEngine;

public class CharacterWalkState : State
{
    private readonly CharacterAnimationController _characterAnimationController;
    private readonly float _speed;
    private readonly float _speedRotate;
    private readonly Rigidbody _characterBody;
    private readonly InputController _inputController;

    public CharacterWalkState(CharacterAnimationController characterAnimationController, float speed, float speedRotate,
        Rigidbody characterBody, InputController inputController)
    {
        _characterAnimationController = characterAnimationController;
        _speed = speed;
        _speedRotate = speedRotate;
        _characterBody = characterBody;
        _inputController = inputController;
    }

    public override void OnStateEnter()
    {
        _characterAnimationController.SetBool(CharacterAnimationType.Walk, true);
    }

    public override void OnStateExit()
    {
        _characterAnimationController.SetBool(CharacterAnimationType.Walk, false);
        _characterBody.velocity = Vector3.zero;
    }

    public override void OnFixedUpdate(float fixedDeltaTime)
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        _characterBody.velocity = _inputController.MoveDirection * _speed;
    }

    private void Rotate()
    {
        var charForward = _characterBody.transform.forward;
        var newDirection = Vector3.RotateTowards(charForward, _inputController.MoveDirection, _speed, 0.0f);
        _characterBody.rotation =
            Quaternion.Lerp(_characterBody.transform.rotation, Quaternion.LookRotation(newDirection),
                _speedRotate * Time.deltaTime);
    }
}