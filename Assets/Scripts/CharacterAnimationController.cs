using System;
using System.Collections.Generic;
using UnityEngine;


public class CharacterAnimationController
{
    private Animator _animator;
    private Dictionary<CharacterAnimationType, int> hashStorage = new Dictionary<CharacterAnimationType, int>();

    public Animator Animator => _animator;

    public CharacterAnimationController(Animator animator)
    {
        _animator = animator;
        foreach (CharacterAnimationType caType in Enum.GetValues(typeof(CharacterAnimationType)))
        {
            hashStorage.Add(caType, Animator.StringToHash(caType.ToString()));
        }
    }

    public void SetBool(CharacterAnimationType animationType, bool value)
    {
        _animator.SetBool(hashStorage[animationType], value);
    }

    public void SetFloat(CharacterAnimationType animationType, float value)
    {
        _animator.SetFloat(hashStorage[animationType], value);
    }

    public void SetPlay(CharacterAnimationType characterAnimationType)
    {
        _animator.Play((hashStorage[characterAnimationType]));
    }

    public void SetTrigger(CharacterAnimationType characterAnimationType)
    {
        _animator.SetTrigger((hashStorage[characterAnimationType]));
    }

    public bool IsAnimationPlay(string animationStateName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName);
    }

    public float NormalizedAnimationPlayTime()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}

public class NpcAnimationController
{
    private Animator _animator;
    private Dictionary<NpcAnimationType, int> hashStorage = new Dictionary<NpcAnimationType, int>();

    public Animator Animator => _animator;

    public NpcAnimationController(Animator animator)
    {
        _animator = animator;
        foreach (NpcAnimationType caType in Enum.GetValues(typeof(NpcAnimationType)))
        {
            hashStorage.Add(caType, Animator.StringToHash(caType.ToString()));
        }
    }

    public void SetBool(NpcAnimationType animationType, bool value)
    {
        _animator.SetBool(hashStorage[animationType], value);
    }

    public void SetFloat(NpcAnimationType animationType, float value)
    {
        _animator.SetFloat(hashStorage[animationType], value);
    }

    public void SetPlay(NpcAnimationType characterAnimationType)
    {
        _animator.Play((hashStorage[characterAnimationType]));
    }

    public void SetTrigger(NpcAnimationType characterAnimationType)
    {
        _animator.SetTrigger((hashStorage[characterAnimationType]));
    }

    public bool IsAnimationPlay(string animationStateName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName);
    }

    public float NormalizedAnimationPlayTime()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}

public class AnimationTransitionCondition : ACondition
{
    private readonly CharacterAnimationController _characterAnimationController;
    private readonly string _transitionName;
    private readonly float _exitTime;

    public AnimationTransitionCondition(CharacterAnimationController characterAnimationController,
        string transitionName, float exitTime = 0.9f)
    {
        _characterAnimationController = characterAnimationController;
        _transitionName = transitionName;
        _exitTime = exitTime;
    }

    public override bool IsConditionSuccess()
    {
        return _characterAnimationController.IsAnimationPlay(_transitionName) &&
               _characterAnimationController.NormalizedAnimationPlayTime() > _exitTime;
    }
}

public abstract class ACondition : ICondition
{
    public abstract bool IsConditionSuccess();

    public virtual void OnStateEntered()
    {
    }

    public virtual void OnStateExited()
    {
    }

    public virtual void OnTick(float deltaTime)
    {
    }
}

public enum CharacterAnimationType
{
    Idle,
    Walk,
    RunFloat,
    Rolling,
    IsRolling,
    IsItemGrab,
    Clear,
    AxeAttack,
    Angry,
}

public enum NpcAnimationType
{
    Idle,
    Walk,
    Run,
    Dead,
    Eat,
}