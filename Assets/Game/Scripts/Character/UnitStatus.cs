using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UnitStatus 
{
    public int HitPoint;
    public int Attack;
    public int CriticalChance;
    public float CriticalDamage;
    public int Defense;
    public float MoveSpeed;


}
[System.Serializable]

public class SkillStatus
{
    public  float BulletMoveSpeedRatio = 1f;
    public  float BulletScaleRatio = 1f;
    public  float HpRatio = 1f;
    public int RegenPerSecond = 0;
    public  float AttackRatio = 1f;
    public float ReduceTimeCoolDown = 1f;
    public float ExtraTimeSkill = 1f;
    public  float MoveSpeedRatio = 1f;
    public  int CriticalRate = 0;
    public float RangeCollectorRatio = 1f;
    public float ReduceDamage = 1f;
    public float BonusExp = 1f;
    public float BonusGold = 1f;
}