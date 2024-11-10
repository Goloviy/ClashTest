using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBombBase : EnemyProjectTile
{
    protected AttackData attackData;
    protected bool isSpawn = false;
    [SerializeField] bool isExplosionAfterTrigger = true;
    EnemyBehavior enemyBehavior;
    public void Init(EnemyBehavior enemyBehavior, Vector3 targetPos, float lifeTime)
    {
        this.enemyBehavior = enemyBehavior;
        this.transform.DOMove(targetPos, lifeTime).SetEase(Ease.Linear).onComplete += OnFinishMOve;
    }

    private void OnFinishMOve()
    {
        if (enemyBehavior)
        {
            enemyBehavior.Explosion(this.transform.position);
        }
        Despawn();
    }

    protected virtual void OnSpawned(SpawnPool pool)
    {
        isSpawn = true;
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

        var eAtk = SpawnerMananger.Instance.GetAttack(OwnerId);
        eAtk = Mathf.RoundToInt(eAtk * multiplyDamge);
        //attackData = new AttackData(null, eAtk, 0);
        GameDynamicData.mainCharacter.TakeDamage(null, eAtk, 0);
        if (isExplosionAfterTrigger)
        {
            Despawn();
            if (enemyBehavior)
            {
                enemyBehavior.Explosion(this.transform.position);
            }
        }
    }
}
