using DG.Tweening;
using UnityEngine;

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
                npc.transform.DOKill();
            });
        }
    }

    public override void OnStateEnter()
    {
        currentTime = deadTime;
        npcAnimationController.SetTrigger(NpcAnimationType.Dead);
    }
}