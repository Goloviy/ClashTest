using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkillElement : SkillBase
{
    [SerializeField] GameObject prefabBullet;
    //[SerializeField] SpriteRenderer goElement;
    float timeRefresh = 5f;
    //Vector3 targetPos;
    [SerializeField] bool selectMonster = false;
    float curMinDistance;
    float deltaDistance;
    [SerializeField] private float MinDistance = 5f;
    [SerializeField] private float DeltaDistance = 1f;
    Vector3 originScale = Vector3.one;
    private float bulletScaleReal = 1f;

    private float lastTimeUpdateDirect = 0f;
    private float timeDirectCD = 0.3f;
    private void Awake()
    {
        curMinDistance = MinDistance;
        deltaDistance = DeltaDistance;
    }
    //public override void FindOpponent()
    //{
    //    if (!selectMonster)
    //    {
    //        targetPos = findOpponentSystem.GetRandomPositionInScreen();

    //    }
    //    else
    //    {
    //        targetPos = findOpponentSystem.GetRandomPosOpponent();
    //    }
    //}
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        base.OnLevelupAnySkill(arg1, arg2);
        if (isDisable)
            return;
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
        curMinDistance = MinDistance * bulletScaleReal;
        deltaDistance = DeltaDistance * bulletScaleReal;
    }
    public override void LevelUpChange()
    {
        base.LevelUpChange();
        //if (level >= 1 && !isDisable)
        //{
        //    goElement.gameObject.SetActive(true);
        //}
    }
    protected override void DisableSkill()
    {
        base.DisableSkill();
        //goElement.gameObject.SetActive(false);

    }
    //private void DirectUpdate()
    //{
    //    if (Time.time - timeDirectCD > lastTimeUpdateDirect)
    //    {
    //        lastTimeUpdateDirect = Time.time;
    //        var mainDirect = findOpponentSystem.GetMainDirect();
    //        bool isRight;
    //        if (mainDirect.x > 0)
    //        {
    //            isRight = true;
    //        }
    //        else
    //        {
    //            isRight = false;
    //        }
    //        goElement.flipX = isRight;
    //    }
    //}
    private void Update()
    {
        //DirectUpdate();
        if (!owner.IsAlive || isDisable)
            return;
        if (level > 0)
        {
            if (Time.time - CurTimeInterval > timeRefresh)
            {
                timeRefresh = Mathf.Infinity;
                CreateBullets();
            }
        }
    }
    public override async void CreateBullets()
    {
        for (int i = 0; i < CurCountBullet; i++)
        {
            //FindOpponent();

            Vector3 dir = Random.insideUnitCircle;
            float length = curMinDistance + deltaDistance * Random.value;
            Vector3 pos = dir.normalized * length + GameDynamicData.mainCharacter.transform.position;

            var tf = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(
                prefabBullet.transform,
                pos,
                Quaternion.identity);
            //originScale = tf.localScale;
            //tf.localScale = originScale * bulletScaleReal;

            await Task.Delay(50);
        }
        timeRefresh = Time.time;
    }
}
