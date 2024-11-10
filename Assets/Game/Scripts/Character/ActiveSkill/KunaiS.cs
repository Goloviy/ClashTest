using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class KunaiS : SkillBase, ICoolDown
{
    float timeRefresh = Mathf.NegativeInfinity;
    public List<GameObject> ListBulletByLevel;
    public GameObject CurPrefabBullet => ListBulletByLevel[level - 1];
    public float lifeTimeBullet = 1f;
    public float bulletSpeedOrigin = 8f;
    float bulletSpeedReal = 8f;
    public float TimeRemainning { get; set; }
    public float TimeCoolDown { get; set; }
    public bool IsEnable { get; set; }
    protected override void OnDisable()
    {
        base.OnDisable();
        IsEnable = false;
    }
    public override void FindOpponent()
    {
        base.FindOpponent();
        mainDirect = findOpponentSystem.GetDirectNearestOpponent();
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
    public override void LevelUpChange()
    {
        base.LevelUpChange();
        IsEnable = true;
        bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeedOrigin, this);
    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        base.OnLevelupAnySkill(arg1, arg2);
        TimeCoolDown = CurTimeInterval;
        TimeRemainning = 0f;
    }
    public override async void CreateBullets()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.SKILL_PISTOL_SHOOT);
        base.CreateBullets();
        if (mainDirect != Vector3.zero)
        {
            int countBullet = CurCountBullet;

            Vector3 cross = Vector3.Cross(mainDirect, Vector3.back) * 0.4f;

            for (int i = 0; i < countBullet; i++)
            {
                var direct = mainDirect;
                CreateBullet(this.transform.position, direct, Vector3.zero);
                var pos2 = this.transform.position + cross;
                CreateBullet(pos2, direct, cross);
                await Task.Delay(120);
            }
        }
        timeRefresh = Time.time;
    }
    private void CreateBullet(Vector3 startPos, Vector3 direct, Vector3 cross)
    {
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        var tf = pool.Spawn(CurPrefabBullet, startPos, Quaternion.identity);
        var originScale = tf.localScale;
        tf.localScale = originScale * CharacterStatusHelper.CalculateBulletScale(this);
        tf.right = direct;
        Vector3 lastPos = this.transform.position + (lifeTimeBullet * bulletSpeedReal * mainDirect) + cross;
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
