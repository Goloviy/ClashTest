using DG.Tweening;
using PathologicalGames;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkillYoyo : SkillBase
{
    public GameObject prefabBullet;
    public bool isExplosion;
    [ShowIf("isExplosion", true)]
    public GameObject prefabExplosion;
    Vector3 rdPos;
    float timeRefresh = 5f;
    private float bulletSpeed = 6f;
    private float bulletSpeedReal;
    private float bulletScaleReal;

    List<Transform> bulletsReturn;
    
    List<Transform> bullets;
    float lastTimeBurn = 0f;
    float burnTimeInterval = 0.2f;
    private void Awake()
    {
        bulletsReturn = new List<Transform>();
        bullets = new List<Transform>();
    }
    protected override void OnDisable()
    {
        RemoveBullets();
        base.OnDisable();
    }
    public override void FindOpponent()
    {
        base.FindOpponent();
        float distance = 0f;
        do
        {
            rdPos = findOpponentSystem.GetRandomPositionInScreen();
            distance = Vector2.Distance(this.transform.position, rdPos);
        } while (distance < 2.25f);
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
                FindOpponent();
                CreateBullets();
            }
        }
        if (bulletsReturn.Count > 0)
        {
            //bullet return
            List<int> removeIndex = new List<int>();
            bool isBurn = false;
            for (int i = 0; i < bulletsReturn.Count; i++)
            {

                var bullet = bulletsReturn[i];
                var direct = (this.transform.position - bullet.position).normalized;
                bullet.position = bullet.position + (direct * bulletSpeedReal * Time.deltaTime);
                if (Vector3.Distance(bullet.position, this.transform.position) < 0.5f)
                {
                    removeIndex.Add(i);
                }
                if (isExplosion && Time.time - lastTimeBurn > burnTimeInterval)
                {
                    PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(prefabExplosion, bullet.position, Quaternion.identity);
                    isBurn = true;
                }
            }
            if (isBurn)
                lastTimeBurn = Time.time;

            if (removeIndex.Count > 0)
            {
                foreach (var index in removeIndex)
                {
                    var tf = bulletsReturn[index];
                    PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(tf);
                    bulletsReturn.RemoveAt(index);
                }
            }
            if (bulletsReturn.Count <= 0)
                timeRefresh = Time.time;
        }

    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeed, this);
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
    }
    public override async void CreateBullets()
    {

        int countBullet = CurCountBullet;
        for (int i = 0; i < countBullet; i++)
        {
            FindOpponent();
            var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
            var tf = pool.Spawn(prefabBullet, this.transform.position, Quaternion.identity);
            bullets.Add(tf);
            var originScale = prefabBullet.transform.localScale;
            tf.localScale = originScale * bulletScaleReal;
            bullets.Add(tf);
            float timeMove =  Vector3.Distance(rdPos, this.transform.position) / bulletSpeedReal;
            //tf.right = rdPos - this.transform.position;
            tf.DOMove(rdPos, timeMove / 1.1f).SetEase(Ease.OutSine).onComplete += () =>
            {
                bulletsReturn.Add(tf);
            };
            await Task.Delay(80);
        }
    }
    private void RemoveBullets()
    {
        foreach (var bullet in bullets)
        {
            //PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(bullet);
            PoolManager.Pools[StringConst.POOL_BULLET_NAME].Remove(bullet);
        }
        bullets.Clear();
        foreach (var bullet in bulletsReturn)
        {
            PoolManager.Pools[StringConst.POOL_BULLET_NAME].Remove(bullet);
        }
        bulletsReturn.Clear();
    }
    public override void SetDisable()
    {
        base.SetDisable();
    }
}
