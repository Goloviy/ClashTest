using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOfBoss : MonoBehaviour
{
    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.BOSS_DIE, OnBossDie);

    }
    protected void OnSpawned(SpawnPool pool)
    {

    }
    protected void OnDespawned(SpawnPool pool)
    {
        //EventDispatcher.Instance.RemoveListener(EventID.BOSS_DIE, OnBossDie);

    }
    private void OnBossDie(Component arg1, object arg2)
    {
        var poolBullet = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        if (poolBullet.IsSpawned(this.transform))
        {
            poolBullet.Despawn(this.transform);
        }
    }

}
