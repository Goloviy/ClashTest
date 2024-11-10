using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterStatusHelper 
{
    public static int CalculateHitPoint(UnitStatus characterStatus, SkillHandle skillStatus)
    {
        return Mathf.RoundToInt(characterStatus.HitPoint * skillStatus.stats.HpRatio);
    }
    public static int CalculateDamage(float multiplyDamageBySkill,UnitStatus characterStatus, SkillHandle skillStatus)
    {
        return Mathf.RoundToInt(characterStatus.Attack * skillStatus.stats.AttackRatio * multiplyDamageBySkill);
    }
    public static int CalculateCriticalChance(UnitStatus characterStatus, SkillHandle skillStatus)
    {
        return characterStatus.CriticalChance + skillStatus.stats.CriticalRate;
    }
 
    public static float CalculateCharacterMoveSpeed(UnitStatus characterStatus, SkillHandle skillStatus)
    {
        return characterStatus.MoveSpeed * skillStatus.stats.MoveSpeedRatio;
    }
    public static float CalculateBulletMoveSpeed(float speedBase, SkillBase skillBase)
    {
        return speedBase * skillBase.CurMultiplySpeed * skillBase.skillHandle.stats.BulletMoveSpeedRatio;
    }
    public static float CalculateBulletMoveTime(float moveTime, SkillBase skillBase)
    {
        return moveTime / skillBase.CurMultiplySpeed / skillBase.skillHandle.stats.BulletMoveSpeedRatio;
    }
    public static float CalculateBulletScale(SkillBase skillBase)
    {
        return skillBase.CurMultiplyScale * skillBase.skillHandle.stats.BulletScaleRatio;
    }
    //public static int CalculateMonsterDamage(int baseDamage, int level)
    //{
    //    return baseDamage + Mathf.RoundToInt((level - 1) * 0.2f * baseDamage);

    //}
    public static int CalculateMonsterHp(int baseHp, int level)
    {
        return baseHp + Mathf.RoundToInt((level - 1) * 0.2f * baseHp);

    }
}
