using PathologicalGames;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBullet : BulletBase
{
    [SerializeField] protected  bool isExplosion;
    [ShowIf("isExplosion", true)]
    [SerializeField] protected Transform prefabExplosion;
    protected Vector3 direct = Vector3.zero;
    public float lifeTime = 5f;
    public float lifeStartTime;
    protected bool isTrigger;

    protected float lastTimeReflect = 0f;
    protected float cdReflect = 0.1f;

    protected EdgeScreenType lastEdge = EdgeScreenType.NONE;
    public override void Init(SkillBase owner)
    {
        base.Init(owner);
        direct = owner.mainDirect;
        lifeStartTime = Time.time;
        this.transform.right = direct - Vector3.zero;

    }
    protected void FixedUpdate()
    {
        CheckReflect();
        Move();

    }
    protected virtual void CheckReflect()
    {
        if (Time.time - cdReflect < lastTimeReflect)
            return;

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
                break;
            case EdgeScreenType.RIGHT:
                direct = Vector3.Reflect(direct, Vector3.right);
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
            if ((checkEdge == EdgeScreenType.LEFT || checkEdge == EdgeScreenType.RIGHT) && Vector3.Angle(direct, Vector3.right) < 15f)
            {
                direct = Vector3.Lerp(direct, Vector3.up, 0.5f);
            }
            if ((checkEdge == EdgeScreenType.TOP || checkEdge == EdgeScreenType.BOTTOM) && Vector3.Angle(direct, Vector3.up) < 15f)
            {
                direct = Vector3.Lerp(direct, Vector3.right, 0.5f);
            }
            lastEdge = checkEdge;
            this.transform.right = direct - Vector3.zero;
        }
    }
    protected void Update()
    {
        LifeTimeChecker();
    }
    protected void LifeTimeChecker()
    {
        if (Time.time - lifeStartTime > lifeTime)
        {
            DespawnThis();
        }
    }
    protected void Move()
    {
        if (direct != Vector3.zero)
        {
            this.transform.MoveTransformWithPhysic(direct, curSpeed);
        }
    }
    protected void Explosion()
    {
        if (!isExplosion)
        {
            return;
        }
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        pool.Spawn(prefabExplosion, this.transform.position, Quaternion.identity);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }
    protected override void DespawnThis()
    {
        isTrigger = false;
        base.DespawnThis();
    }
}
