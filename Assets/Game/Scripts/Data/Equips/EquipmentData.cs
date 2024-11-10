using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/EquipmentData", order = 1)]

public class EquipmentData : ItemBagData
{
    public bool isSuper = false;
    [BoxGroup("Stats")]
    public int hpBase;
    [BoxGroup("Stats")]
    public int atkBase;
    [BoxGroup("Stats")]
    public int defBase;
    [BoxGroup("Stats")]
    public int critRateBase;
    [BoxGroup("Stats")]
    public int critDamageBase;
    public SkillName passiveSkill = SkillName.NONE;
    public GradeSkillData[] gradeSkills;
}
