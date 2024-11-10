using PathologicalGames;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : BulletBaseNew
{
    private int currPiercingCount;
    bool isSpawn;

    [SerializeField] bool isPiercing = false;
    [ShowIf("isPiercing", true)]
    [SerializeField] int piercingCount = 0;
    protected override void OnSpawned(SpawnPool pool)
    {
        base.OnSpawned(pool);
        currPiercingCount = 0;
        isSpawn = true;
    }
    protected override void OnDespawned(SpawnPool pool)
    {
        base.OnDespawned(pool);
        isSpawn = false;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSpawn)
        {
            return;
        }

        if (!isPiercing || ++currPiercingCount > piercingCount)
            DespawnThis();

    }
}
