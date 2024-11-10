using DG.Tweening;
using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFollow : MonsterBulletBase
{
    Rigidbody2D rigidbody;
    [SerializeField] Transform prefabExplosion;
    [Header("Follow Config")]
    [SerializeField] float speed = 3.5f;
    [SerializeField] float timeFollow = 2f;
    [SerializeField] float rotateSpeed = 200f;

    [Header("Active Follow Config")]
    [SerializeField] float timeMoveToActivePoint = 1f;
    [SerializeField] Vector3 offsetPosActiveFollow = new Vector3(0, 2);
    bool isActive = false;
    bool isEndFollowTime = false;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

    }
    protected override void OnSpawned(SpawnPool pool)
    {
        isEndFollowTime = false;
        isActive = false;
        target = GameDynamicData.mainCharacter.transform;
        rigidbody.simulated = true;
        this.StartDelayAction(timeFollow, OnEndTimeFollow);
        this.StartDelayAction(timeFollow + 2f, OnEndTimeLife);
        MoveToActivePoint();
    }
    private void MoveToActivePoint()
    {
        Vector3 posActivePoint = this.transform.position + offsetPosActiveFollow;
        this.transform.DOMove(posActivePoint, timeMoveToActivePoint).SetId(this.gameObject.GetInstanceID()).SetEase(Ease.Linear).onComplete += ActiveFollow;
    }

    private void ActiveFollow()
    {
        isActive = true;
    }

    protected override void OnDespawned(SpawnPool pool)
    {
        isEndFollowTime = false;
        isActive = false;
        rigidbody.simulated = false;
        DOTween.Kill(this.gameObject.GetInstanceID());
    }
    protected override void Despawn()
    {
        if (PoolManager.Pools[StringConst.POOL_MONSTER_BULLET_NAME].IsSpawned(this.transform))
        {
            PoolManager.Pools[StringConst.POOL_MONSTER_BULLET_NAME].Despawn(this.transform);
        }
    }
    protected void OnEndTimeFollow()
    {
        isEndFollowTime = true;
    }
    protected void OnEndTimeLife()
    {
        Despawn();
    }
    private void FixedUpdate()
    {
        if (!isActive)
        {
            return;
        }
        if (target == null)
        {
            target = GameDynamicData.mainCharacter.transform;
            return;
        }
        if (!isEndFollowTime)
        {
            Vector2 direction = (Vector2)target.position - this.rigidbody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rigidbody.angularVelocity = -rotateAmount * rotateSpeed;
            rigidbody.velocity = transform.up * speed;
        }
        else
        {
            rigidbody.velocity = transform.up * speed;
        }
    }
    protected override void AfterTrigger()
    {
        var tf = PoolManager.Pools[StringConst.POOL_MONSTER_BULLET_NAME].Spawn(prefabExplosion);
    }
}
