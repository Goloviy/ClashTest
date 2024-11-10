using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShoot1 : MonsterBase
{
    [SerializeField] Transform prefabBullet;
    [SerializeField] float bulletSpeed = 3f;
    [SerializeField] float bulletTimeLife = 1f;
    float timeRefreshShoot = 5f;
    [SerializeField] float timeIntervalShoot = 3f;
    bool isShoot = false;
    Transform tfBullet;
    protected override void OnSpawned(SpawnPool pool)
    {
        base.OnSpawned(pool);
        isShoot = true;
    }
    protected override void OnDespawned(SpawnPool pool)
    {
        DisableBullet();
        isShoot = false;
        base.OnDespawned(pool);
    }
    private void DisableBullet()
    {
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        if (pool.IsSpawned(tfBullet))
        {
            pool.Despawn(tfBullet);
        }
        DOTween.Kill(this.gameObject.GetInstanceID());
    }
    protected override void Update()
    {
        base.Update();
        if (target && isShoot)
        {
            if (Time.time - (timeIntervalShoot + bulletTimeLife) > timeRefreshShoot)
            {
                timeRefreshShoot = Time.time;
                Shoot();
            }
        }
    }
    protected void Shoot()
    {
        //moveDirect = (target.transform.position - this.transform.position).normalized;
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        tfBullet = pool.Spawn(prefabBullet, this.transform.position, Quaternion.identity);
        tfBullet.right = moveDirect;
        Vector3 endPos = this.transform.position + bulletSpeed * bulletTimeLife * moveDirect;
        tfBullet.DOMove(endPos, bulletTimeLife).SetEase(Ease.Linear).SetId(this.gameObject.GetInstanceID()).onComplete += () =>
        {
            DisableBullet();
        };
    }

}
