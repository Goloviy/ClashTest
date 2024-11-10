using PathologicalGames;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlay : MonoBehaviour
{
    ParticleSystem particleSystem;
    Collider2D collider;
    /// <summary>
    /// if lifetime = -1 not affect 
    /// </summary>
    [SerializeField] float lifeTime = -1;
    [SerializeField] bool isDelayActiveCollider = false;
    [ShowIf("isDelayActiveCollider", true)]
    [SerializeField] float delayTime = 0f;
    [SerializeField] bool isDelayDisableCollider = false;
    [ShowIf("isDelayDisableCollider", true)]
    [SerializeField] float delayTime2 = 0f;
    bool isBulletPool = false;

    float timeStartCollider = 0f;
    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        if (!particleSystem)
        {
            particleSystem = GetComponentsInChildren<ParticleSystem>()[0];
        }
        collider = GetComponent<Collider2D>();
    }
    private void OnSpawned(SpawnPool pool)
    {
        ActiveCollider(false);
        particleSystem.Play();
        var bPool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
        var fxPool = PoolManager.Pools[StringConst.POOL_FX_NAME];
        var itemPool = PoolManager.Pools[StringConst.POOL_ITEM_NAME];
        if (lifeTime > 0)
        {

            //if (isDelayActiveCollider && collider)
            //{

            //    collider.enabled = false;
            //    this.StartDelayAction(delayTime, () =>
            //    {
            //        collider.enabled = true;
            //    });
            //}
            ActiveCollider(true);
            this.StartDelayAction(lifeTime, () =>
            {
                if (bPool.IsSpawned(this.transform))
                {
                    bPool.Despawn(this.transform);
                }
                else if (fxPool.IsSpawned(this.transform))
                {
                    fxPool.Despawn(this.transform);
                }
                else if (itemPool.IsSpawned(this.transform))
                {
                    itemPool.Despawn(this.transform);
                }
            });
        }
    }
    public void ActiveCollider(bool isActive)
    {
        if (collider)
        {
            if (isActive)
            {
                if (isDelayActiveCollider)
                {
                    collider.enabled = false;
                    this.StartDelayAction(delayTime, () =>
                    {
                        collider.enabled = true;
                    });
                }
                else
                    collider.enabled = true;
                if (isDelayDisableCollider)
                {
                    float waitTime = !isDelayActiveCollider ? delayTime2 : delayTime2 + delayTime;
                    this.StartDelayAction(waitTime, () =>
                    {
                        ActiveCollider(false);
                    });
                }

            }
            else
            {
                collider.enabled = false;
            }
        }
        //else
        //{
        //    DebugCustom.LogError("Go doesn't collider2d :" + this.gameObject.name);
        //}
    }
    private void OnDespawned(SpawnPool pool)
    {
        ActiveCollider(false);
        particleSystem.Stop();
    }
}
