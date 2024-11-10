using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : MonoBehaviour, ICharacterSkill
{
    public int Level => curLevel;
    public SkillName skillName;
    public SkillType type;
    public BuffType buffType;
    public List<float> bonusValues;
    private int curLevel;
    public int levelMax = 5;
    public SkillName GetSkillName() => skillName;
    public SkillType GetSkillType() => type;
    public BuffType GetBuff() => buffType;
    public float GetBuffValue() => bonusValues[curLevel <= bonusValues.Count ? curLevel - 1 : 0];

    public bool IsPassiveSkill => type == SkillType.PASSIVE;

    public void InflictDamage(UnitBase triggerTarget, float scaleDamage = 1f, SkillName skill = SkillName.NONE)
    {
        
    }

    public void Init(MainCharacter owner)
    {
        
    }

    public void LevelUp()
    {
        if (curLevel >= levelMax)
        {
            DebugCustom.Log("Max Level");
            return;
        }
        curLevel++;
    }

    public virtual void SetDisable()
    {
        this.gameObject.SetActive(false);
    }
}
