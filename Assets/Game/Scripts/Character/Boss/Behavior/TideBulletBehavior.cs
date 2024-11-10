using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum TideBulletDirect
{
    NONE,
    TARGET,
    LINE_HORIZONTAL,
    LINE_VERTICAL
}
public class TideBulletBehavior : EnemyBehavior
{
    [SerializeField] Transform tfBullet;
    [SerializeField] Transform tfMark;
    [SerializeField] float distanceBullet = 1.5f;
    [SerializeField] int countBullet = 3;
    [SerializeField] TideBulletDirect DirectType = TideBulletDirect.NONE;
    [SerializeField] bool bulletGrowBigger = true;
    bool isAction = false;
    Vector3[] arrayPos;
    Transform[] tfMarks;
    public override void Select(Action<EnemyBehaviorState> action)
    {
        base.Select(action);
        turnFaceable.TurnFaceToEnemy();
        PlayAnim();
        SpawnMark();
    }
    public override void ShortAction()
    {
        if (!isAction)
        {
            turnFaceable.TurnFaceToEnemy();
            isAction = true;
            SpawnBullet();
        }
    }
    public override void FinishBehavior()
    {
        RemoveMarks();
        base.FinishBehavior();
        isAction = false;
    }
    private void RemoveMarks()
    {
        foreach (var mark in tfMarks)
        {
            if (mark)
            {
                if (pool.IsSpawned(mark))
                {
                    pool.Despawn(mark);
                }
            }
        }
    }
    private Vector3 GetDirect()
    {
        Vector3 _direct;
        if (DirectType == TideBulletDirect.TARGET)
        {
            _direct = (GameDynamicData.mainCharacter.transform.position - this.transform.position).normalized;
        }
        else if (DirectType == TideBulletDirect.LINE_HORIZONTAL)
        {
            _direct = turnFaceable.IsFaceLeft ? Vector3.left : Vector3.right;
        }
        else
        {
            _direct = (GameDynamicData.mainCharacter.transform.position - this.transform.position).normalized;

        }
        return _direct;
    }
    private void SpawnMark()
    {
        Vector3 direct = GetDirect();
        arrayPos = new Vector3[countBullet];
        tfMarks = new Transform[countBullet];
        for (int i = 0; i < countBullet; i++)
        {
            var spawnPos = this.transform.position + (distanceBullet * (i + 1) * direct);
            arrayPos[i] = spawnPos;
            var tfM = pool.Spawn(tfMark, spawnPos, tfMark.rotation);
            tfM.transform.localScale = tfMark.localScale * (bulletGrowBigger ? (1f + i * 0.4f) : 1f);

            tfMarks[i] = tfM;
        }
    }
    
    private void SpawnBullet()
    {
        //spawn Bullets n remove Marks
        for (int i = 0; i < countBullet; i++)
        {
            var spawnPos = arrayPos[i];
            int k = i;
            this.StartDelayAction(0.15f * (k + 1), () =>
            {
                if (pool.IsSpawned(tfMarks[k]))
                {
                    pool.Despawn(tfMarks[k]);
                }
                var tfB = pool.Spawn(tfBullet, spawnPos, tfBullet.rotation);
                var projecttile = tfB.GetComponent<EnemyProjectTile>();
                if (projecttile)
                {
                    SetupBullets(projecttile);
                }
                tfB.transform.localScale = tfBullet.localScale * (bulletGrowBigger ? (1f + k * 0.4f) : 1f);
                this.StartDelayAction(0.8f, () =>
                {
                    pool.Despawn(tfB);
                });
            });
        }
        this.StartDelayAction(0.15f * countBullet, FinishBehavior);
    }
    
}
