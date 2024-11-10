using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MonsterContainerData 
{
    public int spawnCount;
    [InlineEditor] public MonsterSettingSpawn monsterSetting;
}
