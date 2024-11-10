using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBoss : BossCharacter
{
    protected override void OnSpawned(SpawnPool pool)
    {
        base.OnSpawned(pool);
        Init();
        var eAtk = SpawnerMananger.Instance.GetAttack(id);
        //CollisionAttackData = new AttackData(this, Mathf.RoundToInt(eAtk * collisionDamageMultiply), 0);
    }
}
