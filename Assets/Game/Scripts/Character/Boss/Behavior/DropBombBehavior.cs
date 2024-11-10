using DG.Tweening;
using Sirenix.OdinInspector;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DropBombBehavior : EnemyBehavior
{
    [SerializeField] protected bool isMovingAndAttack = true;
    [ShowIf("isMovingAndAttack", true)]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected Transform prefabBomb;
    [SerializeField] bool isBombAimTarget = false;
    [ShowIf("isBombAimTarget", true)]
    protected float bombSpeed = 3f;
    [SerializeField] protected float timeBombAlive = 2f;
    [SerializeField] protected float timeBulletAlive = 1.5f;
    [SerializeField] protected bool isExplosion = true;
    [ShowIf("isExplosion", true)]
    [SerializeField] protected Transform prefabBullet;
    [ShowIf("isExplosion", true)]
    [SerializeField] protected float bulletSpeed = 6f;
    protected Vector3 moveTarget;
    //const string moveId = "dropbombmovement";
    protected Vector2[] Direct8 = new Vector2[8]
    {
        new Vector2(0,-1),
        new Vector2(0,1),
        new Vector2(1,1),
        new Vector2(1,0),
        new Vector2(1,-1),
        new Vector2(-1,-1),
        new Vector2(-1,1),
        new Vector2(-1,0)
    };
    protected Vector2[] Direct16 = new Vector2[16]
    {
            new Vector2(0,-1),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0),
            new Vector2(1,-1),
            new Vector2(-1,-1),
            new Vector2(-1,1),
            new Vector2(-1,0),

            new Vector2(0,-1),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0),
            new Vector2(1,-1),
            new Vector2(-1,-1),
            new Vector2(-1,1),
            new Vector2(-1,0)
    };
    public override void Select(Action<EnemyBehaviorState> action)
    {
        base.Select(action);
        PlayAnim();


    }
    protected virtual void MoveToTarget()
    {
        var distance = Vector3.Distance(this.transform.position, moveTarget);
        float time = distance / moveSpeed;
        this.transform.DOMove(moveTarget, time).SetId(this.transform.GetInstanceID()).SetEase(Ease.Linear).onComplete += () =>
        {
            RandomTarget();
            MoveToTarget();
        };
    }
    public override void StartLongAction()
    {
        if (isMovingAndAttack)
        {
            RandomTarget();
            MoveToTarget();
        }
        base.StartLongAction();
    }
    public override void EndLongAction()
    {
        base.EndLongAction();
    }
    public override void ShortAction()
    {
        var tf = pool.Spawn(prefabBomb, this.transform.position, prefabBomb.rotation);
        var projecttile = tf.GetComponent<EnemyProjectTile>();
        if (projecttile)
        {
            SetupBullets(projecttile);
        }
        if (!isBombAimTarget)
        {
            pool.Despawn(tf, timeBombAlive);
            this.StartDelayAction(timeBombAlive, () =>
            {
                SpawnBullets(tf.position);
            });
        }
        else
        {
            var tfTarget = GameDynamicData.mainCharacter.transform;
            var distance = Vector3.Distance(this.transform.position, tf.position);
            var timeMove = distance / bombSpeed;

            tf.DOMove(tfTarget.position, timeMove).SetEase(Ease.Linear).onComplete += OnBombMoveFinish;

        }

        void OnBombMoveFinish()
        {
            pool.Despawn(tf);
            SpawnBullets(tf.position);
        }
    }



    protected virtual void SpawnBullets(Vector2 startPos)
    {
        if (prefabBullet == null || ReferenceEquals(prefabBullet, null))
        {
            return;
        }
        foreach (var direct in Direct8)
        {
            var _direct = direct.normalized;
            var tf = pool.Spawn(prefabBullet, startPos, prefabBomb.rotation);
            var endPos = startPos + (timeBulletAlive * _direct * bulletSpeed);
            tf.DOMove(endPos, timeBulletAlive).SetEase(Ease.Linear).onComplete += () =>
            {
                if (pool.IsSpawned(tf))
                {
                    pool.Despawn(tf);

                }
            };
        }
    }
    public override void FinishBehavior()
    {
        if (isSelected)
        {
            DOTween.Kill(this.transform.GetInstanceID());
        }
        base.FinishBehavior();
    }
    protected virtual void RandomTarget()
    {
        moveTarget = GameDynamicData.BirdCage.GetRandomPosInsdeCage();
        turnFaceable.TurnFaceToPoint(moveTarget);
    }
    public override void AnimationState_End(TrackEntry trackEntry)
    {
        DOTween.Kill(this.transform.GetInstanceID());
        FinishBehavior();
    }
}
