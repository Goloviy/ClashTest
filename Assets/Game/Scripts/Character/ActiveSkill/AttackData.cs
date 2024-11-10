using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackData 
{
    public UnitBase Owner;
    public int attack;
    public int criticalChance;
    public float criticalDamage;
    public SkillName skill;
    public AttackData(UnitBase owner, int attack, int criticalChance, float criticalDamage = 1.5f, SkillName skill = SkillName.NONE)
    {
        this.Owner = owner;
        this.attack = attack;
        this.criticalChance = criticalChance;
        this.criticalDamage = criticalDamage;
        this.skill = skill;
    }
}
public struct AttackDataProcessed
{
    public UnitBase Owner;
    public int Damage;
    public bool IsCritical;
    public SkillName skill;
    public AttackDataProcessed(UnitBase owner, int damage, bool isCritical, SkillName skill = SkillName.NONE)
    {
        Owner = owner;
        Damage = damage;
        IsCritical = isCritical;
        this.skill = skill;
    }
}