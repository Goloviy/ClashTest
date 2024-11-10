using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RocketSkill : SkillBase
{
    public GameObject prefabGrenade;
    public GameObject prefabExplorer;
    float timeRefresh = Mathf.NegativeInfinity;
    [SerializeField] private float lifeTimeBullet = 0.7f;
    [SerializeField] private float bulletSpeed = 7f;
    [SerializeField] private float bulletScaleReal;
    [SerializeField] private float bulletSpeedReal;

    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeed, this);
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
    }
    public override void FindOpponent()
    {
        //mainDirect = findOpponentSystem.GetRandomDirectionToOpponent(this.transform.position);
        mainDirect = findOpponentSystem.GetDirectNearestOpponent();
    }
    private void Update()
    {
        if (!owner.IsAlive)
            return;
        if (level <= 0)
            return;
        if (Time.time - CurTimeInterval > timeRefresh)
        {
            timeRefresh = Mathf.Infinity;
            FindOpponent();
            CreateBullets();
        }
    }
    public override void CreateBullets()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.SKILL_GRANDE_SHOOT);
        base.CreateBullets();
        if (mainDirect != Vector3.zero)
        {
            var cross = Vector3.Cross(mainDirect, Vector3.back) * 2f;
            for (int i = 1; i <= CurCountBullet; i++)
            {
                //await Task.Delay(150);

                if (i == 1 || i == 4 || i == 7)
                {
                    CreateBullet(this.transform.position, mainDirect, Vector3.zero, true);

                }
                else if (i == 2 || i == 5 || i == 8)
                {
                    CreateBullet(this.transform.position, mainDirect, cross, true);
                }
                else if (i == 3 || i == 6 || i == 9)
                {
                    CreateBullet(this.transform.position, mainDirect, cross * -1f, true);

                }
            }
        }
        timeRefresh = Time.time;

    }
    private void CreateBullet(Vector3 startPos, Vector3 direct, Vector3 lastPosCross, bool isLookatTarget)
    {
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        var tf = pool.Spawn(prefabGrenade, startPos, Quaternion.identity);
        if (isLookatTarget)
        {
            tf.transform.right = direct;
        }
        var originScale = prefabGrenade.transform.localScale;
        tf.localScale = originScale * bulletScaleReal;
        Vector3 lastPos = this.transform.position + (lifeTimeBullet * bulletSpeedReal * direct) + lastPosCross;
        tf.DOMove(lastPos,1f).SetEase(Ease.OutSine).onComplete += () =>
        {
            if (pool.IsSpawned(tf))
            {
                var tfExplosion = pool.Spawn(prefabExplorer, tf.position, Quaternion.identity);
                tfExplosion.localScale = originScale * bulletScaleReal;
                tf.localScale = originScale;
                pool.Despawn(tf);
            }

        };
    }
}
