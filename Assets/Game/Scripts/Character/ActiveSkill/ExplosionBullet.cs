using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : BulletBaseNew
{
    [SerializeField] Transform prefabExplosion;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        pool.Spawn(prefabExplosion, this.transform.position, Quaternion.identity);

        DespawnThis();
    }
    protected override void DespawnThis()
    {
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        if (pool.IsSpawned(this.transform))
        {
            base.DespawnThis();
        }
    }
}
