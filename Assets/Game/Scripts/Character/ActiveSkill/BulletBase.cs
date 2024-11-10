using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [HideInInspector] public SkillBase owner = null;
    public float speed = 5f;
    protected float curSpeed;
    protected bool isActive = false;
    protected Vector3 originScale;
    protected Collider2D collider;
    //Rigidbody2D rigid;
    protected virtual void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        //rigid = GetComponent<Rigidbody2D>();
        //rigid.simulated = false;
    }
    protected virtual void OnSpawned(SpawnPool pool)
    {
        //Init();
        collider.enabled = true;
        //rigid.simulated = true;
    }
    protected virtual void OnDespawned(SpawnPool pool)
    {
        collider.enabled = false;
        //rigid.simulated = false;

    }
    public virtual void Init(SkillBase owner)
    {
        this.owner = owner;
        //curSpeed = speed;
        curSpeed = CharacterStatusHelper.CalculateBulletMoveSpeed(speed, owner);
        ScaleBySkill();
        isActive = true;
    }
    protected virtual void ScaleBySkill()
    {
        originScale = this.transform.localScale;
        this.transform.localScale = originScale * CharacterStatusHelper.CalculateBulletScale(owner);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //var opponent = collision.GetComponent<UnitBase>();
        //if (owner != null && opponent != null && opponent.IsAlive)
        //{
        //    owner.InflictDamage(opponent);
                DespawnThis();
        //}
    }
    protected virtual void DespawnThis()
    {
        if (!isActive)
            return;
        isActive = false;
        PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(this.transform);
        this.transform.localScale = originScale;
        curSpeed = speed;
    }
}
