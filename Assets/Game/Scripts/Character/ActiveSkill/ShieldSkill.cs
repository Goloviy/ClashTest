using DG.Tweening;
using PathologicalGames;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShieldSkill : SkillBase
{
    public List<GameObject> prefabShieldsByLevel;
    public GameObject CurPrefabShield => prefabShieldsByLevel[level -1];
    public List<float> ActiveTimeByLevel;
    public float CurActiveTime => ActiveTimeByLevel[level - 1];

    GroupShield curGroup;

    //[SerializeField] bool isEvol;
    //[ShowIf("isEvol", true)]
    //[SerializeField] Transform prefabBullet;
    //[ShowIf("isEvol", true)]
    //[SerializeField] private float distance = 5f;
    //float timeRefresh = 5f;
    //[ShowIf("isEvol", true)]
    //[SerializeField] int cdTimeBulletMs = 1000;

    private float bulletScaleReal;
    protected override void OnEnable()
    {
        base.OnEnable();
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_DIE, DeactiveSkill);
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_REVIVE, ActiveSkill);

    }
    private void DeactiveSkill(Component arg1, object arg2)
    {
        DestroyCurrentGroup();
    }
    private void ActiveSkill(Component arg1, object arg2)
    {
        //if (!isEvol)
        //{
            CreateNewGroup();
        //}
    }
    protected override void OnLevelupAnySkill(Component arg1, object arg2)
    {
        bulletScaleReal = CharacterStatusHelper.CalculateBulletScale(this);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_DIE, DeactiveSkill);
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_REVIVE, ActiveSkill);

    }
    private void DestroyCurrentGroup()
    {
        if (curGroup)
        {
            Destroy(curGroup.gameObject);
            curGroup = null;
        }
    }
    private void CreateNewGroup()
    {
        if (level >= 1)
        {
            var tf = Instantiate(CurPrefabShield, this.transform.position, Quaternion.identity, this.transform);
            curGroup = tf.GetComponent<GroupShield>();
            curGroup.Init(this);
        }
    }
    public override void LevelUpChange()
    {
        base.LevelUpChange();
        //if (!isEvol)
        //{
            DestroyCurrentGroup();
            CreateNewGroup();
        //}
        //else
        //{
        //    DestroyCurrentGroup();
        //}
    }
    
    public override void FindOpponent()
    {
        base.FindOpponent();
        mainDirect = findOpponentSystem.GetMainDirect();
    }
    //private void Update()
    //{
    //    if (!isEvol)
    //    {
    //        return;
    //    }
    //    if (level >= 1)
    //    {
    //        float timeInvertal = 1f;
    //        if (Time.time - timeRefresh > timeInvertal)
    //        {
    //            timeRefresh = Mathf.Infinity;
    //            FindOpponent();
    //            CreateBigSword();
    //        }
    //    }
    //}
    //private async void CreateBigSword()
    //{
    //    if (mainDirect == Vector3.zero)
    //    {
    //        return;
    //    }
    //    var posAppear = this.transform.position + mainDirect * distance;
    //    var pool = PoolManager.Pools[StringConst.POOL_BULLET_NAME];
    //    var tf = pool.Spawn(prefabBullet, posAppear, Quaternion.identity);
    //    var originScale = prefabBullet.localScale;
    //    tf.localScale = originScale * bulletScaleReal;
    //    await Task.Delay(cdTimeBulletMs);
    //    timeRefresh = Time.time;
    //}
}
