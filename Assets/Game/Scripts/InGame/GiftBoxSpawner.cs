using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBoxSpawner : MonoBehaviour
{
    public MainCharacter player;
    [SerializeField] GameObject prefabBox;
    [SerializeField] private float MinDistance = 20f;
    [SerializeField] private float MaxDistance = 5f;
    [SerializeField] private int Count = 1;
    [SerializeField] private float TimeInterval = 20f;
    [SerializeField] private float TimeRefresh = 60;
    private void Update()
    {

        if (player == null)
        {
            player = FindObjectOfType<MainCharacter>();
            return;
        }
        if (InGameManager.Instance.TotalTimePlay - TimeRefresh > TimeInterval)
        {
            TimeRefresh = Time.time;
            SpawnBox();
        }
    }
    private void SpawnBox()
    {
        for (int i = 0; i < Count; i++)
        {
            Vector3 dir = Random.insideUnitCircle;
            float length = MinDistance + MaxDistance * Random.value;
            Vector3 pos = dir.normalized * length + player.transform.position;
            PoolManager.Pools[StringConst.POOL_ITEM_NAME].Spawn(prefabBox, pos, Quaternion.identity);

        }
    }
}
