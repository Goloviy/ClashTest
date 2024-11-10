using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public StaticData staticData;
    public PlayerData playerData;
     

    private void Awake()
    {
        staticData = new StaticData();
        playerData = new PlayerData();
        DontDestroyOnLoad(this.gameObject);
    }
}
