using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : MonoBehaviour
{
    [SerializeField] bool isRandomDirect = false;
    [SerializeField] bool isRotateLeft;
    //[SerializeField] bool isRandomSpeed = false;
    [SerializeField] float speed = 10;
    float z;
    private void OnSpawned()
    {
        if (isRandomDirect)
        {
            isRotateLeft = Random.Range(0, 2) == 0;
        }
        //if (isRandomSpeed)
        //{
        //    speed += Random.Range(-speed, speed);
        //}
    }
    void Update()
    {
        z += speed * (isRotateLeft? 1f : -1f);
        transform.rotation = Quaternion.Euler(0, 0, z);
    }
}
