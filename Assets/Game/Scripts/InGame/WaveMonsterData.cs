using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Spawner/Wave", order = 1)]
[System.Serializable]
public class WaveMonsterData : BaseScriptObject
{
    public bool isWarning = false;
    public float timeInterval = 1f;
    /// <summary>
    /// Time base on Timeline (Time.time)
    /// </summary>
    public float timeStart;
    public float timeEnd;
    public float minDistance = 9f;
    public float deltaDistance = 1f;
    public MonsterContainerData[] containers;
}
