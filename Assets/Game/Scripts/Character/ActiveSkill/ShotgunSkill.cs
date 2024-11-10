using DG.Tweening;
using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// This skill bullet count not affected
/// </summary>
public class ShotgunSkill : SkillBase
{
    [SerializeField] List<ShotgunDirect> groupsDirect;
    ShotgunDirect curGroupDirect => groupsDirect[level > 0 ? level - 1 : 0];

    public Transform goPrefabBullet;
    public float bulletSpeed;
    public float RefreshTime;
    public float timeMove;

    public override void Init(MainCharacter owner)
    {
        base.Init(owner);
        RefreshTime = Time.time;
    }
    public override void FindOpponent()
    {
        base.FindOpponent();
        mainDirect = findOpponentSystem.GetMainDirect();

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
            FindOpponent();
            CreateBullets();
        }
    }
    public override void LevelUpChange()
    {
        base.LevelUpChange();
        UpdateGroup();
    }
    private void UpdateGroup()
    {
        foreach (var item in groupsDirect)
        {
            item.gameObject.SetActive(false);
        }
        curGroupDirect.gameObject.SetActive(true);
    }
    public override void CreateBullets()
    {
        curGroupDirect.transform.right = mainDirect;
        for (int i = 0; i < curGroupDirect.directs.Count; i++)
        {
            var tf = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(goPrefabBullet, this.transform.position, Quaternion.identity);
            //scale size
            var originScale = tf.localScale;
            tf.localScale = originScale * CharacterStatusHelper.CalculateBulletScale(this);
            // move path random
            var direct = (curGroupDirect.directs[i].position - this.transform.position).normalized;
            Vector3 lastPos = this.transform.position +  ( timeMove * bulletSpeed  * direct);
            tf.right = direct;
            tf.DOMove(lastPos, timeMove).SetEase(Ease.Linear).onComplete += () =>
            {
                tf.localScale = originScale;
                PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(tf);
            };
        }
        RefreshTime = Time.time;

    }
}
