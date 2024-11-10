using DG.Tweening;
using PathologicalGames;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MonsterBase : EnemyBase
{
    [SerializeField] bool isMiniBoss = false;
    protected GameObject target;
    protected Vector3 moveDirect;

    //update direct interval
    protected float refreshTime = Mathf.NegativeInfinity;
    protected float intervalTime = 0.1f;

    protected DropBox[] dropBoxs;
    //public int level = 1;
    //protected SpriteRenderer spriteRenderer;
    protected MaterialPropertyBlock _propBlock;
    //[SerializeField]
    protected ParticleSystemRenderer particleRenderer;
    protected ParticleSystem.MainModule particleMain;
    protected ParticleSystem particle;
    [Title("Setting Take Damage")]
    [SerializeField] bool isSqueezeSlight = false;
    //[SerializeField] protected Color ColorTint = new Color(1, 1, 1, 0.1f);
    //[SerializeField] protected Color ColorNormal = new Color(0,0,0,1);
    protected Color ColorTint = new Color(1, 1, 1, 1);
    protected Color ColorNormal = new Color(0,0,0,1);
    [SerializeField] protected Transform prefabFxDeath;

    protected Vector3 originScale;
    protected bool isSqueezing = false;
    static Vector3 SqueezeNew = new Vector3(0f, -0.5f, 0f);
    protected Vector3 squeeze;
    protected Vector3 squeezeSlight;
    [Title("Setting Move")]
    [SerializeField] protected bool isMoveSpeedVolatility;
    [ShowIf("isMoveSpeedVolatility", true)]
    [SerializeField] protected float multiplySpeedUp = 3f;
    [ShowIf("isMoveSpeedVolatility", true)]
    [SerializeField] protected float timeUpSpeed = 1f;
    [ShowIf("isMoveSpeedVolatility", true)]
    [SerializeField] protected float timeNormalSpeed = 2f;
    protected float lastTimeChangeMoveSpeed = 0f;
    protected float lastTimeCheckCollider = 0f;
    protected bool stateMoveUp = false;

    Vector3[] offsetDrops = new Vector3[5]
    {
        Vector3.zero,
        new Vector3(0.5f, -0.5f),
        new Vector3(0.5f, 0.5f),
        new Vector3(-0.5f, 0.5f),
        new Vector3(-0.5f, -0.5f)
    };
    protected override void Awake()
    {
        base.Awake();
        _propBlock = new MaterialPropertyBlock();
        particleRenderer = this.transform.GetComponentInChildren<ParticleSystemRenderer>();
        particle = this.transform.GetComponentInChildren<ParticleSystem>();
        particleMain = particle.main;
        //spriteRenderer = this.transform.GetComponentInChildren<SpriteRenderer>();
        originScale = this.transform.localScale;
    }
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.CHANGE_WAVE_MONSTER, OnChangeWave);
    }

    private void OnChangeWave(Component arg1, object arg2)
    {
        if (!CameraFollower.Instance.IsInsideCam(this.transform.position))
        {
            PoolManager.Pools[StringConst.POOL_OPPONENT_NAME].Despawn(this.transform);
        }
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.CHANGE_WAVE_MONSTER, OnChangeWave);

    }

    protected override void OnSpawned(SpawnPool pool)
    {
        base.OnSpawned(pool);
        EnableCollider();
        //spriteRenderer.transform.localScale = originScale;
        isSqueezing = false;
        squeeze = new Vector3(0f, -originScale.y * 0.45f, 0f);
        squeezeSlight = new Vector3(0f, -originScale.y * 0.35f, 0f);

        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_TAKE_BOMB, OnBombActive);
    }

    protected void OnBombActive(Component arg1, object arg2)
    {
        if (isMiniBoss)
            TakeDamage(GameDynamicData.mainCharacter, GameDynamicData.mainCharacter.unitStatus.Attack * GameConfigData.Instance.BombMultiDamage, 0);
            //TakeDamage(new AttackData(GameDynamicData.mainCharacter, GameDynamicData.mainCharacter.unitStatus.Attack * GameConfigData.Instance.BombMultiDamage, 0));
        else
            Die();

    }

    protected override void OnDespawned(SpawnPool pool)
    {
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_TAKE_BOMB, OnBombActive);

        base.OnDespawned(pool);
    }
    public override void Init()
    {
        //level = GameDynamicData.CurLevelMonster;
        base.Init();
        SetTarget(SpawnerMananger.Instance.mainCharacter);
        SetDropBoxes(SpawnerMananger.Instance.GetDropBox(id));
    }
    protected override void OnCreate()
    {
        HpMax = GameDynamicData.hpMonster;
        HpRemainning = HpMax;
        moveSpeed = unitStatus.MoveSpeed * GameDynamicData.mSpeedMonster;
        originMoveSpeed = moveSpeed;

    }
    protected virtual void SetTarget(GameObject target)
    {
        this.target = target;
    }
    protected virtual void SetDropBoxes(DropBox[] dropBox)
    {
        this.dropBoxs = dropBox;
    }
    protected virtual void UpdateDirect()
    {
        moveDirect = (target.transform.position - this.transform.position).normalized;
        //var curFlip = spriteRenderer.flipX;
        //var flip = target.transform.position.x > this.transform.position.x;
        //if (curFlip != flip)
        //{
        //    spriteRenderer.flipX = flip;
        //}
        if (particleRenderer)
        {
            var curFlip = particleRenderer.flip;
            var flipX = target.transform.position.x > this.transform.position.x ? 1 : 0;
            if (curFlip.x != flipX)
            {
                curFlip.x = flipX;
                particleRenderer.flip = curFlip;
            }
        }

    }

    protected override void Move()
    {
        this.transform.MoveTransformWithoutPhysic(moveDirect, moveSpeed);
    }
    protected virtual void Update()
    {
        if (target??false)
        {
            TimerUpdateDirect();
            TimerUpdateMoveSpeed();
        }
        EnableCollider();
    }
    protected virtual void TimerUpdateDirect()
    {
        if (Time.time - intervalTime > refreshTime)
        {
            refreshTime = Time.time;
            UpdateDirect();
        }
    }
    protected virtual void TimerUpdateMoveSpeed()
    {
        if (isMoveSpeedVolatility)
        {
            var cdTime = stateMoveUp ? timeUpSpeed : timeNormalSpeed;
            if (Time.time - cdTime > lastTimeChangeMoveSpeed)
            {
                lastTimeChangeMoveSpeed = Time.time;
                stateMoveUp = !stateMoveUp;
                SpeedUp(stateMoveUp, multiplySpeedUp);
            }
        }

        void SpeedUp(bool status, float multiply = 3f)
        {
            if (status)
            {
                moveSpeed = moveSpeed * multiply;
            }
            else
                moveSpeed = originMoveSpeed;
        }
    }
    protected virtual void EnableCollider()
    {
        if (collider.enabled)
            return;
        else
        {
            if (CameraFollower.Instance.IsInsideCam(this.transform.position) &&
            collider.enabled == false &&
            IsAlive)
            {
                collider.enabled = true;
                rigid.simulated = true;
            }
        }

    }
    protected virtual void FixedUpdate()
    {
        Move();
    }

    public override async void Die()
    {

        await Task.Delay(220);
        Drop();
        DespawnThis();
        GameDynamicData.KillCount++;
        EventDispatcher.Instance.PostEvent(EventID.MONSTER_DIE);
        PoolManager.Pools[StringConst.POOL_FX_NAME].Spawn(prefabFxDeath,this.transform.position, prefabFxDeath.rotation);
    }
    protected virtual void Drop()
    {
        int i = 0;
        foreach (var box in dropBoxs)
        {
            
            DropItem(box, i++);
        }
    }
    protected virtual void DropItem(DropBox dropBox, int index = 0)
    {
        int rdDrop = Random.Range(0, 100);
        if (rdDrop >= dropBox.percent)
        {
            //not drop this box
            return;
        }
        int indexOffset = index % offsetDrops.Length;
        if (CheckExpItem(dropBox.prefabItem))
        {
            PoolManager.Pools[StringConst.POOL_EXP_NAME].Spawn(
                dropBox.prefabItem
                , this.transform.position + offsetDrops[indexOffset]
                , dropBox.prefabItem.transform.rotation);
        }
        else
        {
            PoolManager.Pools[StringConst.POOL_ITEM_NAME].Spawn(
            dropBox.prefabItem
            , this.transform.position + offsetDrops[indexOffset]
            , dropBox.prefabItem.transform.rotation);
        }
    }
    protected bool CheckExpItem(GameObject go)
    {
        foreach (var _tag in GameStaticData.ITEM_EXP_TYPE)
        {
            if (go.CompareTag(_tag))
            {
                return true;
            }
        }
        return false;
    }
    protected override void AfterTakeDamage()
    {
        ChangeColor();
        Squeeze();
    }


    protected void Squeeze()
    {
        if (!isSqueezing)
        {
            isSqueezing = true;
            this.transform.DOPunchScale(isSqueezeSlight ? squeezeSlight : squeeze, 0.2f, 1).SetEase(Ease.Linear).onComplete += () =>
           {
               isSqueezing = false;
           };
            // this.transform.DOPunchScale(SqueezeNew, 0.25f, 1).SetEase(Ease.Linear).onComplete += () =>
            //{
            //    isSqueezing = false;
            //};
        }
    }
    const string STR_TINT_COLOR_NAME = "_Color6";
    protected async void ChangeColor()
    {
        //blend v2

        //particleMain.startColor = Color.red;
        //particle.Stop();
        //particle.Clear();
        //particle.Simulate(particleMain.duration);
        //particle.Play();
        //await Task.Delay(200);
        //particleMain.startColor = Color.white;
        //particle.Stop();
        //particle.Clear();
        //particle.Simulate(particleMain.duration);
        //particle.Play();

        //tint v1

        //spriteRenderer.GetPropertyBlock(_propBlock);
        //_propBlock.SetColor("_Color5", ColorTint);
        //spriteRenderer.SetPropertyBlock(_propBlock);
        //await Task.Delay(200);
        //_propBlock.SetColor("_Color5", ColorNormal);
        //spriteRenderer.SetPropertyBlock(_propBlock);

        //new tint v2
        particleRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor(STR_TINT_COLOR_NAME, ColorTint);
        particleRenderer.SetPropertyBlock(_propBlock);
        await Task.Delay(100);
        _propBlock.SetColor(STR_TINT_COLOR_NAME, ColorNormal);
        particleRenderer.SetPropertyBlock(_propBlock);

    }
}
