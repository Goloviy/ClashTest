using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangGroupDirect : MonoBehaviour
{
    public List<PathBoomerang> paths;
}
[System.Serializable]
public class PathBoomerang
{
    public bool rotateLeft;
    public Transform[] points;
}