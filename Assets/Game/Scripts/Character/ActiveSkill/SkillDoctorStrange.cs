using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkillDoctorStrange : SkillBase
{
    [SerializeField] GameObject prefabAoe;
    float timeRefresh = Mathf.NegativeInfinity;
    
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
    public override void CreateBullets()
    {
        base.CreateBullets();
        for (int i = 0; i < CurCountBullet; i++)
        {

            var tf = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(
                prefabAoe.transform,
                this.transform.position,
                Quaternion.identity);
        }
        timeRefresh = Time.time;
    }
}
