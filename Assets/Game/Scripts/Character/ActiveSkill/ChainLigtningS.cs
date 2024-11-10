using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChainLigtningS : SkillBase
{
    [SerializeField] GameObject prefabBullet;
    [SerializeField] GameObject prefabBullet2;
    float timeRefresh = 5f;
    Vector3 monsterPos;
    [SerializeField] bool selectMonster = false;
    public override void FindOpponent()
    {
        if (!selectMonster)
        {
            var target = findOpponentSystem.GetRandomOpponentInSideCamera();
            if (target)
            {
                monsterPos = target.position;

            }
            else
                monsterPos = findOpponentSystem.GetPosFirstOpponent();

        }
        else
        {
            monsterPos = findOpponentSystem.GetPosFirstOpponent();
        }
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
        SoundController.Instance.PlaySound(SOUND_TYPE.SKILL_SATELLITE_S);
        base.CreateBullets();
        FindOpponent();
        for (int i = 0; i < CurCountBullet; i++)
        {
            if (monsterPos != Vector3.zero)
            {
                if (i == 0)
                {
                    var tf = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(
                        prefabBullet.transform,
                        monsterPos,
                        Quaternion.identity);
                }
                else
                {
                    await Task.Delay(600);
                    var tf = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(
                        prefabBullet2.transform,
                        monsterPos,
                        Quaternion.identity);
                }

            }
        }
        timeRefresh = Time.time;
    }
}
