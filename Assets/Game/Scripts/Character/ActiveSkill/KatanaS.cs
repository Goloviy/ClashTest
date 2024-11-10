using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class KatanaS : SkillBase, ICoolDown
{
    [SerializeField] Transform prefabBullet;
    [SerializeField] float bulletLifeTime = 0.5f;
    float timeRefresh = 0;
    public float TimeRemainning { get; set; }
    public float TimeCoolDown { get; set; }
    public bool IsEnable { get; set; }
    public override void LevelUp()
    {
        this.gameObject.SetActive(true);
        level = 1;
        LevelUpChange();
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
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        IsEnable = false;
    }
    private void Update()
    {
        if (!owner.IsAlive)
            return;
        if (level <= 0)
            return;
        TimeRemainning = Time.time - timeRefresh;
        if (Time.time - CurTimeInterval > timeRefresh)
        {
            timeRefresh = Mathf.Infinity;
            CreateBullets();
        }
    }
    public override async void CreateBullets()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.SKILL_SWORD_S);

        var bPool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        var tf = bPool.Spawn(
            prefabBullet,
            this.transform.position,
            Quaternion.identity, this.transform);
        var oScale = prefabBullet.localScale;
        var newScale = oScale * CharacterStatusHelper.CalculateBulletScale(this);
        tf.localScale = newScale;
        //this.StartDelayAction(bulletLifeTime, () =>
        //{
        //    //tf.localScale = oScale;
        //    //if (bPool.IsSpawned(tf))
        //    //{
        //    //    bPool.Despawn(tf);
        //    //}
        //    timeRefresh = Time.time;
        //});
        await Task.Delay(Mathf.RoundToInt(bulletLifeTime * 1000));
        timeRefresh = Time.time;
    }

}
