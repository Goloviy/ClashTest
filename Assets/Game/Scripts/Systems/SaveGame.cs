using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : Singleton<SaveGame>
{
    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    [SerializeField] float deltaTime = 5;
    float lastSaveTime = 3;
    private void Update()
    {
        if (Time.time - deltaTime > lastSaveTime && !GameDynamicData.PlayerPlayGame)
        {
            GameData.Instance.playerData.saveData.SavePlayerData();
            lastSaveTime = Time.time;
        }
    }
}
