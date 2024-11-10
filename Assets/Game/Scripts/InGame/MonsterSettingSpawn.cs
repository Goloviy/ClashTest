using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Monster Container", menuName = "ScriptableObjects/Spawner/Monster Container Data", order = 1)]
[System.Serializable]
public class MonsterSettingSpawn : BaseScriptObject
{
    [HideInInspector] public int idMonster;
    public int hp;
    public int atk;
    public float mSpeed;
    public GameObject PrefabMonster;
    public DropBox[] dropBox;
    [Button(ButtonSizes.Gigantic)]
    private void UpdateData()
    {
        var monster = PrefabMonster.GetComponent<MonsterBase>();
        idMonster = monster.id;
    }
}
