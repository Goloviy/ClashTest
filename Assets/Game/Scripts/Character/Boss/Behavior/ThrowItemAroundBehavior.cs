using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ThrowItemAroundBehavior : EnemyBehavior
{
    [SerializeField] Transform prefabBullet;
    List<Transform> tfBullets;
    [SerializeField] float timeBulletAlive = 1.5f;
    [SerializeField] float bulletSpeed = 6f;

    [SerializeField] int way = 10;
    Vector2[] Direct8 = new Vector2[8]
    {
            new Vector2(0,-1),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0),
            new Vector2(1,-1),
            new Vector2(-1,-1),
            new Vector2(-1,1),
            new Vector2(-1,0)
    };
    public override void Select(Action<EnemyBehaviorState> action)
    {
        tfBullets = new List<Transform>();
        base.Select(action);
        PlayAnim();
        for (int i = 0; i < way; i++)
        {
            int k = i;
            this.StartDelayAction(0.25f * (k + 1), () =>
              {
                  SpawnBullets(this.transform.position, k);
              });
        }
        this.StartDelayAction(0.25f * way, FinishBehavior);
    }

    private void SpawnBullets(Vector3 startPos, int index = 0)
    {
        bool isInvertWay = index >= way / 2;
        foreach (var direct in Direct8)
        {
            Vector3 _direct = direct.normalized;
            var crossDirect = index * (isInvertWay? 1f : -1f) * Vector3.Cross(_direct, Vector3.back);
            var tf = pool.Spawn(prefabBullet, startPos, prefabBullet.rotation);
            var projecttile = tf.GetComponent<EnemyProjectTile>();
            if (projecttile)
            {
                SetupBullets(projecttile);
            }
            var endPos = startPos + (timeBulletAlive * _direct * bulletSpeed) + crossDirect;
            
            tf.DOMove(endPos, timeBulletAlive).SetEase(Ease.Linear).onComplete += () =>
            {
                if (pool.IsSpawned(tf))
                {
                    pool.Despawn(tf);

                }
            };
        }
    }
}
