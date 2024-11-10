using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkillKnife : SkillBase, ICoolDown
{
    float timeRefresh = Mathf.NegativeInfinity;
    public List<GameObject> ListBulletByLevel;
    public GameObject CurPrefabBullet => ListBulletByLevel[level - 1];

    public float TimeRemainning { get ; set ; }
    public float TimeCoolDown { get ; set ; }
    public bool IsEnable { get; set; }

    public float lifeTimeBullet = 1f;
    public float bulletSpeedOrigin = 8f;
    float bulletSpeedReal = 8f;
    float bulletScaleReal = 1f;
    [SerializeField] int numberSpread = 1;
    public override void FindOpponent()
    {
        base.FindOpponent();
        mainDirect = findOpponentSystem.GetMainDirect();
    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeedOrigin, this);
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
        base.OnLevelupAnySkill(arg1, arg2);
        TimeCoolDown = CurTimeInterval;
        TimeRemainning = 0f;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        IsEnable = false;
    }
    public void Update()
    {
        if (!owner.IsAlive)
            return;
        if (level >= 1)
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
        IsEnable = true;
        base.LevelUpChange();
    }
    public override async void CreateBullets()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.SKILL_CROSSBOW);
        base.CreateBullets();
        if (mainDirect != Vector3.zero)
        {
            int countBullet = CurCountBullet;

            Vector3 cross = Vector3.Cross(mainDirect, Vector3.back) * 0.7f;
            for (int j = 0; j < numberSpread; j++)
            {
                int countUp = 0;
                int coutnDown = 0;
                for (int i = 0; i < countBullet; i++)
                {
                    if (i == 0)
                    {
                        //tao vien dan o trung tam
                        CreateBullet(this.transform.position, mainDirect, Vector3.zero);
                    }
                    else if (i % 2 == 0)
                    {
                        //tao vien dan ben duoi
                        CreateBullet(this.transform.position, mainDirect, ++coutnDown * cross);

                    }
                    else
                    {
                        await Task.Delay(40);
                        //tao vien dan ben tren
                        CreateBullet(this.transform.position, mainDirect, -1f * cross * ++countUp);

                    }
                    var direct = mainDirect;
                }
                if (numberSpread > 1)
                {
                    await Task.Delay(200);
                }
            }

        }
        timeRefresh = Time.time;
    }
    private void CreateBullet(Vector3 startPos, Vector3 direct, Vector3 cross)
    {
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        var tf = pool.Spawn(CurPrefabBullet, startPos, Quaternion.identity);
        var originScale = CurPrefabBullet.transform.localScale;
        tf.localScale = originScale * bulletScaleReal;
        Vector3 lastPos = this.transform.position + (lifeTimeBullet * bulletSpeedReal * direct) + cross;
        tf.right = this.transform.position - lastPos;
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
