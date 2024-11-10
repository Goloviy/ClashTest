using DG.Tweening;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpread : EnemyBehavior
{
    [SerializeField] private Transform[] firePosL;
    [SerializeField] private Transform[] firePosR;
    [SerializeField] private Transform[] tfDicrectPointL;
    [SerializeField] private Transform[] tfDicrectPointR;

    [SerializeField] private Transform prefabBullet;
    [SerializeField] private float lifeTimeBullet = 1f;
    [SerializeField] private float bulletSpeed = 5f;
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
        Transform[] firePos = !turnFaceable.IsFaceLeft ? firePosL : firePosR;
        Transform[] endFirePos = !turnFaceable.IsFaceLeft ? tfDicrectPointL : tfDicrectPointR;
        //create bullet
        foreach (var tfStartFire in firePos)
        {
            foreach (var tfEndFire in endFirePos)
            {
                var direct = (firePos[0].position - tfEndFire.position).normalized;
                var tfBullet = pool.Spawn(prefabBullet, tfStartFire.position, Quaternion.identity);
                var projecttile = tfBullet.GetComponent<EnemyProjectTile>();
                if (projecttile)
                {
                    SetupBullets(projecttile);
                }
                tfBullet.right = direct;
                Vector3 lastPos = tfStartFire.position + (lifeTimeBullet * bulletSpeed * direct);
                tfBullet.DOMove(lastPos, lifeTimeBullet).SetEase(Ease.Linear).onComplete += () =>
                {
                    if (pool.IsSpawned(tfBullet))
                    {
                        pool.Despawn(tfBullet);
                    }
                };
            }
        }
    }
    public override void AnimationState_End(TrackEntry trackEntry)
    {
        base.AnimationState_End(trackEntry);
        FinishBehavior();
    }
}
