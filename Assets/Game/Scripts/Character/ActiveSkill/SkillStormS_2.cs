using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStormS_2 : SkillBase
{
    public Transform prefabBullet;
    public Transform prefabLavarPlash;
    [SerializeField] float timeIntervalTrail = 1.5f;
    float timeRefresh = 5f;
    Transform tfStorm;
    private float bulletScaleReal = 1f;

    public override void FindOpponent()
    {
        mainDirect = findOpponentSystem.GetRandomDirectionToOpponent(this.transform.position);

        if (mainDirect == Vector3.zero)
        {
            mainDirect = new Vector3(0.7f, 0.7f, 0);
        }
    }
    private void Update()
    {
        if (tfStorm != null)
        {
            if (Time.time - timeIntervalTrail > timeRefresh)
            {
                timeRefresh = Time.time;
                CreateLarva();
                //DebugCustom.Log("Create Lava");
            }
        }
    }
    private void CreateLarva()
    {
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        var tfDrop = pool.Spawn(prefabLavarPlash, tfStorm.position, Quaternion.identity);
        tfDrop.localScale = prefabLavarPlash.localScale * bulletScaleReal;
    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        CreateStorms();
    }
    public override void LevelUpChange()
    {
        CreateStorms();
    }
    public override void LevelUp()
    {
        base.LevelUp();

    }
    public void CreateStorms()
    {
        ClearStorm();
        if (level >= 1)
        {
            CreateStorm();
        }
    }
    private void ClearStorm()
    {
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        if (tfStorm)
        {
            pool.Despawn(tfStorm);
            tfStorm = null;
        }
    }
    private void CreateStorm()
    {
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
        FindOpponent();
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        tfStorm = pool.Spawn(prefabBullet, this.transform.position, Quaternion.identity);
        tfStorm.localScale = prefabBullet.transform.localScale * bulletScaleReal;
        var storm = tfStorm.GetComponent<StormBullet>();
        storm.Init(this);
    }

    public override void SetDisable()
    {
        ClearStorm();
        base.SetDisable();
    }
}
