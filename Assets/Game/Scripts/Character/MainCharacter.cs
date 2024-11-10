using PathologicalGames;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]

public class MainCharacter : UnitBase, ICanUpLevel
{

    public SkillHandle skillHandle;
    [SerializeField] protected SkeletonAnimation skeAnim;
    [SpineAnimation(dataField: "skeAnim")]
    public string animIdle = "";
    [SpineAnimation(dataField: "skeAnim")]
    public string animRun = "";
    [SpineAnimation(dataField: "skeAnim")]
    public string animDeath = "";
    protected Vector3 MoveDirection = Vector3.zero;
    bool isMoving = false;
    
    public FindOpponentSystem findOpponentSystem;
    public CollectorBase collectorBase;

    public GameObject goDamage;
    public int CurrentExp => currExp;
    public int CurrentLevel => level;
    /// <summary>
    /// Amount exp to up level will change by bonusExp (reduce by percen)
    /// </summary>
    public int NextLevelExp => Mathf.RoundToInt(ExpLevels[level - 1 < maxLevel - 1? level - 1 : maxLevel - 2] * skillHandle.stats.BonusExp);

    private int currExp = 0;
    private int level = 1;
    private int maxLevel = 99;
    public int[] ExpLevels;
    [Header("References")]
    [SerializeField] private GameObject prefabFxLevelUp;

