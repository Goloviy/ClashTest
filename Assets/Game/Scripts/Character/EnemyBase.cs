using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyBase : UnitBase
{
    [Header("Normal Attack Setting")]
    [SerializeField] protected float deltaTimeNormalAttack = 0.25f;
    //protected AttackData CollisionAttackData;
    public int id;
    protected float originMoveSpeed = 0.5f;
    protected override void Awake()
    {
        base.Awake();
    }
    protected virtual void OnSpawned(SpawnPool pool)
    {
        Init();
        var eAtk = SpawnerMananger.Instance.GetAttack(id);
        //CollisionAttackData = new AttackData(this, eAtk, 0);
    }
    protected virtual void OnDespawned(SpawnPool pool)
    {
        collider.enabled = false;
        rigid.simulated = false;
    }
   

    public virtual void DespawnThis()
    {
        if (PoolManager.Pools[StringConst.POOL_OPPONENT_NAME].IsSpawned(this.transform))
        {
            PoolManager.Pools[StringConst.POOL_OPPONENT_NAME].Despawn(this.transform);
        }
    }
    public override void TakeDamage(UnitBase owner, int attack, int criticalChance, float criticalDamage = 1.5f, SkillName skill = SkillName.NONE)
    {
        if (IsAlive)
        {
            int damage;
            bool isCrit = CalculateDamageByCritical(attack, criticalChance,
                criticalDamage, out damage); 
            HpRemainning -= damage;
            if (HpRemainning <= 0)
            {
                collider.enabled = false;
                rigid.simulated = false;
                Die();
            }
            ShowDamage(damage, isCrit);
        }
    }

    #region MONSTER TAKE DAMAGE 

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        ICharacterSkill skill = null;
        int countDamage = 1;
        float multiplyDamage = 1f;

        if (collision.CompareTag(StringConst.TAG_PISTOL))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.PISTOL);
            Knockback();

        }
        else if (collision.CompareTag(StringConst.TAG_PISTOL_S))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.PISTOL_S);
            Knockback();

        }
        else if (collision.CompareTag(StringConst.TAG_KNIFE))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.KNIFE);
            Knockback();


        }
        else if (collision.CompareTag(StringConst.TAG_KNIFE_S))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.KNIFE_S);
            Knockback();


        }
        else if (collision.CompareTag(StringConst.TAG_YOYO))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.YOYO);
            Knockback();


        }
        else if (collision.CompareTag(StringConst.TAG_YOYO_S))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.YOYO_S);
            Knockback();
        }
        else if (collision.CompareTag(StringConst.TAG_LIGHTNING))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.CHAIN_LIGHTNING);
            Knockback();

        }
        else if (collision.CompareTag(StringConst.TAG_LIGHTNING_S))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.CHAIN_LIGHTNING_S);
            Knockback();

        }
        else if (collision.CompareTag(StringConst.TAG_SHIELD))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.DANCING_SWORD);
            Knockback();
        }
        else if (collision.CompareTag(StringConst.TAG_SHIELD_S))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.DANCING_SWORD_S);
            Knockback();
        }
        else if (collision.CompareTag(StringConst.TAG_SHIELD_S2))
        {
            countDamage = 3;
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.DANCING_SWORD_S);
            Knockback();

        }
        else if (collision.CompareTag(StringConst.TAG_BRICK))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.BRICKS);
        }
        else if (collision.CompareTag(StringConst.TAG_BOOMERANG))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.BOOMERANG);
        }
        else if (collision.CompareTag(StringConst.TAG_DRILL))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.DRILL);
        }
        else if (collision.CompareTag(StringConst.TAG_DRILL_S))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.DRILL_S);
        }
        else if (collision.CompareTag(StringConst.TAG_BALL))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.BALL);
        }
        else if (collision.CompareTag(StringConst.TAG_SHOTGUN))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.SHOTGUN);
        }

        else if (collision.CompareTag(StringConst.TAG_SWORD))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.SWORD);
            Knockback(150,2f);
        }
        else if (collision.CompareTag(StringConst.TAG_SWORD_S))
        {
            countDamage = 4;
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.SWORD_S);
            Knockback(150, 2f);

        }
		else if (collision.CompareTag(StringConst.TAG_L_KATANA))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.L_KATANA);
            SlowMove(500,0);
        }
        else if (collision.CompareTag(StringConst.TAG_L_KATANA_S))
        {
            countDamage = 3;
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.L_KATANA_S);
			Burning(SkillName.L_KATANA_S, 500, 100);

            SlowMove(500,0);

        }
        else if (collision.CompareTag(StringConst.TAG_GRENADE))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.GRENADE_LAUNCHER);
            Knockback();
        }
        else if (collision.CompareTag(StringConst.TAG_GRENADE_S))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.GRENADE_LAUNCHER_S);
            Knockback(100,4f);
        }
        else if (collision.CompareTag(StringConst.TAG_SONIC_BOOM))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.SONIC_BOOM);
            SlowMove(500);
        }
        else if (collision.CompareTag(StringConst.TAG_SONIC_BOOM_S))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.SONIC_BOOM_S);
            SlowMove(500);
        }
        else if (collision.CompareTag(StringConst.TAG_STORM))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.STORM);
            SlowMove(300);

        }
        else if (collision.CompareTag(StringConst.TAG_STORM_S))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.STORM_S);
            SlowMove(300);
        }
        if (collision.CompareTag(StringConst.TAG_STORM_S_TICK))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.STORM_S);
            lastTimeTakeDamageFromStorm = Time.time;
            multiplyDamage = 0.3f;
        }
        else if (collision.CompareTag(StringConst.TAG_LASER))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.LASER);
            lastTimeTakeDamageFromLaser = Time.time;
        }
        else if (collision.CompareTag(StringConst.TAG_LASER_S))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.LASER_S);
            lastTimeTakeDamageFromLaser = Time.time;
        }
        else if (collision.CompareTag(StringConst.TAG_RAIN_FIRE))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.RAIN_FIRE);
            Burning(SkillName.RAIN_FIRE, 1000, 250);
        }
        else if (collision.CompareTag(StringConst.TAG_RAIN_ICE))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.RAIN_ICE);
            SlowMove(800);
        }
        else if (collision.CompareTag(StringConst.TAG_SUPER_METEOR))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.MEGA_METEOR);
            Knockback();
            SlowMove(1500);
            //countDamage = 3;
        }
        //else if (collision.CompareTag(StringConst.TAG_DOCTOR_STRANGE_2))
        //{
        //    skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.MEGA_METEOR);
        //    lastTimeTakeDamageFromChain = Time.time;
        //}
        else if (collision.CompareTag(StringConst.TAG_CLAW))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.CLAW);
            Burning(SkillName.CLAW, 2000, 250);
            Knockback();
        }
        else if (collision.CompareTag(StringConst.TAG_CLAW_S))
        {
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.CLAW_S);
            Burning(SkillName.CLAW_S, 2000, 250);
            Knockback();
        }
        else if (collision.CompareTag(StringConst.TAG_FIRE_BALL))
        {
            lastTimeTakeDamageFromFireBall = Time.time;
            skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.FIRE_BALL);
            Knockback();
        }

        //monster attack monster
        else if (collision.CompareTag(StringConst.TAG_MONSTER_SELF_KILL))
        {
            var damage = GameDynamicData.mainCharacter.GetAttack();

            this.TakeDamage(this, damage, 0);
            skill = null;
        }
        if (skill != null)
        {
            for (int i = 0; i < countDamage; i++)
            {
                skill.InflictDamage(this, multiplyDamage, skill.GetSkillName());
            }
            AfterTakeDamage();
        }
    }
    protected virtual void AfterTakeDamage()
    {

    }
    protected async void Burning(SkillName skillName ,int time = 2000, int cd = 250)
    {
        var skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(skillName);
        int count = time / cd;
        for (int i = 0; i < count; i++)
        {
            await Task.Delay(cd);
            skill.InflictDamage(this, 0.4f, skill.GetSkillName());
        }
    }
    protected async void SlowMove(int time = 350, float multiplySpeed = 0.3f)
    {
        moveSpeed = originMoveSpeed;
        moveSpeed *= multiplySpeed;
        await Task.Delay(time);
        moveSpeed = originMoveSpeed;
    }
    protected async void Knockback(int time = 150, float force = 3f) 
    {

        moveSpeed = originMoveSpeed;
        moveSpeed = -1f * force;
        await Task.Delay(time);
        moveSpeed = originMoveSpeed;
    }

    protected float deltaTime = 0.25f;
    protected float deltaTime2 = 0.5f;
    protected float deltaTime3 = 0.4f;
    protected float lastTimeTakeDamageFromBottleGas = Mathf.NegativeInfinity;
    protected float lastTimeTakeDamageFromFireBall = Mathf.NegativeInfinity;
    protected float lastTimeTakeDamageFromGravity = Mathf.NegativeInfinity;
    protected float lastTimeTakeDamageFromStorm = Mathf.NegativeInfinity;
    protected float lastTimeTakeDamageFromLaser = Mathf.NegativeInfinity;
    protected float lastTimeTakeDamageFromChain = Mathf.NegativeInfinity;
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        ICharacterSkill skill = null;
        float multiplyDamage = 1f;
        if (collision.CompareTag(StringConst.TAG_BOTTLE_GAS))
        {
            if (Time.time - lastTimeTakeDamageFromBottleGas > deltaTime)
            {
                skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.BOTTLE_GAS);
                lastTimeTakeDamageFromBottleGas = Time.time;
                multiplyDamage = 0.5f;

            }
        }
        if (collision.CompareTag(StringConst.TAG_GRAVITY_FIELD))
        {
            if (Time.time - lastTimeTakeDamageFromGravity > deltaTime2)
            {
                skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.GRAVITY_FIELD);
                lastTimeTakeDamageFromGravity = Time.time;
                multiplyDamage = 0.5f;
            }
        }
        if (collision.CompareTag(StringConst.TAG_STORM))
        {
            if (Time.time - lastTimeTakeDamageFromStorm > deltaTime3)
            {
                skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.STORM);
                lastTimeTakeDamageFromStorm = Time.time;
                multiplyDamage = 1f;
            }
        }
        if (collision.CompareTag(StringConst.TAG_STORM_S_TICK))
        {
            if (Time.time - lastTimeTakeDamageFromStorm > deltaTime)
            {
                skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.STORM_S);
                lastTimeTakeDamageFromStorm = Time.time;
                multiplyDamage = 1f;
            }
        }

        //else if (collision.CompareTag(StringConst.TAG_DOCTOR_STRANGE_2))
        //{
        //    if (Time.time - lastTimeTakeDamageFromChain > deltaTime2)
        //    {
        //        skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.DOCTOR_STRANGE);
        //        lastTimeTakeDamageFromChain = Time.time;
        //    }
        //}
        else if (collision.CompareTag(StringConst.TAG_FIRE_BALL))
        {
            if (Time.time - lastTimeTakeDamageFromFireBall > deltaTime)
            {
                skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.FIRE_BALL);
                lastTimeTakeDamageFromFireBall = Time.time;
            }

        }
        else if (collision.CompareTag(StringConst.TAG_LASER_S))
        {
            if (Time.time - lastTimeTakeDamageFromFireBall > deltaTime)
            {
                skill = GameDynamicData.mainCharacter.skillHandle.GetSkill(SkillName.LASER_S);
                lastTimeTakeDamageFromFireBall = Time.time;
                multiplyDamage = 0.4f;
            }

        }

        if (skill != null)
        {
            skill.InflictDamage(this, multiplyDamage, skill.GetSkillName());
            AfterTakeDamage();
        }
    }
    #endregion


    protected float lastTimeDealDamage = Mathf.NegativeInfinity;
    private float lastMoveSpeed;

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(StringConst.TAG_PLAYER))
        {
            if (Time.time - lastTimeDealDamage > deltaTimeNormalAttack)
            {
                lastTimeDealDamage = Time.time;
                var eAtk = SpawnerMananger.Instance.GetAttack(id);
                GameDynamicData.mainCharacter.TakeDamage(this, eAtk,0);
            }
        }
    }
    
}
