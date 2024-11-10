using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Grade Skill", menuName = "ScriptableObjects/GradeSkill", order = 1)]

public class GradeSkillData : BaseScriptObject
{
    public GradeSkill gradeSkill;
    [ShowIf("gradeSkill", GradeSkill.LEARN_ONE_SKILL)]
    public SkillName bonusSkill = SkillName.NONE;
    public Rarity rarity;
    public string descriptions;
    public float value;
}
