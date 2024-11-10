using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBaseNew : MonoBehaviour
{
    protected SkillName skillName;
    protected Collider2D collider;

    bool isSpawn = false;
    protected virtual void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }
    protected virtual void OnSpawned(SpawnPool pool)
    {
        collider.enabled = true;
        isSpawn = true;
    }
    protected virtual void OnDespawned(SpawnPool pool)
    {
        collider.enabled = false;
        isSpawn = false;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
    }
    protected virtual void DespawnThis()
    {
        if (isSpawn)
        {
            if (PoolManager.Pools[StringConst.POOL_BULLET_NAME].IsSpawned(this.transform))
            {
                this.transform.localScale = Vector3.one;
                PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(this.transform);
            }
        }
    }
}
