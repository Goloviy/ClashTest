using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterSkill 
{
    int Level { get;}
    SkillName GetSkillName();
    SkillType GetSkillType();
    BuffType GetBuff();
    bool IsPassiveSkill { get;}
    float GetBuffValue();
    void InflictDamage(UnitBase triggerTarget, float scaleDamage = 1f, SkillName skill = SkillName.NONE);
    
    void Init(MainCharacter owner);
    void LevelUp();
    void SetDisable();
}
