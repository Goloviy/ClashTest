using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class KatanaSkill : SkillBase, ICoolDown
{
    public Transform goPrefabBullet;
    public Transform goPrefabSubBullet;
    //public float bulletSpeed = 7f;
    [HideInInspector] float bulletSpeedReal;
    public float RefreshTime = 0f;
    public float lifeTime = 0.32f;
    public float TimeRemainning { get; set; }
    public float TimeCoolDown { get; set; }
    public bool IsEnable { get; set; }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        base.OnLevelupAnySkill(arg1, arg2);
        TimeCoolDown = CurTimeInterval;
        TimeRemainning = 0f;
    }
    public override void Init(MainCharacter owner)
    {
        base.Init(owner);
        RefreshTime = Time.time;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        IsEnable = false;
    }
    public override void FindOpponent()
    {
        base.FindOpponent();

        mainDirect = findOpponentSystem.GetMainDirect();
        if (mainDirect == Vector3.zero)
        {
            mainDirect = Vector3.left;
        }
    }

    private void Update()
    {
        if (!owner.IsAlive)
            return;
        if (level < 1)
            return;
        TimeRemainning = Time.time - RefreshTime;
        if (Time.time - RefreshTime > CurTimeInterval)
        {
            RefreshTime = Mathf.Infinity;
            CreateBullets();
        }
    }
    public override void LevelUpChange()
    {
        base.LevelUpChange();
        IsEnable = true;
        //bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeed, this);
    }
    public override async void CreateBullets()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.SKILL_KATANA);

        for (int i = 0; i < CurCountBullet; i++)
        {
            FindOpponent();
            if (i % 2 == 0)
                CreateSlash(true);
            else
                CreateSlash(false);
            await Task.Delay(200);
        }
        RefreshTime = Time.time;
        base.CreateBullets();
    }
    private void CreateSlash(bool isFront)
    {
        var direct = mainDirect * (isFront ? 1f : -1f);
        var spawnPos = this.transform.position + direct;
        var tf = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(goPrefabBullet, spawnPos, Quaternion.identity, this.transform);
        tf.right = direct;
        var originScale = tf.localScale;
        tf.localScale = originScale * CharacterStatusHelper.CalculateBulletScale(this);
        
        // move path
        //Vector3 lastPos = this.transform.position + (lifeTime * bulletSpeedReal * direct);
        //tf.DOMove(lastPos, timeMove).SetEase(Ease.Linear).onComplete += () =>
        //{
        //    tf.localScale = originScale;
        //    PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(tf);
        //};
        this.StartDelayAction(lifeTime, () =>
        {
            tf.localScale = originScale;
            PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(tf);
        });
    }
}
