using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BossBehaviorTideBullet : EnemyBehavior
{
    [SerializeField] Transform tfBullet;
    [SerializeField] Transform tfMark;
    [SerializeField] float distanceBullet = 1.5f;
    [SerializeField] int countBulletPerLine = 3;
    [SerializeField] TideBulletDirect DirectType = TideBulletDirect.NONE;
    [SerializeField] bool bulletGrowBigger = true;
    //bool isAction = false;
    //Vector3[] allPosOfLine;
    Queue<Transform> queueTfMark;
    List<Vector3[]> linesPos;
    int spawnLineCounter = 0;
    public override void Select(Action<EnemyBehaviorState> action)
    {
        linesPos = new List<Vector3[]>();
        
        base.Select(action);
        turnFaceable.TurnFaceToEnemy();
        PlayAnim();
    }
    public override void MarkTargetAction()
    {
        base.MarkTargetAction();
        SpawnMark();
    }
    public override void ShortAction()
    {

            turnFaceable.TurnFaceToEnemy();
            SpawnBullet();
    }
    public override void FinishBehavior()
    {
        RemoveMarks();
        base.FinishBehavior();
    }
    private void RemoveMarks()
    {
        foreach (var mark in queueTfMark)
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
        var allPosOfLine = new Vector3[countBulletPerLine];
        queueTfMark = new Queue<Transform>();
        for (int i = 0; i < countBulletPerLine; i++)
        {
            var spawnPos = this.transform.position + (distanceBullet * (i + 1) * direct);
            allPosOfLine[i] = spawnPos;
            var tfM = pool.Spawn(tfMark, spawnPos, tfMark.rotation);
            tfM.transform.localScale = tfMark.localScale * (bulletGrowBigger ? (1f + i * 0.4f) : 1f);

            queueTfMark.Enqueue(tfM);
        }
        linesPos.Add(allPosOfLine);
    }

    private void SpawnBullet()
    {
        if (linesPos.Count <= spawnLineCounter + 1)
        {
            return;
        }
        var curLine = linesPos[spawnLineCounter++];
        //spawn Bullets n remove Marks
        for (int i = 0; i < countBulletPerLine; i++)
        {
            
            var spawnPos = curLine[i];
            int k = i;
            this.StartDelayAction(0.12f * (k + 1), () =>
            {
                if (pool.IsSpawned(queueTfMark.Peek()))
                {
                    pool.Despawn(queueTfMark.Dequeue());
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
        this.StartDelayAction(0.12f * countBulletPerLine, FinishBehavior);
    }

}
