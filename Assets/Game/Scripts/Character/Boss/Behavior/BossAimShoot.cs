using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BossAimShoot : EnemyBehavior
{
    [Tooltip("Random position or select player")]
    [SerializeField] bool isTargetRandom = false; 
    [SerializeField] Transform prefabMark;
    [SerializeField] MonsterBombBase prefabBullet;
    [Tooltip("Spawn count bullet per action")]
    [SerializeField] int bulletCount = 1;
    [SerializeField] float timeExplosion = 0.4f;
    [SerializeField] Transform prefabExplosion;
    [SerializeField] protected float bulletSpeed = 6f;
    public override void Select(Action<EnemyBehaviorState> action)
    {
        base.Select(action);
        PlayAnim();
    }
    public override void ShortAction()
    {
        Shoot();
    }
    private async void Shoot()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 targetPos = isTargetRandom ?
                CameraFollower.Instance.RandomInsideCam() :
                GameDynamicData.mainCharacter.transform.position;
            var mark = pool.Spawn(prefabMark.transform,
                targetPos,
                prefabBullet.transform.rotation);
            await Task.Delay(100);
            var tf = pool.Spawn(prefabBullet.transform,
                this.transform.position,
                prefabBullet.transform.rotation);
            MonsterBombBase bomb = tf.GetComponent<MonsterBombBase>();
            float distance = Vector3.Distance(targetPos, this.transform.position);
            float timeBulletAlive = distance / bulletSpeed;
            bomb.Init(this, targetPos, timeBulletAlive);
            this.StartDelayAction(timeBulletAlive, () =>
            {
                if (pool.IsSpawned(mark))
                {
                    pool.Despawn(mark, timeBulletAlive);
                }
            });
            var projecttile = bomb.GetComponent<EnemyProjectTile>();
            if (projecttile)
            {
                SetupBullets(projecttile);
            }
        }
    }

    public override void Explosion(Vector3 posTarget)
    {
        var explosion = pool.Spawn(prefabExplosion, posTarget, Quaternion.identity);
        this.StartDelayAction(timeExplosion, () =>
        {
            if (pool.IsSpawned(explosion))
            {
                pool.Despawn(explosion);
            }
        });
    }
    public override void AnimationState_End(TrackEntry trackEntry)
    {
        FinishBehavior();
    }
}
