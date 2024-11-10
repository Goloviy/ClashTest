using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshPro))]
public class FloatingDamage : MonoBehaviour
{
    TextMeshPro tmp;
    static string POOL_NAME = "Props";
    float startTime = 0f;
    [SerializeField] float lifeTime = 0.3f;
    float delayTime = 0.15f;
    bool isEnable = false;
    Vector3 originScale;
    Color originColor;
    [SerializeField] Vector3 scaleMax = new Vector3(0.15f, 0.1f, 0.15f);
    float speedMoveUp = 0.5f;
    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
        originColor = tmp.color;
        originScale = this.transform.localScale;
    }
    private void OnSpawned(SpawnPool pool)
    {

        if (GameDynamicData.dictStringDamage.ContainsKey(GameDynamicData.curUnitInflictDamage))
        {
            isEnable = true;
            this.transform.localScale = originScale;
            tmp.color = originColor;
            Init2(GameDynamicData.dictStringDamage[GameDynamicData.curUnitInflictDamage], this.transform.position);
            this.transform.DOPunchScale(scaleMax, lifeTime, 1, 1);
            tmp.DOFade(0.3f, lifeTime - delayTime).SetDelay(delayTime);
        }
        else
        {
            DebugCustom.Log("Not available damage in DictStringDamage");
            DespawnThis();
        }
    }

    //private void OnScaleFinish()
    //{
    //    this.transform.DOScale(0.73f, lifeTime / 2);
    //}

    private void OnDespawned(SpawnPool pool)
    {
        isEnable = false;
    }
    //public void Init(AttackDataProcessed attackDataP, Vector3 startPos)
    //{
    //    tmp.text = attackDataP.Damage.ToString();
    //    var rdPosX = Random.Range(-0.15f, 0.15f);
    //    var rdPosY = Random.Range(-0.1f, 0.1f);
    //    startPos = new Vector3(startPos.x + rdPosX, startPos.y + rdPosY);
    //    this.transform.position = startPos;
    //    startTime = Time.time;
    //}
    /// <summary>
    /// optimize gc
    /// </summary>
    public void Init2(string damage, Vector3 startPos)
    {
        tmp.text = damage;
        var rdPosX = Random.Range(-0.15f, 0.15f);
        var rdPosY = Random.Range(-0.1f, 0.1f);
        startPos = new Vector3(startPos.x + rdPosX, startPos.y + rdPosY);
        this.transform.position = startPos;
        startTime = Time.time;
    }
    private void Update()
    {
        if (!isEnable)
        {
            return;
        }
        if (Time.time - startTime > lifeTime)
        {
            DespawnThis();
        }
        this.transform.MoveTransformWithoutPhysic(Vector3.up, speedMoveUp);
    }
    private void DespawnThis()
    {
        PoolManager.Pools[POOL_NAME].Despawn(this.transform);
    }

}
