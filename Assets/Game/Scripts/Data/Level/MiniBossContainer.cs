using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mini Boss", menuName = "ScriptableObjects/Spawner/Mini Boss Data", order = 1)]

[System.Serializable]
public class MiniBossContainer : BaseScriptObject
{
    /// <summary>
    /// Seconds from start play
    /// </summary>
    public int timeSpawn;
    public int idMonster;
    public int hp;
    public int atk;
    public float mSpeed;
    public float sizeMultiply;
    public GameObject PrefabMonster;
    public DropBox[] dropBox;
}