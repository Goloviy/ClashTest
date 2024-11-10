using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillClaw : SkillBase, ICoolDown
{
    [SerializeField] Transform prefabBullet;
    [SerializeField] float lifeTimeBullet = 0.4f;
    float timeRefresh = 0f;
    [SerializeField] float distance = 1.5f;
    private float bulletScaleReal = 1f;

    public float TimeRemainning { get; set; }
    public float TimeCoolDown { get; set; }
    public bool IsEnable { get; set; }

    public override void FindOpponent()
    {
        mainDirect = findOpponentSystem.GetMainDirect();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        IsEnable = false;
    }
    public override void LevelUpChange()
    {
        base.LevelUpChange();
        IsEnable = true;
    }
    
    private void Update()
    {

        if (level <= 0)
        {
            return;
        }
        else
        {
            TimeRemainning = Time.time - timeRefresh;
            if (Time.time - CurTimeInterval > timeRefresh)
            {
                timeRefresh = Mathf.Infinity;
                FindOpponent();
                CreateBullets();
            }

        }

    }
    public override void CreateBullets()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.SKILL_CLAW);
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        var posSpawn = distance * bulletScaleReal * mainDirect + this.transform.position;
        var tf = pool.Spawn(prefabBullet, posSpawn, prefabBullet.rotation);
        tf.up = mainDirect;
        tf.localScale = prefabBullet.localScale * bulletScaleReal;
        this.StartDelayAction(lifeTimeBullet, () =>
        {
            timeRefresh = Time.time;
            if (pool.IsSpawned(tf))
            {
                pool.Despawn(tf);
            }
        });
    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        base.OnLevelupAnySkill(arg1, arg2);
        if (isDisable)
            return;
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
        TimeCoolDown = CurTimeInterval;
    }
}
