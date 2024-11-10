using DG.Tweening;
using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BoomerangSkill : SkillBase
{
    [SerializeField] List<BoomerangGroupDirect> groupsDirect;
    BoomerangGroupDirect curGroupDirect => groupsDirect[0];

    public Transform goPrefabBoomerang;

    public float RefreshTime;
    public override void Init(MainCharacter owner)
    {
        base.Init(owner);
        RefreshTime = Time.time;
    }
    public override void FindOpponent()
    {
        base.FindOpponent();
        mainDirect = findOpponentSystem.GetRandomDirectionToOpponent(this.transform.position);

    }

    private void Update()
    {
        if (!owner.IsAlive)
            return;
        if (Time.time - RefreshTime > CurTimeInterval)
        {
            RefreshTime = Mathf.Infinity;
            CreateBullets();
        }
    }

    private async void CreateBullets()
    {

        for (int i = 0; i < CurCountBullet; i++)
        {
            if (i >= 1)
                await Task.Delay(250);
            FindOpponent();
            curGroupDirect.transform.right = mainDirect;
            var tf = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(goPrefabBoomerang, this.transform.position, Quaternion.identity);
            //scale size
            var originScale = tf.localScale;
            tf.localScale = originScale * CharacterStatusHelper.CalculateBulletScale(this);
            // move path random
            int rdPath = UnityEngine.Random.Range(0, curGroupDirect.paths.Count);

            float tweenTime = CharacterStatusHelper.CalculateBulletMoveTime(3f, this);
            tf.DOPath(curGroupDirect.paths[rdPath].points.Select(p=>p.position).ToArray(), tweenTime, PathType.CatmullRom).SetEase(Ease.Linear).onComplete += MoveFinish;
            // rotate boomerang
            bool isLeft = curGroupDirect.paths[rdPath].rotateLeft;
            tf.DORotate(UnityEngine.Random.Range(550f, 850f) * (isLeft ? Vector3.forward : Vector3.back), tweenTime, RotateMode.FastBeyond360);

            void MoveFinish()
            {
                tf.localScale = originScale;
                PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(tf);
            }
        }
        RefreshTime = Time.time;

    }
}
