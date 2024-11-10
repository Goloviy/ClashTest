using DG.Tweening;
using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]

public class CollectorBase : MonoBehaviour
{
    CircleCollider2D circleCollider;
    MainCharacter character;
    float originRadius;
    const string MASK_COLLECT_S = "CollectorS";
    const string MASKE_COLLECT = "Collector";
    List<Transform> collectingItems;
    bool isMagnetCollecting = false;
    //float deltaTimeUpdateDirect = 0.2f;
    //float lastTimeRefrest = 0f;
    float speedMoveX = 0.22f;
    float speedMoveY = 0.22f;
    float speedMoveX_N = 0.165f;
    float speedMoveY_N = 0.165f;
    int countFinish = 0;
    Vector3 UpPos = new Vector3(0, 1.5f);
    Vector3 DownPos = new Vector3(0, -1.5f);
    //List<Transform> tfNormalCollectFinish;
    //int normalCollectTotal = 0;
    List<Transform> tfNormalCollectFinish2;
    List<Transform> tfRemove;

    //bool isEnableItemMagnet = false;
    private void Awake()
    {
        tfNormalCollectFinish2 = new List<Transform>();
        tfRemove = new List<Transform>();
        collectingItems = new List<Transform>();
        circleCollider = GetComponent<CircleCollider2D>();
        originRadius = circleCollider.radius;
        gameObject.layer = LayerMask.GetMask(MASK_COLLECT_S);
    }
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.SKILL_LEVEL_UP_AFTER, AfterSkillLevelUp);

    }
    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.SKILL_LEVEL_UP_AFTER, AfterSkillLevelUp);
    }

    private void AfterSkillLevelUp(Component arg1, object arg2)
    {
        circleCollider.radius = originRadius * character.skillHandle.stats.RangeCollectorRatio;
    }

    public void Init(MainCharacter character)
    {
        this.character = character;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isMagnetCollecting)
        {
            return;
        }
        else
        {
            bool isExpItem = collision.gameObject.CompareTag(StringConst.TAG_EXP)
                || collision.gameObject.CompareTag(StringConst.TAG_EXP2)
                || collision.gameObject.CompareTag(StringConst.TAG_EXP3);
            bool isOtherItem = collision.gameObject.CompareTag(StringConst.TAG_BOMB)
                || collision.gameObject.CompareTag(StringConst.TAG_MAGNET)
                || collision.gameObject.CompareTag(StringConst.TAG_BOX_GACHA)
                || collision.gameObject.CompareTag(StringConst.TAG_BOX_GACHA_RV)
                || collision.gameObject.CompareTag(StringConst.TAG_MEAT)
                || collision.gameObject.CompareTag(StringConst.TAG_GOLD)
                || collision.gameObject.CompareTag(StringConst.TAG_GOLD_2)
                || collision.gameObject.CompareTag(StringConst.TAG_GOLD_3);
            if (isOtherItem || isExpItem)
            {
                var posBack = collision.transform.position +
                    (this.transform.position.y - collision.transform.position.y < 0 ? UpPos : DownPos);
                //float distance = Vector3.Distance(this.transform.position, posBack);
                //float time = 0.5f;
                collision.transform.DOMove(posBack, 0.4f).SetEase(Ease.OutQuad).onComplete += () =>
                {
                                //PoolManager.Pools[StringConst.POOL_ITEM_NAME].Despawn(collision.transform);
                                //CollectHandle(collision.gameObject.tag);
                                //normalCollectTotal++;
                                //tfNormalCollectFinish.Add(collision.transform);
                                tfNormalCollectFinish2.Add(collision.transform);
                };
            }
            //else if (isExpItem)
            //{
            //    float distance = Vector3.Distance(this.transform.position, collision.transform.position);
            //    float time = distance / 2f;
            //    collision.transform.DOMove(this.transform.position, time).SetEase(Ease.InBack).onComplete += () =>
            //    {
            //        PoolManager.Pools[StringConst.POOL_EXP_NAME].Despawn(collision.transform);
            //        CollectHandle(collision.gameObject.tag);
            //    };
            //}
        }

    }
    private bool IsExpItem(Transform tf)
    {
        return tf.CompareTag(StringConst.TAG_EXP)
            || tf.CompareTag(StringConst.TAG_EXP2)
            || tf.CompareTag(StringConst.TAG_EXP3);
    } 
    private void CollectHandle(string tag)
    {
        if (tag.Equals(StringConst.TAG_EXP))
        {
            character.CollectItem(DropItem.EXP, 1);

        }
        else if (tag.Equals(StringConst.TAG_EXP2))
        {
            character.CollectItem(DropItem.EXP, 5);

        }
        else if (tag.Equals(StringConst.TAG_EXP3))
        {
            character.CollectItem(DropItem.EXP, 25);

        }
        else if (tag.Equals(StringConst.TAG_BOMB))
        {
            character.CollectItem(DropItem.BOMB);
            EventDispatcher.Instance.PostEvent(EventID.CHARACTER_TAKE_BOMB);
        }
        else if (tag.Equals(StringConst.TAG_MAGNET))
        {
            MagnetExp();
        }
        else if (tag.Equals(StringConst.TAG_MEAT))
        {
            character.CollectItem(DropItem.MEAT);

        }
        else if (tag.Equals(StringConst.TAG_BOX_GACHA))
        {
            EventDispatcher.Instance.PostEvent(EventID.CHARACTER_TAKE_GACHA);
        }
        else if (tag.Equals(StringConst.TAG_BOX_GACHA_RV))
        {
            EventDispatcher.Instance.PostEvent(EventID.CHARACTER_TAKE_GACHA);
        }
        else if (tag.Equals(StringConst.TAG_GOLD))
        {
            character.CollectItem(DropItem.GOLD, GameConfigData.Instance.PackageGold);
        }
        else if (tag.Equals(StringConst.TAG_GOLD_2))
        {
            character.CollectItem(DropItem.GOLD, GameConfigData.Instance.PackageGold2);
        }
        else if (tag.Equals(StringConst.TAG_GOLD_3))
        {
            character.CollectItem(DropItem.GOLD, GameConfigData.Instance.PackageGold3);
        }
        else
        {
            DebugCustom.LogError(tag + " : Tag is not define in CollectorBase");
        }
    }
    private void MagnetExp()
    {
        if (!isMagnetCollecting)
        {
            EventDispatcher.Instance.PostEvent(EventID.CHARACTER_TAKE_MAGNET);
            character.CollectItem(DropItem.MAGNET);
            collectingItems = new List<Transform>(PoolManager.Pools[StringConst.POOL_EXP_NAME]);
            countFinish = collectingItems.Count;
            isMagnetCollecting = true;
        }
    }

    private void FixedUpdate()
    {
        //if (Time.timeScale == 0)
        //{
        //    return;
        //}
        if (isMagnetCollecting)
        {
            Vector3 targetPos = this.transform.position;
            foreach (var itemExp in collectingItems)
            {
                if (!PoolManager.Pools[StringConst.POOL_EXP_NAME].IsSpawned(itemExp))
                {
                    continue;
                }
                else
                {
                    MoveToTarget(targetPos, itemExp, () =>
                    {
                        PoolManager.Pools[StringConst.POOL_EXP_NAME].Despawn(itemExp);
                        CollectHandle(itemExp.gameObject.tag);
                        countFinish--;
                    }, true);
                }
            }
            if (countFinish <= 3)
            {
                isMagnetCollecting = false;
                EventDispatcher.Instance.PostEvent(EventID.MAGNET_END_EFFECT);
            }
        }
        else
        {
            //normal collect item Updater
            if (tfNormalCollectFinish2.Count > 0)
            {
                //if (normalCollectTotal > 0)
                //{
                tfRemove.Clear();
                Vector3 targetPos = this.transform.position;
                foreach (var tf in tfNormalCollectFinish2)
                {
                    var pool = PoolManager.Pools[IsExpItem(tf) ?
                        StringConst.POOL_EXP_NAME :
                        StringConst.POOL_ITEM_NAME];
                    if (pool.IsSpawned(tf))
                    {
                        MoveToTarget(targetPos, tf, () =>
                        {
                            tfRemove.Add(tf);
                            CollectHandle(tf.tag);
                            pool.Despawn(tf);
                            //normalCollectTotal--;
                        });
                    }
                }

                if (tfRemove.Count == tfNormalCollectFinish2.Count)
                {
                    tfNormalCollectFinish2.Clear();
                    tfRemove.Clear();
                }
                else
                {
                    foreach (var tf in tfRemove)
                    {
                        //if (tfNormalCollectFinish2.Contains(tf))
                        //{
                        tfNormalCollectFinish2.Remove(tf);
                        //}
                    }
                }


            }
        }
    }

    private void MoveToTarget(Vector3 targetPos, Transform itemExp, Action OnFinish = null, bool isUpSpeed = false)
    {
        var curSpeedX = isUpSpeed ? speedMoveX : speedMoveX_N;
        var curSpeedY = isUpSpeed ? speedMoveY : speedMoveY_N;
        float deltaX = itemExp.position.x - targetPos.x;
        float deltaY = itemExp.position.y - targetPos.y;

        float scaleYX = deltaY / deltaX;
        float scaleXY = deltaX / deltaY;

        if (deltaX > 0.35f || deltaX < -0.35f)
        {
            deltaX = deltaX > 0 ? -curSpeedX : curSpeedX;
            if (scaleXY < 1f && scaleXY > 0)
                deltaX *= scaleXY;
            else if (scaleXY < 0f && scaleXY > -1f)
            {
                deltaX *= scaleXY * -1f;
            }
        }
        else
            deltaX = 0f;

        if (deltaY > 0.35f || deltaY < -0.35f)
        {
            deltaY = deltaY > 0 ? -curSpeedY : curSpeedY;
            if (scaleYX < 1f && scaleYX > 0)
                deltaY *= scaleYX;
            else if (scaleYX < 0f && scaleYX > -1f)
            {
                deltaY *= scaleYX * -1f;
            }
        }
        else
            deltaY = 0f;
        if (deltaY == 0f && deltaX == 0f)
        {
            //PoolManager.Pools[StringConst.POOL_EXP_NAME].Despawn(itemExp);
            //CollectHandle(itemExp.gameObject.tag);
            //countFinish--;
            OnFinish?.Invoke();
        }
        else
        {
            Vector3 newPos = new Vector3(itemExp.position.x + deltaX,
                itemExp.position.y + deltaY,
                targetPos.z);
            itemExp.position = newPos;
        }
    }
}
