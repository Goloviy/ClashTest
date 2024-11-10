using PathologicalGames;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public interface ICanInvisiable
{
    void SetVisiable(bool isVisiable);
}
public interface ITurnFaceable
{
    public bool IsFaceLeft { get; set; }
    public void TurnFaceToEnemy();
    public void TurnFaceToPoint(Vector3 point);
}
public class StreamBehavior
{
    public string streamName;
    public EnemyBehavior[] behaviors;
}
public class StreamBehaviorHandle
{
    int progress;
    public StreamBehavior stream;
    
}
public class BossCharacter : EnemyBase, ITurnFaceable, ICanInvisiable
{
    [SerializeField] GameObject goBlood;
    [SerializeField] private float collisionDamageMultiply = 0.5f;
    [SerializeField] float distanceFloatingDamage = 2f;
    protected SkeletonAnimation skeAnim;
    [SpineAnimation(dataField: "skeAnim")]
    public string animationNameDeath = "";


    protected StreamBehavior curStreamBehavior;
    protected StreamBehavior[] streams;
    protected MeshRenderer renderer;
    //protected MainCharacter mainCharacter;
    public bool IsFaceLeft { get; set; } = true;
    public EnemyBehavior idleBehavior;
    protected EnemyBehavior currBehavior;
    protected EnemyBehavior[] behaviors;
    [SerializeField] protected Transform tfModel;
    protected DropBox[] dropBoxs;
    Vector3[] offsetDrops = new Vector3[5]
{
        Vector3.zero,
        new Vector3(0.5f, -0.5f),
        new Vector3(0.5f, 0.5f),
        new Vector3(-0.5f, 0.5f),
        new Vector3(-0.5f, -0.5f)
};

