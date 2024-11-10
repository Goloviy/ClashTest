using DG.Tweening;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// move random in cage
/// </summary>
public class MovingRandomBehavior : EnemyBehavior
{
    [SerializeField] Transform tfModel;
    [SerializeField] float moveSpeed = 2f;
    MainCharacter target;

    Vector3 moveTarget;
    string idMove = "moveidboss";
    protected override void Start()
    {
        //target = GameDynamicData.mainCharacter;

    }

    public override void Select(Action<EnemyBehaviorState> action)
    {
        base.Select(action);
        PlayAnim(true);
        RandomTarget();
        MoveToTarget();
    }
    private void MoveToTarget()
    {
        var distance = Vector3.Distance(this.transform.position, moveTarget);
        float time = distance / moveSpeed;
        this.transform.DOMove(moveTarget, time).SetEase(Ease.Linear).SetId(idMove).onComplete += FinishBehavior;
    }

    private void RandomTarget()
    {
        moveTarget = GameDynamicData.BirdCage.GetRandomPosAroundPlayer();
        turnFaceable.TurnFaceToPoint(moveTarget);
    }
    public override void FinishBehavior()
    {
        base.FinishBehavior();
        if (isSelected)
        {
            DOTween.Kill(idMove);
        }
    }
    public override void AnimationState_End(TrackEntry trackEntry)
    {
        
    }
}
