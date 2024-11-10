using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : Singleton<CameraFollower>
{
    MainCharacter mainCharacter = null;
    const float x = 3.5f;
    const float y = 7f;
    const float x2 = 1.75f;
    const float y2 = 3.5f;
    public bool GetNearCharacter(Vector3 position)
    {
        float deltaX = mainCharacter.transform.position.x - position.x;
        float deltaY = mainCharacter.transform.position.y - position.y;
        if (Mathf.Abs(deltaX) > x2 || Mathf.Abs(deltaY) > y2)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public Vector3 RandomInsideCam()
    {
        var rdX = Random.Range(-x, x);
        var rdY = Random.Range(-y, y);
        return mainCharacter.transform.position + new Vector3(rdX, rdY);
    }
    /// <summary>
    /// Extra range outside camera
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool IsInsideCam(Vector3 position, float extra = 0.5f)
    {
        float deltaX = mainCharacter.transform.position.x - position.x;
        float deltaY = mainCharacter.transform.position.y - position.y ;
        if (Mathf.Abs(deltaX) > x + extra || Mathf.Abs(deltaY) > y + extra)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void LateUpdate()
    {
        if (mainCharacter == null)
        {
            FindTarget();
        }
        else
        {
            this.transform.position = new Vector3(mainCharacter.transform.position.x, mainCharacter.transform.transform.position.y, this.transform.position.z);
        }
    }
    private void FindTarget()
    {
        mainCharacter = FindObjectOfType<MainCharacter>();
    }
}
