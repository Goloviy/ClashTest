using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Big Boss", menuName = "ScriptableObjects/Spawner/Big Boss Data", order = 1)]

[System.Serializable]
public class BossContainer : BaseScriptObject
{
    /// <summary>
    /// Seconds from start play
    /// </summary>
    public int timeSpawn;
    public int idBoss;
    public int hp;
    public int atk;
    public float mSpeed;
    public float sizeMultiply = 1;
    public GameObject PrefabBoss;
    public DropBox[] dropBox;
}