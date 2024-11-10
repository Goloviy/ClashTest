using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChainLightningSkill : SkillBase
{
    [SerializeField] GameObject prefabBullet;
    float timeRefresh = 5f;
    //Vector3 monsterPos;
    Transform target;
    //[SerializeField] bool selectMonster = false;
    public override void FindOpponent()
    {

        target = findOpponentSystem.GetRandomOpponentInSideCamera();
    }
    private void Update()
    {
        if (!owner.IsAlive)
            return;
        if (level <= 0)
            return;
        if (Time.time - CurTimeInterval > timeRefresh)
        {
            timeRefresh = Mathf.Infinity;
            CreateBullets();
        }
    }
    public override async void CreateBullets()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.SKILL_SATELLITE);

        base.CreateBullets();
        for (int i = 0; i < CurCountBullet; i++)
        {
            FindOpponent();
            if (target != null)
            {
                var tf = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(
                    prefabBullet.transform,
                    target.position,
                    Quaternion.identity);
                await Task.Delay(100);
            }
        }
        timeRefresh = Time.time;
    }
}
