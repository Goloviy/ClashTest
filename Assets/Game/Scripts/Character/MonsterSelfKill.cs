using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSelfKill : MonsterBase
{
    [SerializeField] Transform prefabExplosion;
    [SerializeField] float timeExplosion = 0.35f;
    public override void Die()
    {
        HpRemainning = 0;
        collider.enabled = false;
        rigid.simulated = false;
        base.Die();
        Drop();
        DespawnThis();
        GameDynamicData.KillCount++;
        EventDispatcher.Instance.PostEvent(EventID.MONSTER_DIE);
        //PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(prefabFxDeath, this.transform.position, prefabFxDeath.rotation);
        Explosion();
    }
    private void Explosion()
    {
        var pool = PoolManager.Pools[StringConst.POOL_FX_NAME];
        var tf = pool.Spawn(prefabExplosion, this.transform.position, Quaternion.identity);
        //this.StartDelayAction(timeExplosion, () =>
        //{
        //    if (pool.IsSpawned(tf))
        //    {
        //        pool.Despawn(tf);
        //    }
        //});
    }
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if (IsAlive && collision.gameObject.layer.Equals(GameDynamicData.mainCharacter.gameObject.layer))
        {
            Die();
        }
    }

}
