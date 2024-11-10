using DG.Tweening;
using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BottleGasSkill : SkillBase
{
    //public List<GameObject> prefabsBottleByLevel;
    //public GameObject CurPrefabBottle => prefabsBottleByLevel[level - 1];
    public GameObject prefabBottle;
    public GameObject prefabBurning;
    public List<float> ActiveTimeByLevel;
    public float CurActiveTime => ActiveTimeByLevel[level - 1];
    float refreshTime = 0;
    List<Transform> listTf;
    [SerializeField] List<GameObject> listGroup;
    float bottleMoveTime = 0.8f;
    bool isActiveFire;
    List<Transform> tfBurnings;
    public override void LevelUpChange()
    {
        refreshTime = Time.time;
        base.LevelUpChange();
        for (int i = 0; i < listGroup.Count; i++)
        {
            if (i == level - 1)
            {
                bottleMoveTime = CharacterStatusHelper.CalculateBulletMoveTime(bottleMoveTime, this);
                var curGroup = listGroup[i];
                curGroup.SetActive(true);
                listTf = curGroup.GetComponentsInChildren<Transform>().ToList();
                listTf.RemoveAt(0);
            }
            else
            {
                listGroup[i].SetActive(false);
            }
        }

    }
    
    
    private void Update()
    {
        if (!owner.IsAlive)
            return;
        if (level <= 0)
            return;
        if (!isActiveFire)
        {
            if (Time.time - CurTimeInterval > refreshTime)
            {
                LaucherBottle();
                refreshTime = Mathf.Infinity;
                isActiveFire = true;
            }
        }
        else
        {
            if (Time.time - CurActiveTime > refreshTime)
            {
                RemoveFire();
                refreshTime = Time.time;
                isActiveFire = false;
            }
        }
    }
    private async void RemoveFire()
    {
        foreach (var item in tfBurnings)
        {
            PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(item);
            await Task.Delay(100);
        }
    }
    private async void LaucherBottle()
    {
        tfBurnings = new List<Transform>();
        for (int i = 0; i < listTf.Count; i++)
        {
            var targetPos = listTf[i];
            var tf = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(prefabBottle, this.transform.position, Quaternion.identity);
            var originScale = tf.localScale;
            tf.localScale = originScale * CharacterStatusHelper.CalculateBulletScale(this);
            var posFire = targetPos.position;
            tf.DOMove(posFire, bottleMoveTime).SetEase(Ease.OutCubic).onComplete += OnBottleCrack;
            tf.DOPunchScale( Vector3.one * 1.3f, bottleMoveTime, 1).SetEase(Ease.OutCubic);
            void OnBottleCrack()
            {
                tf.localScale = originScale;
                PoolManager.Pools[StringConst.POOL_BULLET_NAME].Despawn(tf);
                var particle = PoolManager.Pools[StringConst.POOL_BULLET_NAME].Spawn(prefabBurning, posFire, Quaternion.identity);
                ParticleTrigger particleTrigger = particle.GetComponent<ParticleTrigger>();
                particleTrigger.Init(this);
                tfBurnings.Add(particle);
            }
            await Task.Delay(100);
            refreshTime = Time.time;
        }
    }


}
