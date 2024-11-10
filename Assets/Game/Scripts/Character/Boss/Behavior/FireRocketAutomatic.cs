using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRocketAutomatic : EnemyBehavior
{
    [SerializeField] private Transform[] firePosL;
    [SerializeField] private Transform[] firePosR;

    [SerializeField] private Transform prefabBullet;

    protected override void Awake()
    {
        base.Awake();
        
    }

    public override void Select(Action<EnemyBehaviorState> action)
    {
        base.Select(action);
        turnFaceable.TurnFaceToEnemy();
        PlayAnim();
    }
    public override void ShortAction()
    {
        Fire();
    }
    private void Fire()
    {
        Transform[] firePos = turnFaceable.IsFaceLeft ? firePosL : firePosR;
        //create bullet
        foreach (var tfFire in firePos)
        {
            var tfBullet = pool.Spawn(prefabBullet, tfFire.position, prefabBullet.transform.rotation);
            var projecttile = tfBullet.GetComponent<EnemyProjectTile>();
            if (projecttile)
            {
                SetupBullets(projecttile);
            }
            //var direct = ((tfFire.position + Vector3.up) - tfFire.position).normalized;
            //tfBullet.right = direct;
        }
    }
    public override void AnimationState_End(TrackEntry trackEntry)
    {
        base.AnimationState_End(trackEntry);
        FinishBehavior();
    }
}
