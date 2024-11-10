using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using UnityEngine;

public class BossBehaviorDash : EnemyBehavior
{
    [SerializeField] float dashSpeed;
    [SerializeField] Transform prefabMarkTarget;
    [Tooltip("Bullet wrap around boss when dash")]
    [SerializeField] Transform prefabBulletDash;
    Transform tfBulletDash;
    Transform tfMarkTarget;
    Transform tfTarget;
    //Vector3 lastPosAim = Vector3.zero;
    Vector3 directDash;
    bool isAiming = false;
    bool isDash = false;
    public override void Select(Action<EnemyBehaviorState> action)
    {
        isAiming = false;
        isDash = false;
        directDash = Vector3.zero;
        //lastPosAim = Vector3.zero;
        tfTarget = GameDynamicData.mainCharacter.transform;
        base.Select(action);
        PlayAnim();

    }

    public override void StartLongAction()
    {
        isDash = true;
        tfBulletDash = pool.Spawn(prefabBulletDash, this.transform.position, Quaternion.identity, this.transform);
        var projecttile = tfBulletDash.GetComponent<EnemyProjectTile>();
        if (projecttile)
        {
            SetupBullets(projecttile);
        }
        base.StartLongAction();
    }
    public override void EndLongAction()
    {
        isDash = false;
        if (pool.IsSpawned(tfBulletDash))
        {
            pool.Despawn(tfBulletDash);
        }
        base.EndLongAction();
    }
    public override void ShortAction()
    {
        base.ShortAction();
    }
    public override void MarkTargetAction()
    {
        isAiming = true;
        tfMarkTarget = pool.Spawn(prefabMarkTarget, this.transform.position, Quaternion.identity, this.transform);
        base.MarkTargetAction();
    }
    public override void UnmarkTargetAction()
    {
        if (tfTarget != null) 
        {
            //lastPosAim = tfTarget.position;
            directDash = Vector3.Normalize(tfTarget.position - this.transform.position);
            if (pool.IsSpawned(tfMarkTarget))
            {
                pool.Despawn(tfMarkTarget);
            }
            tfMarkTarget = null;
        }
        isAiming = false;
        base.UnmarkTargetAction();
    }

    public override void AnimationState_End(TrackEntry trackEntry)
    {
        isDash = false;
        base.AnimationState_End(trackEntry);
        FinishBehavior();

    }
    //private void StartDash()
    //{
    //    if (lastPosAim != Vector3.zero && !isAiming)
    //    {
    //        float distance = Vector3.Distance(lastPosAim, this.transform.position);
    //        float timeMove = distance / dashSpeed;
    //        this.transform.DOMove(lastPosAim, timeMove);
    //    }
    //}
    private void CreateBulletDash()
    {

    }
    private void Update()
    {
        if (tfTarget != null && tfMarkTarget != null && isAiming)
        {
            var curDirect = Vector3.Normalize( tfTarget.position - this.transform.position);
            tfMarkTarget.up = curDirect;
        }
        if (isDash && !isAiming && directDash != Vector3.zero)
        {
            this.transform.MoveTransformWithPhysic(directDash, dashSpeed * Time.fixedDeltaTime);
        }
    }
}
