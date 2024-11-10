using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletYoyo : MonoBehaviour
{
    LineRenderer line;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    private void Spawned(SpawnPool pool)
    {
        line.enabled = true;
    }
    private void Update()
    {
        if (line)
        {
            line.SetPosition(0, GameDynamicData.mainCharacter.transform.position);
            line.SetPosition(1, this.transform.position);
        }
    }
    private void Despawned(SpawnPool pool)
    {
        line.enabled = false;
        if (line)
        {
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
        }
    }
}
