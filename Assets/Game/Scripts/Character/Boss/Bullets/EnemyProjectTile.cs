using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectTile : MonoBehaviour
{
    public bool IsBossBullet = true;
    [HideIf("IsBossBullet", false)]
    public int OwnerId;
    public float multiplyDamge = 1f;

    /// <summary>
    /// Only boss need update OwnerId of bullet
    /// </summary>
    /// <param name="OwnerId"></param>
    public void Init(int OwnerId)
    {
        this.OwnerId = OwnerId;
    }
    
}
