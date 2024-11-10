using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralMap : MonoBehaviour
{
    [SerializeField] float rectRange = 10.2f;
    [SerializeField] Transform[] prefabsCampaign;
    Transform curPrefab;
    float timeRefresh = 0f;
    float timeInterval = 0.25f;
    int count = 9;
    Transform tfMainCharacter;
    Vector3[] deltaDistance = new Vector3[9]
    {
        new Vector3(-10.2f,10.2f),
        new Vector3(0,10.2f),
        new Vector3(10.2f,10.2f),
        new Vector3(-10.2f,0),
        new Vector3(0.01f,0f),
        new Vector3(10.2f,0),
        new Vector3(-10.2f,-10.2f),
        new Vector3(0,-10.2f),
        new Vector3(10.2f,-10.2f)
    };
    void Start()
    {
        try
        {
            if (GameDynamicData.CurGameMode == GameMode.CAMPAIGN)
            {
                curPrefab = prefabsCampaign[GameDynamicData.SelectChapterLevel - 1];
            }
        }
        catch 
        {
            if (!curPrefab)
            {
                curPrefab = prefabsCampaign[0];
            }
        }
        CreateTile(Vector3.zero);
    }
    void CreateTile(Vector3 center)
    {
        for (int i = 0; i < count; i++)
        {
            var pos = center + deltaDistance[i];
            PoolManager.Pools[StringConst.POOL_ENVIRONMENT_NAME].Spawn(curPrefab.transform,
                pos, Quaternion.identity, this.transform);
        }
    }
    void Update()
    {
        if (GameDynamicData.mainCharacter == null)
        {
            return;
        }
        else
        {
            tfMainCharacter = GameDynamicData.mainCharacter.transform;
        }
        if (Time.time - timeRefresh > timeInterval)
        {
            timeRefresh = Time.time;
            UpdateMapTile();
        }
    }

    private void UpdateMapTile()
    {
        SpawnPool pool = PoolManager.Pools[StringConst.POOL_ENVIRONMENT_NAME];
        var tileds = new List<Transform>(pool);
        var centerTiledId = FindTiledCenter();
        var posCenter = tileds[centerTiledId].position;
        PoolManager.Pools[StringConst.POOL_ENVIRONMENT_NAME].DespawnAll();
        CreateTile(posCenter);       

        int FindTiledCenter()
        {
            float minDistance = Mathf.Infinity;
            int index = -1;
            for (int i = 0; i < tileds.Count; i++)
            {
                var distance = Vector3.Distance(tfMainCharacter.position, tileds[i].position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    index = i;
                }
            }
            return index;
        }
    }
}
