using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BricksSkill : SkillBase
{
    Vector3 startLine = new Vector3(-6, -9);
    Vector3 endLine = new Vector3(6, -9);

    public GameObject goPrefabBrick;

    public float RefreshTime;
    public override void Init(MainCharacter owner)
    {
        base.Init(owner);
        RefreshTime = Time.time;
    }
    private Vector3 GetEndPos()
    {
        var rd = Random.Range(0, 1f);
        return Vector3.Slerp(startLine, endLine, rd);
    }
    private async void CreateBricks()
    {
        for (int i = 0; i < CurCountBullet; i++)
        {
            var tf = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(goPrefabBrick, this.transform.position, Quaternion.identity);
            var localScale = tf.localScale;
            tf.localScale = localScale * CharacterStatusHelper.CalculateBulletScale(this);
            float timeMove = 3.2f;
            timeMove = timeMove / CurMultiplySpeed;
            float rdForce = Random.Range(11.5f, 13.5f);
            tf.DOJump(GetEndPos(), rdForce, 1, timeMove).SetEase(Ease.OutSine).onComplete += () =>
            {
                tf.localScale = localScale;
                PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(tf);
            };
            await Task.Delay(80);
        }
        RefreshTime = Time.time;
    }
    private void Update()
    {
        if (!owner.IsAlive)
            return;
        if (level < 1)
            return;
        if (Time.time - RefreshTime > CurTimeInterval)
        {
            RefreshTime = Mathf.Infinity;
            CreateBricks();
        }
    }

}
