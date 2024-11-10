using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MegaMeteorSkill : SkillBase
{
    [SerializeField] GameObject prefabBullet;
    //float curMinDistance;
    //float deltaDistance;
    //[Header("Area setting")]
    //[SerializeField] private float MinDistance = 5f;
    //[SerializeField] private float DeltaDistance = 1f;
    [Header("Bullets")]
    [SerializeField] private float timeMoveCircle = 5f;
    float timeMoveLine;
    private float timeRefresh = 0f;
    Vector3 originScale = Vector3.one;
    private float bulletScaleReal = 1f;

    //Vector3 up = Vector3.up;
    //Vector3 down = Vector3.down;
    //Vector3 left = Vector3.left;
    //Vector3 right = Vector3.right;
    [SerializeField] float distanceBulletToCharacter = 2f;
    float curDistanceBulletToCharacter = 2f;
    Vector3[] directs = new Vector3[]
    {
        Vector3.up ,
        Vector3.right ,
        Vector3.down ,
        Vector3.left
    };
    int curLineId = 0;

    float lastTimeUpdateLine = 0f;
    private void Awake()
    {
        //curMinDistance = MinDistance;
        //deltaDistance = DeltaDistance;
        curLineId = 0;
    }

    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        base.OnLevelupAnySkill(arg1, arg2);
        if (isDisable)
            return;
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
        //curMinDistance = MinDistance * bulletScaleReal;
        //deltaDistance = DeltaDistance * bulletScaleReal;
        curDistanceBulletToCharacter = distanceBulletToCharacter * bulletScaleReal;
        timeMoveLine = timeMoveCircle / directs.Length;

    }
    public override void LevelUpChange()
    {
        base.LevelUpChange();
    }
    protected override void DisableSkill()
    {
        base.DisableSkill();

    }
    
    private void Update()
    {
        if (!owner.IsAlive || isDisable)
            return;
        if (level > 0)
        {
            UpdateLine();
            if (Time.time - CurTimeInterval > timeRefresh)
            {
                timeRefresh = Time.time;
                CreateBullets();
            }
        }
    }
    private void UpdateLine()
    {
        if (Time.time - timeMoveLine >= lastTimeUpdateLine)
        {
            lastTimeUpdateLine = Time.time;
            curLineId = curLineId < directs.Length - 1 ? curLineId + 1 : 0;
            DebugCustom.Log("curLineId" + curLineId);
        }
    }
    public override void CreateBullets()
    {
        float t = Mathf.Clamp01(( Time.time - lastTimeUpdateLine) / timeMoveLine);
        var posTarget = this.transform.position + 
            Vector3.Slerp(
                directs[curLineId] * curDistanceBulletToCharacter,
                directs[curLineId + 1 > directs.Length - 1 ? 0: curLineId + 1] * curDistanceBulletToCharacter,
                t);

        int line2 = curLineId + 2 > directs.Length - 1 ? curLineId + 2 - directs.Length : curLineId + 2;
        var posTarget2 = this.transform.position + Vector3.Slerp(directs[line2] * curDistanceBulletToCharacter,
            directs[line2 + 1 > directs.Length - 1 ? 0 : line2 + 1] * curDistanceBulletToCharacter,
            t);
        PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(
            prefabBullet.transform,
            posTarget,
            Quaternion.identity);
        PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(
            prefabBullet.transform,
            posTarget2,
            Quaternion.identity);
        //for (int i = 0; i < CurCountBullet; i++)
        //{

        //    Vector3 dir = Random.insideUnitCircle;
        //    float length = curMinDistance + deltaDistance * Random.value;
        //    Vector3 pos = dir.normalized * length + GameDynamicData.mainCharacter.transform.position;

        //    var tf = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(
        //        prefabBullet.transform,
        //        pos,
        //        Quaternion.identity);

        //    await Task.Delay(50);
        //}
    }
}
