using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshPro))]

public class HealingDamage : MonoBehaviour
{
    TextMeshPro tmp;
    static string POOL_NAME = "Props";

    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }
    private void OnSpawned()
    {
        Init(GameDynamicData.healAmount, this.transform.position);
    }
    public void Init(int healAmount, Vector3 startPos)
    {
        tmp.text = healAmount.ToString();
        this.transform.position = startPos;
        this.transform.DOMove(startPos + Vector3.up * 0.5f, 0.4f).onComplete += () =>
        {
            DespawnThis();
        };
    }
    private void DespawnThis()
    {
        PoolManager.Pools[POOL_NAME].Despawn(this.transform);
    }
}
