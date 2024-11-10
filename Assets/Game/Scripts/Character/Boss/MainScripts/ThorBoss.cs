using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorBoss : BossCharacter
{
    protected override void OnSpawned(SpawnPool pool)
    {
        base.OnSpawned(pool);
        Init();
        //CollisionAttackData = new AttackData(this, Mathf.RoundToInt(GameDynamicData.atkMonster * collisionDamageMultiply), 0);
    }

    protected override void OnCreate()
    {
        HpMax = GameDynamicData.hpMonster;
        HpRemainning = HpMax;
        //Attack = GameDynamicData.atkMonster;
        moveSpeed *= GameDynamicData.mSpeedMonster;
    }


}
