using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkillStorm : SkillBase
{
    public Transform prefabBullet;

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
