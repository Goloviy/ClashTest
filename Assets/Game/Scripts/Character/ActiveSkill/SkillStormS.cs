using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStormS : SkillBase
{
    float timeRefresh = 5f;
    public Transform prefabBullet;
    public float lifeTimeBullet = 1f;
    public float bulletSpeedOrigin = 5.5f;
    float bulletSpeedReal = 6;
    float bulletScaleReal = 1f;
    Vector2[] listDirect = new Vector2[8]
    {
        new Vector2(0,1),
        new Vector2(0,-1),
        new Vector2(1,1),
        new Vector2(1,0),
        new Vector2(1,-1),
        new Vector2(-1,1),
        new Vector2(-1,0),
        new Vector2(-1,-1),
    };
    private void Awake()
    {
        bulletSpeedReal = bulletSpeedOrigin;
            }
    public void Update()
    {
        if (!owner.IsAlive)
            return;
        if (level >= 1)
        {
            float timeInvertal = CurTimeInterval;
            if (Time.time - timeRefresh > timeInvertal)
            {
                timeRefresh = Mathf.Infinity;
                CreateBullets();
            }
        }
    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeedOrigin, this);
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
    }

    public override void CreateBullets()
    {
        base.CreateBullets();

        for (int i = 0; i < listDirect.Length; i++)
        {
            var direct = listDirect[i].normalized;
            CreateBullet(this.transform.position, direct);
        }
        timeRefresh = Time.time;
    }
    private void CreateBullet(Vector3 startPos, Vector3 direct)
    {
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        var tf = pool.Spawn(prefabBullet, startPos, Quaternion.identity);
        var originScale = prefabBullet.transform.localScale;
        tf.localScale = originScale * bulletScaleReal;
        Vector3 lastPos = this.transform.position + (lifeTimeBullet * bulletSpeedReal * direct);
        tf.DOMove(lastPos, lifeTimeBullet).SetEase(Ease.Linear).onComplete += () =>
        {
            if (pool.IsSpawned(tf))
            {
                tf.localScale = originScale;
                pool.Despawn(tf);
            }
        };
    }
}
