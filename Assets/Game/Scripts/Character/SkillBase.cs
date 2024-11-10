using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Active Skill
/// </summary>
public abstract class SkillBase : MonoBehaviour, ICharacterSkill
{
    public int Level => level;
    public SkillName skillName;
    public SkillType type;
    public List<float> MultiplyDamageByLevel = new List<float>() { 1f, 1.5f, 2f, 2f, 2.5f };
    public List<float> MultiplySpeedByLevel = new List<float>() { 1f, 1.5f, 1.5f, 2f, 2f };
    public List<float> MultiplyScaleByLevel = new List<float>() { 1f, 1f, 1.5f, 1.5f, 2f };
    public List<int> CountBulletByLevel = new List<int>() { 1, 2, 3, 4, 5 };
    public List<float> TimeIntervalByLevel = new List<float>() { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
    public float DeltaTimeInflictDamage { get; private set; } = 0.3333f;

    public float CurMultiplyDamage => MultiplyDamageByLevel[level - 1];
    public float CurMultiplySpeed => MultiplySpeedByLevel[level - 1];
    public float CurMultiplyScale => MultiplyScaleByLevel[level - 1];
    public int CurCountBullet => CountBulletByLevel[level - 1];
    public float CurTimeInterval => TimeIntervalByLevel[level - 1] * skillHandle.stats.ReduceTimeCoolDown;

    //public FindOpponentType findOpponentType;
    [HideInInspector] public List<UnitBase> Opponents;
    [HideInInspector] public UnitBase unitTarget;
    [HideInInspector] public Vector3 mainDirect = Vector3.zero;
    [HideInInspector] public int level = 0;
    [HideInInspector] public int levelMax = 5;
    [HideInInspector] protected FindOpponentSystem findOpponentSystem;
    [HideInInspector] public MainCharacter owner;
    [HideInInspector] public SkillHandle skillHandle;

    public bool IsPassiveSkill => type == SkillType.PASSIVE;

    protected bool isDisable = false;
    public virtual void Init(MainCharacter owner)
    {
        this.owner = owner;
        this.skillHandle = owner.skillHandle;
        this.findOpponentSystem = owner.findOpponentSystem;
        this.gameObject.SetActive(false);
        level = 0;
    }

    protected virtual void DisableSkill()
    {
        isDisable = true;
        this.gameObject.SetActive(false);

    }
    protected virtual void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.SKILL_LEVEL_UP_AFTER, OnLevelupAnySkill);
    }
    protected virtual void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.SKILL_LEVEL_UP_AFTER, OnLevelupAnySkill);

    }
    protected virtual void OnLevelupAnySkill(Component arg1, object arg2)
    {
        if (isDisable)
        {
            return;
        }
        //CheckEvol((SkillName)arg2);
    }

    public virtual void FindOpponent()
    {

    }
    public virtual void LevelUp()
    {
        this.gameObject.SetActive(true);
        level++;
        if (level >= levelMax)
            level = levelMax;
        LevelUpChange();
    }
    public virtual void LevelUpChange()
    {

    }
    public virtual void InflictDamage(UnitBase triggerTarget, float scaleDamage = 1f, SkillName skillEffected = SkillName.NONE)
    {
        owner.InflictDamage( CurMultiplyDamage * scaleDamage, triggerTarget, skillEffected);
    }
    public virtual void CreateBullets()
    {

    }

    public SkillName GetSkillName() => skillName;

    public SkillType GetSkillType() => type;

    public BuffType GetBuff()
    {
        return BuffType.NONE;
    }
    public float GetBuffValue()
    {
        return 0;
    }

    public virtual void SetDisable()
    {
        this.gameObject.SetActive(false);
    }
}
