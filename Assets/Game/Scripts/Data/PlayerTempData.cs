using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTempData
{
    public PlayerStat PlayerTotalStat
    {
        get
        {
            playerTotalStat.hp = baseStat.hp + equipStat.hp + buffStat.hp + talentStat.hp;
            playerTotalStat.atk = baseStat.atk + equipStat.atk + buffStat.atk + talentStat.atk;
            playerTotalStat.moveSpeed = baseStat.moveSpeed + equipStat.moveSpeed + buffStat.moveSpeed + talentStat.moveSpeed;
            playerTotalStat.def = baseStat.def + equipStat.def + buffStat.def + talentStat.def;
            playerTotalStat.critDamage = baseStat.critDamage + equipStat.critDamage + buffStat.critDamage + talentStat.critDamage;
            playerTotalStat.critRate = Math.Min( 100, 
                baseStat.critRate + equipStat.critRate + buffStat.critRate + talentStat.critRate);
            return playerTotalStat;
        }
    }
    PlayerStat playerTotalStat = new PlayerStat();
    public PlayerStat baseStat = new PlayerStat() { 
        hp = GameConfigData.Instance.StartHp, 
        atk = GameConfigData.Instance.StartAttack, 
        moveSpeed = GameConfigData.Instance.StartMoveSpeed, 
        critDamage = GameConfigData.Instance.StartCriticalDamage,
        critRate = GameConfigData.Instance.StartCriticalRate};
    public PlayerStat equipStat = new PlayerStat();
    public PlayerStat buffStat = new PlayerStat();
    public PlayerStat talentStat = new PlayerStat();
    public List<SkillName> equipSkills = new List<SkillName>();
    public List<SkillName> equipBonusSkills = new List<SkillName>();
    public int CountRefreshSkill { get; private set; }
    public int Level { get; private set; }
    Dictionary<SkillName, int> dictSkill;
    List<SkillName> listAttackSkill;
    public List<SkillName> ListAttackSkill => listAttackSkill;
    List<SkillName> listSupportSkill;
    public List<SkillName> ListSupportSkill => listSupportSkill;
    List<SkillName> listDeleteSkill;
    int maxSlotSkill = 6;
    public PlayerTempData()
    {
        dictSkill = new Dictionary<SkillName, int>();
        listAttackSkill = new List<SkillName>();
        listSupportSkill = new List<SkillName>();
        listDeleteSkill = new List<SkillName>();
        CountRefreshSkill = 1;
        Level = 1;
    }

    public Dictionary<SkillName, int> LearnedSkill => dictSkill;
    public void Clear()
    {
        listAttackSkill.Clear();
        listSupportSkill.Clear();
        listDeleteSkill.Clear();
        CountRefreshSkill = 1;
        Level = 1;
        dictSkill.Clear();
    }
    public bool RefreshSkillAvailable => CountRefreshSkill >= 1;
    public void RefreshSkill()
    {
        CountRefreshSkill--;
    }

    public List<SkillName> GetSkillLearnedCanUpLevel()
    {
        List<SkillName> list = new List<SkillName>();
        
        foreach (var key in dictSkill.Keys)
        {
            var skillData = GameData.Instance.staticData.skillsData.GetSkill(key);
            if (dictSkill[key] < skillData.maxLevel)
            {
                list.Add(key);
            }
        }
        return list;
    }
    public void LearnSkill(SkillName type)
    {
        Level++;
        int skillLevel;
        if (dictSkill.TryGetValue(type, out skillLevel))
        {
            dictSkill[type] = skillLevel + 1;
        }
        else
        {
            dictSkill.Add(type, 1);
        }
        var data = GameData.Instance.staticData.skillsData.GetSkill(type);
        //Neu la skill tien hoa => xoa skill cap thap cua no , dua vao list de khong hoc lai
        if (data.evolved)
        {

            foreach (var skill in data.originSkills)
            {
                //remove skill origin
                listAttackSkill.Remove(skill);
                listDeleteSkill.Add(skill);
            }
        }

        //add to group
        if (data.group == GroupSkill.ACTIVE)
        {
            if (!listAttackSkill.Contains(type))
            {
                listAttackSkill.Add(type);
            }
        }
        else if (data.group == GroupSkill.PASSIVE)
        {
            if (!listSupportSkill.Contains(type))
            {
                listSupportSkill.Add(type);

            }
        }
        
    }
    public SkillData[] GetRandom3SkillToLearn()
    {
        var list = GetSkillsCanLearn();
        var result = list.OrderBy(x => Guid.NewGuid()).Take(3).ToArray();
        if (result.Length == 0)
            result = GameData.Instance.staticData.skillsData.GetDefaultSkill();
        return result;
    }
    public int GetLevelSkill(SkillName type)
    {
        int skillLevel;
        if (!dictSkill.TryGetValue(type, out skillLevel))
            skillLevel = 0;
        return skillLevel;
    }
    /// <summary>
    /// return false nghia la skill khong du dieu kien hoc
    /// </summary>
    private bool CheckConditionLearn(SkillData skillData)
    {
        List<SkillName> list = skillData.group == GroupSkill.ACTIVE ? listAttackSkill : listSupportSkill;
        //kiem tra xem da hoc skill nay chua
        bool isLearned = list.Contains(skillData.type);
        //kiem tra xem danh sach skill da max chua
        bool isMaxSlotSkill = list.Count >= maxSlotSkill;
        //kiem tra xem co phai skill duoc tien hoa khong
        bool isEvolSkill = skillData.evolved;
        //kiem tra xem max level chua
        bool isMaxLevel = GetLevelSkill(skillData.type) >= skillData.maxLevel;
        //kiem tra xem co phai skill mac dinh (Meat default)
        bool isDefault = skillData.type == SkillName.DEFAULT_MEAT;
        //kiem tra xem co du dieu kien skill nen` tang
        bool isEnoughBase = true;
        //DebugCustom.LogFormat("skill : {0} ,isLearned {1}, isMaxSlotSkill {2}, isEvolSkill  {3}, isMaxLevel  {4}, isDefault {5}, isEnoughBase {6}", skillData.nameSkill, isLearned, isMaxSlotSkill, isEvolSkill, isMaxLevel, isDefault, isEnoughBase);
        foreach (var require in skillData.requireLearns)
        {
            if (GetLevelSkill(require.requireSkill) < require.requirelevel)
            {
                isEnoughBase = false;
                break;
            }
        }
        //kiem tra xem da hoc tien hoa chua 
        bool hasLearnEvol = listDeleteSkill.Contains(skillData.type);

        if (!isLearned && isMaxSlotSkill && !isEvolSkill)
        {
            return false;
        }
        else if (isLearned && isMaxLevel)
        {
            return false;
        }
        else if (isDefault || !isEnoughBase || hasLearnEvol)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }
    public List<SkillData> GetSkillsCanLearn()
    {

        List<SkillData> listCanLearn = new List<SkillData>();
        var skills = GameData.Instance.staticData.skillsData.Skills;
        foreach (var skill in skills)
        {
            if (CheckConditionLearn(skill))
            {
                listCanLearn.Add(skill);
            }
        }
        return listCanLearn;

    }
}
