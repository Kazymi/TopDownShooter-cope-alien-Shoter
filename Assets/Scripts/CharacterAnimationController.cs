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

public class AnimationTransitionCondition : ICondition
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

    public bool IsConditionSuccess()
    {
        return _characterAnimationController.IsAnimationPlay(_transitionName) &&
               _characterAnimationController.NormalizedAnimationPlayTime() > _exitTime;
    }
}

public enum CharacterAnimationType
{
    Idle,
    Walk,
    RunFloat,
    Rolling,
    IsRolling
}