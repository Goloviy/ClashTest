using Spine;
using System;
using UnityEngine;

public class B_Meteor : EnemyBehavior
{
    [SerializeField] bool isInvisiable = false;
    [SerializeField] int countPerCall = 1;
    [SerializeField] Transform prefabBullet;
    [SerializeField] Transform prefabMark;
    [SerializeField] float timeMark = 0.4f;

    protected override void Awake()
    {
        base.Awake();
    }
    public override void Select(Action<EnemyBehaviorState> action)
    {
        base.Select(action);
        PlayAnim();
        canInvisiable.SetVisiable(true);
    }
    public override void StartLongAction()
    {
        if (isInvisiable)
        {
            canInvisiable.SetVisiable(false);
        }
    }
    public override void EndLongAction()
    {

        if (isInvisiable)
        {
            canInvisiable.SetVisiable(true);
        }
    }

    public override void ShortAction()
    {
        for (int i = 0; i < countPerCall; i++)
        {
            var pos = CameraFollower.Instance.RandomInsideCam();
            var tfMark = pool.Spawn(prefabMark, pos, Quaternion.identity);
            this.StartDelayAction(timeMark, () =>
            {
                tfMark.gameObject.SetActive(false);
            });
            var tfBullet = pool.Spawn(prefabBullet, pos, Quaternion.identity);
            var projecttile = tfBullet.GetComponent<EnemyProjectTile>();
            if (projecttile)
            {
                SetupBullets(projecttile);
            }
        }
    }

    public override void AnimationState_End(TrackEntry trackEntry)
    {
        FinishBehavior();
    }
}