    protected override void Awake()
    {
        base.Awake();
        skillHandle = GetComponent<SkillHandle>();
        collectorBase = GetComponentInChildren<CollectorBase>();

    }
    protected void Start()
    {
        maxLevel = GameConfigData.Instance.MaxLevel;
        collider.enabled = true;
        rigid.simulated = true;
        Init();
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_REVIVE, OnRevive);
        EventDispatcher.Instance.RegisterListener(EventID.SKILL_LEVEL_UP_AFTER, OnAfterLevelupSkill);
    }

    private void OnAfterLevelupSkill(Component arg1, object arg2)
    {

        //handle hp
        var hpMax = CharacterStatusHelper.CalculateHitPoint(unitStatus, skillHandle);
        var extraHp = hpMax - HpMax;
        HpMax = hpMax;
        HpRemainning = HpRemainning + extraHp;

        //handle movespeed
        moveSpeed = CharacterStatusHelper.CalculateCharacterMoveSpeed(unitStatus, skillHandle);

    }

    private void OnRevive(Component arg1, object arg2)
    {
        Revive();
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_REVIVE, OnRevive);
        EventDispatcher.Instance.RemoveListener(EventID.SKILL_LEVEL_UP_AFTER, OnAfterLevelupSkill);

    }
    protected override void OnCreate()
    {
        currExp = 0;
        level = 1;
        collectorBase.Init(this);
        skillHandle.Init(this);
        GameDynamicData.mainCharacter = this;
        ApplyStat();
        ApplyEquipmentSkill();
#if UNITY_EDITOR
        Test();
#endif

    }
    public override int GetAttack()
    {
        return Mathf.RoundToInt( unitStatus.Attack * skillHandle.stats.AttackRatio);
    }
    protected void ApplyStat()
    {
        unitStatus.Attack = GameData.Instance.playerData.tempData.PlayerTotalStat.atk;
        unitStatus.HitPoint = GameData.Instance.playerData.tempData.PlayerTotalStat.hp;
        unitStatus.MoveSpeed = GameData.Instance.playerData.tempData.PlayerTotalStat.moveSpeed;
        unitStatus.Defense = GameData.Instance.playerData.tempData.PlayerTotalStat.def;
        unitStatus.CriticalChance = GameData.Instance.playerData.tempData.PlayerTotalStat.critRate;
        unitStatus.CriticalDamage = GameData.Instance.playerData.tempData.PlayerTotalStat.critDamage;
        
        HpMax = CharacterStatusHelper.CalculateHitPoint(unitStatus, skillHandle);
        HpRemainning = HpMax;
        moveSpeed = CharacterStatusHelper.CalculateCharacterMoveSpeed(unitStatus, skillHandle);

        ExpLevels = GameConfigData.Instance.ExpLevel;
    }
    protected void ApplyEquipmentSkill()
    {

        if (GameData.Instance.playerData.tempData.equipSkills.Count <= 0)
        {
            LearnSkill(GameConfigData.Instance.DefaultSkill);
        }
        else
        {
            foreach (var passiveSkill in GameData.Instance.playerData.tempData.equipSkills)
            {
                LearnSkill(passiveSkill);
            }
        }
        if (GameData.Instance.playerData.tempData.equipBonusSkills.Count > 0)
        {
            foreach (var bonusSkill in GameData.Instance.playerData.tempData.equipBonusSkills)
            {
                LearnSkill(bonusSkill);
            }
        }
    }
    private void Test()
    {
        for (int i = 0; i < GameConfigData.Instance.TestSkills.Length; i++)
        {
            skillHandle.LearnSkill(GameConfigData.Instance.TestSkills[i]) ;
        }
    }
    private void Update()
    {
        UpdateDirect();
    }
    private void FixedUpdate()
    {
        Move();
    }
    public void UpdateDirect()
    {
        MoveDirection = new Vector3(UltimateJoystick.GetHorizontalAxis("Movement"), UltimateJoystick.GetVerticalAxis("Movement"), 0).normalized;
    }
    protected override void Move()
    {
        if (!IsAlive)
            return;
        if (MoveDirection != Vector3.zero)
        {
            this.transform.MoveTransformWithPhysic(MoveDirection, CharacterStatusHelper.CalculateCharacterMoveSpeed( unitStatus, skillHandle));
            SetAnimMove(true);
            UpdateFlip(MoveDirection.x < 0);
        }
        else
        {
            SetAnimMove(false);
        }
    }
    bool isFlipLeft = true;
    private void UpdateFlip(bool isLeft)
    {
        if (isFlipLeft == isLeft)
            return;
        isFlipLeft = isLeft;
        skeAnim.transform.localScale = new Vector3(skeAnim.transform.localScale.x * -1f, 1, 1);
    }
    private void SetAnimMove(bool _isMoving, bool forceChange = false)
    {
        if (!forceChange && ( !IsAlive || (isMoving == _isMoving)))
            return;
        isMoving = _isMoving;
        skeAnim.AnimationState.SetAnimation(0, _isMoving ? animRun : animIdle, true);
        skeAnim.AnimationState.TimeScale = 1f;
    }
    public override void Die()
    {
        skeAnim.AnimationState.SetAnimation(0,animDeath, false);
        skeAnim.AnimationState.TimeScale = 2f;
        EventDispatcher.Instance.PostEvent(EventID.CHARACTER_DIE);
    }
    public override void Revive()
    {
        if (IsAlive)
            return;
        HpRemainning = HpMax;
        SetAnimMove(false, true);
    }
    public override void InflictDamage(float skillMulplyDamageBySkill, UnitBase victim, SkillName skill)
    {
        var damage = CharacterStatusHelper.CalculateDamage(skillMulplyDamageBySkill, unitStatus, skillHandle);
        var criticalChange = CharacterStatusHelper.CalculateCriticalChance(unitStatus, skillHandle);
        var criticalDamage = unitStatus.CriticalDamage;

        victim.TakeDamage(this, damage, criticalChange, criticalDamage);
    }
    public override void TakeDamage(UnitBase owner, int attack, int criticalChance, float criticalDamage = 1.5f, SkillName skill = SkillName.NONE)
    {
        if (IsAlive)
        {
            //var attackDataP = CreateAttackProcessed(attackData);
            int damage;
            bool isCrit = CalculateDamageByCritical(attack, criticalChance,
                criticalDamage, out damage);
            HpRemainning -= Mathf.RoundToInt(damage * skillHandle.stats.ReduceDamage);
            ShowDamage(damage, isCrit);
            if (HpRemainning <= 0)
            {
                Die();
            }
            else
            {
                //PoolManager.Pools[StringConst.POOL_FX_NAME].Spawn(prefabParticleBlood,this.transform.position, prefabParticleBlood.transform.rotation);
                goDamage.gameObject.SetActive(true);
                this.StartDelayAction(0.15f, () => { goDamage.gameObject.SetActive(false); });
                //spawn blood n vibrate
                Vibration.VibratePop();
            }
        }

    }


    #region COLLECT
    public void CollectItem(DropItem itemType, int amount = 1)
    {
        if (itemType == DropItem.EXP)
        {
            CollectExp(amount);
        }
        else if (itemType == DropItem.MEAT)
        {
            CollectMeat();
        }
        else if (itemType == DropItem.GOLD)
        {
            CollectGold(amount);
        }
    }
    private void CollectGold(int amount = 1000)
    {
        int total = Mathf.RoundToInt(amount * skillHandle.stats.BonusGold);
        GameDynamicData.GoldReceived += total;
        EventDispatcher.Instance.PostEvent(EventID.CHARACTER_TAKE_GOLD, total);
    }
    private void CollectMeat()
    {
        Healing(0.3f);
    }
    public void Healing(float percent)
    {
        if (!IsAlive)
            return;
        var value = Mathf.RoundToInt(HpMax * percent);
        HpRemainning += value;
        HpRemainning = HpRemainning >= HpMax ? HpMax : HpRemainning;
        ShowHealing(value);
    }
    private void CollectExp(int amount)
    {
        if (!IsAlive)
            return;
        amount = amount * GameConfigData.Instance.MultiplyExp;
        currExp += amount;
        while (currExp >= NextLevelExp)
        {
            var redundancy = currExp - NextLevelExp;
            currExp = redundancy;
            Levelup();
        }
        EventDispatcher.Instance.PostEvent(EventID.CHARACTER_TAKE_EXP);
    }
    #endregion COLLECT

    public void Levelup()
    {
        if (level < maxLevel)
        {
            level++;
            //show effect levelUp
            var fx = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(prefabFxLevelUp, this.transform.position, Quaternion.identity, this.transform);
            this.StartDelayAction(0.5f, () =>
            {
                PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(fx);
            });
            //show popup select skill
            EventDispatcher.Instance.PostEvent(EventID.CHARACTER_LEVELUP);
        }
    }
    public void LearnSkill(SkillName skill)
    {
        if (!IsAlive)
            return;
        skillHandle.LearnSkill(skill);
    }
}
