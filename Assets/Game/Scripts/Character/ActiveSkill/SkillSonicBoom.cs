using DG.Tweening;
using PathologicalGames;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkillSonicBoom : SkillBase
{
    float timeRefresh = 5;
    public Transform prefabBoom;
    [SerializeField] bool isSpawnBullet;
    [ShowIf("isSpawnBullet", true)]
    public Transform prefabBullet;
    //[ShowIf("isSpawnBullet", true)]
    //[SerializeField] float bulletSpeed = 6f;
    //float bulletSpeedReal ;
    float bulletScaleReal ;
    public override void FindOpponent()
    {
        base.FindOpponent();
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
        //bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeed, this);
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
    }
    public override void LevelUpChange()
    {
        base.LevelUpChange();
    }
    public override void CreateBullets()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.SKILL_QUAKE);
        CreateBoom();
    }
    private async void CreateBoom()
    {
        int count = CurCountBullet;
        for (int i = 0; i < count; i++)
        {
            var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
            var tf = pool.Spawn(prefabBoom, this.transform.position, Quaternion.identity);
            var originScale = prefabBoom.transform.localScale;
            tf.localScale = originScale * bulletScaleReal;
            if (isSpawnBullet)
            {
                CreateBullet();
            }
            await Task.Delay(350);
        }
        timeRefresh = Time.time;


    }
    void CreateBullet()
    {
        var tfBullet = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(prefabBullet, this.transform.position, Quaternion.identity);
        var tfBullet2 = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(prefabBullet, this.transform.position, Quaternion.identity);

        var endpos = this.transform.position + Vector3.up * 10;
        var endpos2 = this.transform.position + Vector3.down * 10; 
        tfBullet.right = endpos - this.transform.position;
        tfBullet2.right = endpos2 - this.transform.position;
        var originScale = prefabBullet.transform.localScale;
        tfBullet.localScale = originScale * bulletScaleReal;
        tfBullet2.localScale = originScale * bulletScaleReal;
        tfBullet.DOMove(endpos, 1.5f).onComplete += () =>
        {
            PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(tfBullet);
        };
        tfBullet2.DOMove(endpos2, 1.5f).onComplete += () =>
        {
            PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(tfBullet2);
        };
    }
    //private void CreateBullet(Vector3 startPos, Vector3 direct, Vector3 cross)
    //{
    //    var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
    //    var tf = pool.Spawn(curPrefabBoom, startPos, Quaternion.identity);
    //    var originScale = curPrefabBoom.transform.localScale;
    //    tf.localScale = originScale * bulletScaleReal;
    //    Vector3 lastPos = this.transform.position + (lifeTimeBullet * bulletSpeedReal * direct) + cross;
    //    //tf.right = direct;
    //    tf.DOMove(lastPos, lifeTimeBullet).SetEase(Ease.Linear).onComplete += () =>
    //    {
    //        if (pool.IsSpawned(tf))
    //        {
    //            tf.localScale = originScale;
    //            pool.Despawn(tf);
    //        }

    //    };
    //}
}
