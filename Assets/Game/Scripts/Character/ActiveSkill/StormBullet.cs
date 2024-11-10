using PathologicalGames;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBullet : BulletBase
{
    //[SerializeField] Transform prefabExplosion;
    Vector3 direct = Vector3.zero;
    //bool isTrigger;

    float lastTimeReflect = 0f;
    float cdReflect = 0.2f;

    EdgeScreenType lastEdge = EdgeScreenType.NONE;
    private float bulletSpeedReal;
    //private float bulletSizeReal;
    Action onRespawn;
    SkillStorm skillStorm;
    float removeDistance = 16f;
    //SpriteRenderer spriteRenderer;
    protected override void Awake()
    {
        base.Awake();
        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }
    //private void TurnRenderer(bool isLeft)
    //{
    //    spriteRenderer.flipX = !isLeft;
    //}
    public override void Init(SkillBase owner)
    {
        skillStorm = owner as SkillStorm;
        base.Init(owner);
        direct = owner.mainDirect;
        TurnByDirect();
        UpdateBulletData();
    }
    private void TurnByDirect()
    {
        //if (direct.x > 0)
        //{
        //    TurnRenderer(false);
        //}
        //else
        //{
        //    TurnRenderer(true);
        //}
    }
    private void UpdateBulletData()
    {
        bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(speed, owner);
    }

    private void OnAfterSkillup(Component arg1, object arg2)
    {
        UpdateBulletData();
    }
    protected override void OnSpawned(SpawnPool pool)
    {
        base.OnSpawned(pool);
        EventDispatcher.Instance.RegisterListener(EventID.SKILL_LEVEL_UP_AFTER, OnAfterSkillup);

    }
    protected override void ScaleBySkill()
    {
    }
    protected override void OnDespawned(SpawnPool pool)
    {
        base.OnDespawned(pool);
        EventDispatcher.Instance.RemoveListener(EventID.SKILL_LEVEL_UP_AFTER, OnAfterSkillup);

    }
    private void FixedUpdate()
    {
        CheckReflect();
        Move();

    }
    private void CheckReflect()
    {
        if (Time.time - cdReflect < lastTimeReflect)
            return;
        float deltaX = Mathf.Abs(GameDynamicData.mainCharacter.transform.position.x - this.transform.position.x);
        float deltaY = Mathf.Abs(GameDynamicData.mainCharacter.transform.position.y - this.transform.position.y);
        if (removeDistance < deltaX + deltaY && skillStorm)
        {
            skillStorm.CreateStorms();
            return;
        }
        var checkEdge = this.transform.CheckTriggerEdgeScreen();
        if (lastEdge == checkEdge)
        {
            return;
        }
        switch (checkEdge)
        {
            case EdgeScreenType.NONE:
                break;
            case EdgeScreenType.TOP:
                direct = Vector3.Reflect(direct, Vector3.up);
                break;
            case EdgeScreenType.LEFT:
                direct = Vector3.Reflect(direct, Vector3.right);
                //TurnRenderer(false);
                break;
            case EdgeScreenType.RIGHT:
                direct = Vector3.Reflect(direct, Vector3.right);
                //TurnRenderer(true);
                break;
            case EdgeScreenType.BOTTOM:
                direct = Vector3.Reflect(direct, Vector3.up);
                break;
            default:
                break;
        }
        if (checkEdge != EdgeScreenType.NONE)
        {
            Explosion();
            lastTimeReflect = Time.time;
            if (Vector3.Angle(direct, Vector3.right) < 15f)
            {
                direct = Vector3.Slerp(direct, Vector3.up, 0.5f);
            }
            else if (Vector3.Angle(direct, Vector3.up) < 15f)
            {
                direct = Vector3.Slerp(direct, Vector3.right, 0.5f);
            }
            lastEdge = checkEdge;
            //this.transform.right = direct - Vector3.zero;
        }
    }

    private void Move()
    {
        if (direct != Vector3.zero)
        {
            this.transform.MoveTransformWithPhysic(direct, bulletSpeedReal);
        }
        else
        {
            direct = Vector3.left;
        }
    }
    private void Explosion()
    {
        //if (!isExplosion)
        //{
        //    return;
        //}
        //var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        //pool.Spawn(prefabExplosion, this.transform.position, Quaternion.identity);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }
    protected override void DespawnThis()
    {
        //isTrigger = false;
        base.DespawnThis();
    }
}