    //tint data 
    protected const string STR_TINT_COLOR_NAME = "_Black";
    protected Color ColorTint = new Color(1, 1, 1, 1);
    protected Color ColorNormal = new Color(0, 0, 0, 1);
    protected override void Awake()
    {
        base.Awake();
        skeAnim = GetComponentInChildren<SkeletonAnimation>();
        renderer = skeAnim.gameObject.GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        skeAnim.AnimationState.Event += OnEvent;
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_TAKE_BOMB, OnPlayerTakeBomb);
    }
    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_TAKE_BOMB, OnPlayerTakeBomb);
        skeAnim.AnimationState.Event -= OnEvent;
    }


    protected override void OnSpawned(SpawnPool pool)
    {
        //base.OnSpawned(pool);
        collider.enabled = true;
        rigid.simulated = true;
        InitBehavior();
        ActionIdleBehavior();
        SetDropBoxes(SpawnerMananger.Instance.GetDropBox(id));

    }

    protected override void OnDespawned(SpawnPool pool)
    {
        //base.OnDespawned(pool);
        collider.enabled = false;
        rigid.simulated = false;
    }

    protected void OnPlayerTakeBomb(Component arg1, object arg2)
    {
        TakeDamage(GameDynamicData.mainCharacter, GameDynamicData.mainCharacter.unitStatus.Attack * GameConfigData.Instance.BombMultiDamage, 0);
        //TakeDamage(new AttackData(GameDynamicData.mainCharacter, GameDynamicData.mainCharacter.unitStatus.Attack * GameConfigData.Instance.BombMultiDamage, 0));
    }
    protected override void AfterTakeDamage()
    {
        EventDispatcher.Instance.PostEvent(EventID.BOSS_TAKE_DAMAGE, (float)HpRemainning/HpMax);
        //if (!ReferenceEquals( prefabBlood, null) || prefabBlood != null)
        //{
        //PoolManager.Pools[StringConst.POOL_FX_NAME].Spawn(prefabBlood, this.transform.position, Quaternion.identity, this.transform);
        //}
        //if (!ReferenceEquals(goBlood,null) || goBlood != null)
        //{
        //    goBlood.SetActive(true);
        //    this.StartDelayAction(0.15f, () => { goBlood.SetActive(false); });
        //}
        TintColor();
    }

    protected virtual async void TintColor()
    {
        if (renderer)
        {
            renderer.material.SetColor(STR_TINT_COLOR_NAME, ColorTint);
            await Task.Delay(100);
            renderer.material.SetColor(STR_TINT_COLOR_NAME, ColorNormal);
            //mainMaterial.SetColor()
        }
    }


    public void TurnFaceToEnemy()
    {
        bool isTurnLeft = GameDynamicData.mainCharacter.transform.position.x < this.transform.position.x;
        if (isTurnLeft != IsFaceLeft)
        {
            IsFaceLeft = isTurnLeft;
            tfModel.localScale = new Vector3(tfModel.localScale.x * -1, tfModel.localScale.y, tfModel.localScale.z);
        }
    }
    public void TurnFaceToPoint(Vector3 point)
    {
        bool isTurnLeft = point.x < this.transform.position.x;
        if (isTurnLeft != IsFaceLeft)
        {
            IsFaceLeft = isTurnLeft;
            tfModel.localScale = new Vector3(tfModel.localScale.x * -1, tfModel.localScale.y, tfModel.localScale.z);
        }
    }
    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (currBehavior == null)
        {
            return;
        }
        if (e.Data.Name.Equals("fire"))
        {
            currBehavior.ShortAction();
        }
        else if (e.Data.Name.Equals("start"))
        {
            currBehavior.StartLongAction();
        }
        else if (e.Data.Name.Equals("end"))
        {
            currBehavior.EndLongAction();
        }
        else if (e.Data.Name.Equals("mark"))
        {
            currBehavior.MarkTargetAction();
        }
        else if (e.Data.Name.Equals("unmark"))
        {
            currBehavior.UnmarkTargetAction();
        }
    }

    protected virtual void InitBehavior()
    {
        behaviors = transform.GetComponents<EnemyBehavior>();
        foreach (var behavior in behaviors)
        {
            behavior.Init(this,this, id);
        }
    }
    protected virtual void ChooseBehaviorRandom()
    {
        if (!IsAlive)
        {
            return;
        }
        int rd = UnityEngine.Random.Range(0, behaviors.Length);
        currBehavior = behaviors[rd];
        currBehavior.Select(ActionCallBack);
    }
    protected virtual void ActionIdleBehavior()
    {
        currBehavior = idleBehavior;
        currBehavior.Select(ActionCallBack);
    }
    protected void ActionCallBack(EnemyBehaviorState state)
    {
        switch (state)
        {
            case EnemyBehaviorState.NONE:
                break;
            case EnemyBehaviorState.START_LONG_ACTION:
                break;
            case EnemyBehaviorState.SHORT_ACTION:
                break;
            case EnemyBehaviorState.END_LONG_ACTION:
                break;
            case EnemyBehaviorState.FINISH:
                FinishBehavior();
                break;
            default:
                break;
        }
    }
    protected void FinishBehavior()
    {
        if (currBehavior == idleBehavior)
        {
            currBehavior = null;
            ChooseBehaviorRandom();
        }
        else
        {
            currBehavior = null;
            ActionIdleBehavior();
        }
    }
    public override void Die()
    {
        currBehavior.FinishBehavior();
        skeAnim.state.SetAnimation(0, animationNameDeath, false);
        EventDispatcher.Instance.PostEvent(EventID.BOSS_DIE);
        Drop();
        DespawnThis();
        
    }
    public override void DespawnThis()
    {
        if (PoolManager.Pools[StringConst.POOL_OPPONENT_NAME].IsSpawned(this.transform))
        {
            PoolManager.Pools[StringConst.POOL_OPPONENT_NAME].Despawn(this.transform, 2f);
        }
    }
    public override void ShowDamage(int Damage, bool IsCritical)
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
        PoolManager.Pools[StringConst.POOL_PROPS_NAME].Spawn(prefab, this.transform.position + Vector3.up * distanceFloatingDamage, prefabFloatingDamage.rotation);
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
        int rdDrop = UnityEngine.Random.Range(0, 100);
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
    protected virtual void SetDropBoxes(DropBox[] dropBox)
    {
        this.dropBoxs = dropBox;
    }

    public void SetVisiable(bool isVisiable)
    {
        collider.enabled = isVisiable;
        rigid.simulated = isVisiable;
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(StringConst.TAG_PLAYER))
        {
            if (Time.time - lastTimeDealDamage > deltaTimeNormalAttack)
            {
                lastTimeDealDamage = Time.time;
                var eAtk = SpawnerMananger.Instance.GetAttack(id);
                GameDynamicData.mainCharacter.TakeDamage(this, Mathf.RoundToInt( eAtk * collisionDamageMultiply) , 0);
            }
        }
    }
}
