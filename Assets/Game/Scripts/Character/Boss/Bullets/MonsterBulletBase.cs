using PathologicalGames;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class MonsterBulletBase : EnemyProjectTile
{
    [Title("Mechanims")]
    [Header("Default lifetime = -1 ( if lifetime < 0 disable this property)")]
    [SerializeField] float lifeTime = -1f;
    [SerializeField] public bool isDamageContinious = false;
    [ShowIf("isDamageContinious", true)]
    public float timeInterval = 0.33f;
    private float timeRefresh = 0f;
    protected AttackData attackData;
    protected Transform target;
    protected bool isSpawn = false;
    [SerializeField] bool isDestroyAfterTrigger = true;
    [Title("Collider Config")]
    [SerializeField] Collider2D[] colliders;
    [SerializeField] bool isDelayActiveCollider = false;
    public float delayActiveTime = 0f;
    [SerializeField] bool isDelayDeactiveCollider = false;
    public float delayDeactiveTime = 0f;

    protected virtual void SetActiveColliders(bool isActive)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].gameObject.SetActive(isActive);
        }
    }
    protected virtual void OnSpawned(SpawnPool pool)
    {
        HandleLifeTime();
        HandleStatusCollider();
        target = GameDynamicData.mainCharacter.transform;
        isSpawn = true;
    }
    protected virtual void HandleStatusCollider()
    {
        if (isDelayActiveCollider)
        {
            SetActiveColliders(false);
            this.StartDelayAction(delayActiveTime, () =>
            {
                SetActiveColliders(true);
            });
        }
        if (isDelayDeactiveCollider)
        {
            this.StartDelayAction(delayDeactiveTime, () =>
            {
                SetActiveColliders(false);
            });
        }
    }
    protected virtual void HandleLifeTime()
    {
        if (lifeTime > 0)
        {
            this.StartDelayAction(lifeTime, () =>
            {
                Despawn();
            });
        }

    }
    protected virtual void OnDespawned(SpawnPool pool)
    {
        isSpawn = false;
    }
    protected virtual void Despawn()
    {
        if (PoolManager.Pools[StringConst.POOL_MONSTER_BULLET_NAME].IsSpawned(this.transform))
        {
            PoolManager.Pools[StringConst.POOL_MONSTER_BULLET_NAME].Despawn(this.transform);

        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == StringConst.LAYER_AGAINS_BULLET)
        {
            if (isDestroyAfterTrigger)
            {
                Despawn();
            }
        }
        else
        {
            var eAtk = SpawnerMananger.Instance.GetAttack(OwnerId);
            eAtk = Mathf.RoundToInt(eAtk * multiplyDamge);
            //attackData = new AttackData(null, eAtk, 0);
            GameDynamicData.mainCharacter.TakeDamage(null, eAtk, 0);
            if (isDestroyAfterTrigger)
            {
                Despawn();
                AfterTrigger();
            }
        }

    }
    protected virtual void AfterTrigger()
    {

    }
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (isDamageContinious)
        {
            if (Time.time - timeInterval > timeRefresh)
            {
                timeRefresh = Time.time;
                var eAtk = SpawnerMananger.Instance.GetAttack(OwnerId);
                eAtk = Mathf.RoundToInt(eAtk * multiplyDamge);
                //attackData = new AttackData(null, eAtk, 0);
                GameDynamicData.mainCharacter.TakeDamage(null, eAtk, 0);
            }

        }
    }
}
