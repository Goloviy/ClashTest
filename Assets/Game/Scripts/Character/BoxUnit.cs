using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxUnit : MonoBehaviour
{
    [SerializeField] List<GameObject> listItem;
    [SerializeField] Transform prefabFxDeath;

    protected void OnDespawned(SpawnPool pool)
    {
        DropItem();
    }
    protected void Die()
    {
        PoolManager.Pools[StringConst.POOL_ITEM_NAME].Despawn(this.transform);
    }
    protected void DropItem()
    {
        if (listItem.Count > 0)
        {
            var prefab = listItem[Random.Range(0, listItem.Count)];
            if (prefab)
            {
                if (prefab.CompareTag(StringConst.TAG_EXP) ||
                    prefab.CompareTag(StringConst.TAG_EXP2) ||
                    prefab.CompareTag(StringConst.TAG_EXP3))
                {
                    PoolManager.Pools[StringConst.POOL_EXP_NAME].Spawn(prefab, this.transform.position, Quaternion.identity);
                }
                else
                {
                    PoolManager.Pools[StringConst.POOL_ITEM_NAME].Spawn(prefab, this.transform.position, Quaternion.identity);
                }
            }

            PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(prefabFxDeath, this.transform.position, prefabFxDeath.rotation);
        }
        else
        {
            DebugCustom.Log("Box Empty");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }
}
