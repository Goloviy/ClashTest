using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillsStaticData
{
    public List<SkillData> Skills => skills;
    List<SkillData> skills;
    Dictionary<SkillName, SkillData> dictSkill;
    public SkillsStaticData()
    {
        skills = Resources.LoadAll<SkillData>(StringConst.PATH_SKILL_DATA).ToList();
        dictSkill = new Dictionary<SkillName, SkillData>();
        foreach (var skill in skills)
        {
            dictSkill.Add(skill.type, skill);
        }
        
    }

    public SkillData[] GetDefaultSkill()
    {
        return new SkillData[1] { dictSkill[SkillName.DEFAULT_MEAT] };
    }

    public SkillData GetSkill(SkillName type)
    {
        SkillData skillData; 
        if (dictSkill.TryGetValue(type, out skillData))
        {
            return skillData;
        }
        else
        {
            return null;
        }
    }

}
