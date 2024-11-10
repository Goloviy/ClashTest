using PathologicalGames;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    SkeletonAnimation skeAnim;
    [SpineAnimation(dataField: "skeAnim")]
    public string animationName = "";

    protected bool isSelected;
    protected Action<EnemyBehaviorState> action;

    protected ITurnFaceable turnFaceable;
    protected ICanInvisiable canInvisiable;
    protected SpawnPool pool;
    protected int monsterId;
    protected virtual void Awake()
    {
        pool = PoolManager.Pools[StringConst.POOL_MONSTER_BULLET_NAME];
        isSelected = false;
        skeAnim = GetComponentInChildren<SkeletonAnimation>();
    }
    protected virtual void Start()
    {
        skeAnim.AnimationState.Complete += AnimationState_End;
    }

    public virtual void AnimationState_End(TrackEntry trackEntry)
    {
        //DebugCustom.Log("End Animation");
    }

    public virtual void Init(ITurnFaceable turnFaceable, ICanInvisiable canInvisiable, int monsterId )
    {
        this.monsterId = monsterId;
        this.turnFaceable = turnFaceable;
        this.canInvisiable = canInvisiable;
    }
    public virtual void SetupBullets(params EnemyProjectTile[] enemyProjectTiles)
    {
        foreach (var enemyProjectTile in enemyProjectTiles)
        {
            enemyProjectTile.Init(this.monsterId);
        }
    }
    public virtual void Select(Action<EnemyBehaviorState> action)
    {
        this.action = action;
        isSelected = true;
    }
    public virtual void PlayAnim(bool isLoop = false)
    {
        skeAnim.state.SetAnimation(0, animationName, isLoop);
    }

    public virtual void ShortAction()
    {

    }
    public virtual void MarkTargetAction()
    {

    }
    public virtual void UnmarkTargetAction()
    {

    }
    public virtual void StartLongAction()
    {
        

    }
    public virtual void EndLongAction()
    {
        

    }
    public virtual void Explosion(Vector3 target)
    {

    }
    public virtual void FinishBehavior()
    {
        if (isSelected)
        {
            //DebugCustom.Log(this.GetType().ToString() + ": FinishBehavior");
            isSelected = false;
            this.action?.Invoke(EnemyBehaviorState.FINISH);
        }

    }

}
