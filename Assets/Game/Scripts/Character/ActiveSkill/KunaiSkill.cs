using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class KunaiSkill : SkillBase, ICoolDown
{
    float timeRefresh = Mathf.NegativeInfinity;
    public List<GameObject> ListBulletByLevel;
    public GameObject CurPrefabBullet => ListBulletByLevel[level - 1];

    public float TimeRemainning { get; set; }
    public float TimeCoolDown { get; set; }
    public bool IsEnable { get; set; }

    public float lifeTimeBullet = 1f;
    public float bulletSpeedOrigin = 8f;
    float bulletSpeedReal = 8f;

    Vector3 lastTargetPos = Vector3.zero;
    public override void FindOpponent()
    {
        mainDirect = findOpponentSystem.GetDirectNearestOpponent();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        IsEnable = false;
    }
    public void Update()
    {
        if (level < 1 || !owner.IsAlive)
            return;
        else
        {
            float timeInvertal = CurTimeInterval;
            TimeRemainning = Time.time - timeRefresh;
            if (Time.time - timeRefresh > timeInvertal)
            {
                timeRefresh = Mathf.Infinity;
                FindOpponent();
                CreateBullets();
            }
        }
    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        base.OnLevelupAnySkill(arg1, arg2);
        TimeCoolDown = CurTimeInterval;
        TimeRemainning = 0f;
    }
    public override void LevelUpChange()
    {
        base.LevelUpChange();
        IsEnable = true;
        bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeedOrigin, this);
    }
    public override async void CreateBullets()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.SKILL_PISTOL_SHOOT);
        base.CreateBullets();
        if (mainDirect != Vector3.zero)
        {
            int countBullet = CurCountBullet;
            for (int i = 0; i < countBullet; i++)
            {
                var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
                var tf = pool.Spawn(CurPrefabBullet, this.transform.position, Quaternion.identity);
                var originScale = tf.localScale;
                tf.localScale = originScale * CharacterStatusHelper.CalculateBulletScale(this);
                tf.right = mainDirect - Vector3.zero;
                Vector3 lastPos = this.transform.position + (lifeTimeBullet * bulletSpeedReal * mainDirect);
                tf.DOMove(lastPos, lifeTimeBullet).SetEase(Ease.Linear).onComplete += () =>
                {
                    if (pool.IsSpawned(tf))
                    {
                        tf.localScale = originScale;
                        pool.Despawn(tf);
                    }

                };
                await Task.Delay(100);
            }
        }

        timeRefresh = Time.time;
    }
}
