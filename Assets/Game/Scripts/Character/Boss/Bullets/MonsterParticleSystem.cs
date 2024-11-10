using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParticleSystem : MonsterBulletBase
{
    ParticleSystem ParticleSystem;
    private void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
        if (!ParticleSystem)
        {
            ParticleSystem = GetComponentInChildren<ParticleSystem>();
        }
    }
    protected override void OnSpawned(SpawnPool pool)
    {
        ParticleSystem.Play();
        base.OnSpawned(pool);
    }
    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!isDespawnAfterTrigger)
    //    {
    //        var eAtk = SpawnerMananger.Instance.GetAttack(OwnerId);
    //        eAtk = Mathf.RoundToInt(eAtk * multiplyDamge);
    //        attackData = new AttackData(null, eAtk, 0);
    //        GameDynamicData.mainCharacter.TakeDamage(attackData);
    //        Despawn();
    //    }
    //}
}
