using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillDataObject", order = 1)]

public class SkillData : ScriptableObject
{
    [PreviewField(50, ObjectFieldAlignment.Left)]
    public Sprite spriteIcon;
    public SkillName type;
    public GroupSkill group;
    public int maxLevel = 5;
    public string nameSkill;
    public string descriptions;
    public List<SkillName> supportEvol;
    public List<RequireLearn> requireLearns;
    public bool evolved;
    [ShowIf("evolved", true)]
    public SkillName[] originSkills;
    public List<string> descriptionsLevelup = new List<string>(5);
}
[System.Serializable]
public class RequireLearn
{
    public SkillName requireSkill;
    public int requirelevel;
}