using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBalista : SkillBase
{
    public Transform prefabBalista;
    public Transform prefabBullet;
    float balistaLifeTime = 8f;
    float bulletSpeed = 5f;
    float bulletSpeedReal = 5f;
    float bulletScaleReal = 1f;

    Vector3 rdPos;

    List<Transform> listBalista;
    private void Awake()
    {
        listBalista = new List<Transform>();
    }
    public override void FindOpponent()
    {
        var target = findOpponentSystem.GetRandomOpponentInSideCamera();
        if (target)
        {
            rdPos = target.position;
        }

    }

    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        bulletSpeedReal = CharacterStatusHelper.CalculateBulletMoveSpeed(bulletSpeed, this);
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
    }
    public override void LevelUpChange()
    {
        base.LevelUpChange();
        CreateBalista();
    }

    public void CreateBalista()
    {
        var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        Vector2 spawnPos = this.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * 2f;
        var tf = pool.Spawn(prefabBalista, spawnPos, prefabBalista.rotation);
        listBalista.Add(tf);
        this.StartDelayAction(balistaLifeTime, () =>
        {
            listBalista.Remove(tf);
            pool.Despawn(tf);
        });

    }
    private void Update()
    {
        if (listBalista.Count > 0)
        {
        }
    }
}
