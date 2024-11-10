using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DrillSkill : SkillBase
{
    float timeRefresh = 5;
    public GameObject[] prefabBullets;
    Transform[] bullets = new Transform[0];
    bool isFirstShoot = true;
    GameObject PrefabBullet => prefabBullets[level - 1];
    public override void FindOpponent()
    {
        base.FindOpponent();
        mainDirect = findOpponentSystem.GetRandomDirectionToOpponent(this.transform.position);
        if (mainDirect == Vector3.zero)
        {
            mainDirect = Vector2.one;
        }
    }
    public void Update()
    {
        if (!owner.IsAlive)
            return;
        if (level >= 1)
        {
            float timeInvertal = isFirstShoot ? 0 : CurTimeInterval;
            if (Time.time - timeRefresh > timeInvertal)
            {
                if (isFirstShoot)
                {
                    isFirstShoot = false;
                }
                timeRefresh = Mathf.Infinity;
                FindOpponent();
                CreateBullets();
            }
        }
    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        //bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeedOrigin, this);
        //bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
    }
    public override void LevelUp()
    {
        base.LevelUp();
        CreateBullets();
    }
    protected void ClearOldBullet()
    {
        foreach (var bullet in bullets)
        {
            if (bullet != null && PoolManager.Pools[StringConst.POOL_BULLET_NAME].IsSpawned(bullet))
            {
                PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(bullet);
            }
        }
        Array.Clear(bullets, 0, bullets.Length);
    }
    public override void CreateBullets()
    {
        ClearOldBullet();
        base.CreateBullets();
        if (mainDirect != Vector3.zero && level <= prefabBullets.Length && level > 0)
        {
            int countBullet = CurCountBullet;
            bullets = new Transform[countBullet];
            for (int i = 0; i < countBullet; i++)
            {
                FindOpponent();
                var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
                var tf = pool.Spawn(PrefabBullet, this.transform.position, Quaternion.identity);
                var bullet = tf.GetComponent<BulletBase>();
                //bullet.transform.position = this.transform.position;

                bullet.Init(this);
                bullets[i] = bullet.transform;
            }
        }
        timeRefresh = Time.time;
    }
}
