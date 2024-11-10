using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyData 
{
    List<EnemyBase> listEnemy;
    Dictionary<int, EnemyBase> dictEnemy;

    public EnemyData()
    {
        listEnemy = new List<EnemyBase>();
        listEnemy = Resources.LoadAll<EnemyBase>(StringConst.PATH_PREFAB_ENEMY).ToList();
        dictEnemy = new Dictionary<int, EnemyBase>();
        foreach (var prefabEnemy in listEnemy)
        {
            dictEnemy.Add(prefabEnemy.id, prefabEnemy);
        }
    }
    public EnemyBase GetEnemyData(int id)
    {
        EnemyBase enemy;
        if (dictEnemy.TryGetValue(id, out enemy))
        {
            return enemy;
        }
        else
            return null;
    }
}
