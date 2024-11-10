using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShotgunDirect : MonoBehaviour
{
    public List<Transform> directs;
    public Transform main;
    private void Awake()
    {
        directs = main.GetComponentsInChildren<Transform>().ToList();
        directs.Remove(main);
        this.gameObject.SetActive(false);
    }
}
