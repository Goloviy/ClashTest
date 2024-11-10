using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class UnitBase : MonoBehaviour, IAlive
{
    [SerializeField]
    protected string _name;
    protected Collider2D collider;
    protected Rigidbody2D rigid;
    public UnitStatus unitStatus;
    //public int Attack { get => unitStatus.Attack * }
    public int HpRemainning { get; set; }
    public int HpMax { get; set; }
    public float moveSpeed;
    /// <summary>
    /// Floating Damage system
    /// </summary>
    protected Transform prefabFloatingDamage;
    protected Transform prefabFloatingDamageCritical;
    protected Transform prefabHealingDamage;
    public static string  PathFloatingDamage = "Props/FloatingDamage";
    public static string  PathFloatingDamageCritical = "Props/FloatingDamageCritical";
    public static string  PathHealingDamage = "Props/HealingDamage";
    protected virtual void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        rigid = GetComponent<Rigidbody2D>();
        rigid.simulated = false;
        prefabFloatingDamage = Resources.Load<Transform>(PathFloatingDamage);
        prefabFloatingDamageCritical = Resources.Load<Transform>(PathFloatingDamageCritical);
        prefabHealingDamage = Resources.Load<Transform>(PathHealingDamage);
    }
    public virtual int GetAttack()
    {
        return unitStatus.Attack;
    }
    public virtual void Init()
    {

        OnCreate();
    }
    protected virtual void OnCreate()
    {
        HpMax = unitStatus.HitPoint;
        HpRemainning = HpMax;
        moveSpeed = unitStatus.MoveSpeed;
    }

    protected virtual void Move()
    {

    }
    public virtual void InflictDamage(float skillMulplyDamage, UnitBase victim, SkillName skill = SkillName.NONE)
    {

    }
    public virtual void ShowDamage(int Damage, bool IsCritical)
    {
        if (GameDynamicData.curUnitInflictDamage == Damage)
        {

        }
        else
        {
            GameDynamicData.curUnitInflictDamage = Damage;
            if (!GameDynamicData.dictStringDamage.ContainsKey(Damage))
            {
                GameDynamicData.dictStringDamage.Add(Damage, Damage.ToString());
            }
        }
        var prefab = !IsCritical ? prefabFloatingDamage : prefabFloatingDamageCritical;
        PoolManager.Pools[StringConst.POOL_PROPS_NAME].Spawn(prefab, this.transform.position + Vector3.up * 0.3f, prefabFloatingDamage.rotation);

    }
    public virtual void ShowHealing(int healAmount)
    {
        GameDynamicData.healAmount = healAmount;
        PoolManager.Pools[StringConst.POOL_PROPS_NAME].Spawn(prefabHealingDamage, this.transform.position + Vector3.up * 0.3f, prefabHealingDamage.rotation);
    }
    public virtual AttackDataProcessed CreateAttackProcessed(AttackData attackData)
    {
        int damage;
        bool isCrit = CalculateDamageByCritical(attackData.attack, attackData.criticalChance,
            attackData.criticalDamage, out damage);
        return new AttackDataProcessed(attackData.Owner, damage, isCrit, attackData.skill);
    }
    protected bool CalculateDamageByCritical(int inputDamage, int criticalChange, float criticalDamage, out int outputDamage)
    {
        var rd = Random.Range(0, 100);
        if (rd < criticalChange)
        {
            outputDamage = Mathf.RoundToInt(inputDamage * criticalDamage);
            return true;
        }
        else
            outputDamage = inputDamage;
        return false;
    }
    public virtual void TakeDamage(UnitBase owner, int attack, int criticalChance, float criticalDamage = 1.5f, SkillName skill = SkillName.NONE)
    {

    }
    public virtual void Die()
    {

    }

    public virtual void Revive()
    {

    }
    public virtual bool IsAlive => HpRemainning > 0;    
}
