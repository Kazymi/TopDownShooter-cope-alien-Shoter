using UnityEngine;

public class HandAxe : PlayerHandItem
{
    private InputController inputController;
    private CharacterAnimationController characterAnimationController;
    
    private void Update()
    {
        inputController ??= FindObjectOfType<InputController>();
        characterAnimationController ??= FindObjectOfType<CharacterStateMachine>().CharacterAnimationController;
        if (IsSelectable == false)
        {
            characterAnimationController.SetBool(CharacterAnimationType.AxeAttack, inputController.IsAttack);
        }
    }

    public override void ItemDeSelected()
    {
        base.ItemDeSelected();
        characterAnimationController.SetBool(CharacterAnimationType.AxeAttack, false);
    }
}