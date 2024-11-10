using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLaserS : SkillBase
{
    private float bulletScaleReal;
    [SerializeField] Transform prefabLaser;

    //Transform tfLaser;
    Vector3 lastDirect = Vector3.zero;
    Transform tfLaser;
    public override void FindOpponent()
    {
        base.FindOpponent();
        mainDirect = findOpponentSystem.GetMainDirect();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_DIE, DeactiveSkill);
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_REVIVE, ActiveSkill);

    }
    private void DeactiveSkill(Component arg1, object arg2)
    {

    }
    private void ActiveSkill(Component arg1, object arg2)
    {

    }
    protected override void DisableSkill()
    {
        RemoveBullet();
        base.DisableSkill();
    }

    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        base.OnLevelupAnySkill(arg1, arg2);
        if (isDisable)
            return;
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);

    }
    public override void LevelUpChange()
    {
        if (isDisable)
            return;
        CreateBullet();
    }
    private void RemoveBullet()
    {
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        if (tfLaser)
        {
            pool.Despawn(tfLaser.transform);
            tfLaser = null;
        }
    }
    private void CreateBullet()
    {
        RemoveBullet();
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        tfLaser = pool.Spawn(prefabLaser, this.transform.position, prefabLaser.transform.rotation, this.transform);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_DIE, DeactiveSkill);
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_REVIVE, ActiveSkill);

    }
}
