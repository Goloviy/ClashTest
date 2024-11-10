using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class SkillHandle : MonoBehaviour
{
    public SkillStatus stats;
    public Dictionary<SkillName, ICharacterSkill> dictSkill;
    public GameObject containerSkill;
    MainCharacter mainCharacter;
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_SELECT_SKILL_LEVELUP, OnPlayerSelectSkill);
    }
    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_SELECT_SKILL_LEVELUP, OnPlayerSelectSkill);
    }

    private void OnPlayerSelectSkill(Component arg1, object arg2)
    {
        var skillType = (SkillName)arg2;
        LearnSkill(skillType);
    }

    public void Init(MainCharacter mainCharacter)
    {
        this.mainCharacter = mainCharacter;
        var skills = containerSkill.GetComponentsInChildren<ICharacterSkill>(true).ToList();
        dictSkill = new Dictionary<SkillName, ICharacterSkill>();
        foreach (var skill in skills)
        {
            skill.Init(mainCharacter);
            dictSkill.Add(skill.GetSkillName(), skill);
        }
    }
    public ICharacterSkill GetSkill(SkillName type)
    {
        ICharacterSkill skill;
        if (!dictSkill.TryGetValue(type, out skill))
            DebugCustom.LogError("Player doesn't have Skill :" + type);
        return skill;
    } 

    public void LearnSkill(SkillName type, bool isMain = false)
    {
        ICharacterSkill skill;
        if (dictSkill.TryGetValue(type, out skill))
        {
            var dataSkill = GameData.Instance.staticData.skillsData.GetSkill(type);
            //If evol skill => remove skill base
            if (dataSkill.evolved)
            {
                foreach (var require in dataSkill.requireLearns)
                {
                    //neu la skill tien hoa , remove gameobject cua skill co ban
                    ICharacterSkill baseSkill;
                    if (dictSkill.TryGetValue(require.requireSkill, out baseSkill))
                    {
                        baseSkill.SetDisable();
                    }
                }

            }
            //add new skill
            GameData.Instance.playerData.LearnSkill(type);
            skill.LevelUp();
            if (skill.IsPassiveSkill)
                HandlePassiveSkill(skill);
            EventDispatcher.Instance.PostEvent(EventID.SKILL_LEVEL_UP_AFTER, type);
            
        }
        else
        {
            DebugCustom.LogError("Player doesn't have Skill :" + type);
        }
    }
    private void HandlePassiveSkill(ICharacterSkill skill)
    {
        switch (skill.GetBuff())
        {
            case BuffType.NONE:
                break;
            case BuffType.MAGNET:
                stats.RangeCollectorRatio = 1f + skill.GetBuffValue();
                break;
            case BuffType.CRITICAL_RATE:
                stats.CriticalRate = Mathf.RoundToInt( skill.GetBuffValue());
                break;
            case BuffType.ATTACK_DAMAGE:
                stats.AttackRatio = 1f + skill.GetBuffValue();
                break;
            case BuffType.BULLET_SPEED:
                stats.BulletMoveSpeedRatio = 1f + skill.GetBuffValue();
                break;
            case BuffType.BULLET_SCALE:
                stats.BulletScaleRatio = 1f + skill.GetBuffValue();
                break;
            case BuffType.CHARACTER_SPEED:
                stats.MoveSpeedRatio = 1f + skill.GetBuffValue();
                break;
            case BuffType.HIT_POINT_MAX:
                stats.HpRatio = 1f + skill.GetBuffValue();
                break;
            case BuffType.RENGEN_PER_SECONDS:
                stats.RegenPerSecond = Mathf.RoundToInt(0 + skill.GetBuffValue());
                break;
            case BuffType.REDUCE_TIME_CD:
                stats.ReduceTimeCoolDown = 1f + skill.GetBuffValue();
                break;
            case BuffType.HEAL:
                float percentHeal = skill.GetBuffValue();
                mainCharacter.Healing(percentHeal);
                break;
            case BuffType.REDUCE_DAMAGE:
                stats.ReduceDamage = 1f + skill.GetBuffValue();
                break;
            case BuffType.EXP:
                stats.BonusExp = 1f + skill.GetBuffValue();
                break;
            case BuffType.GOLD:
                stats.BonusGold = 1f + skill.GetBuffValue();
                break;
            case BuffType.EXTRA_TIME_SKILL:
                stats.ExtraTimeSkill = 1f + skill.GetBuffValue();
                break;
            default:
                break;
        }
    }
}
