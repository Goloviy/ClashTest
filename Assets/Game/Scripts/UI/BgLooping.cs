using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgLooping : MonoBehaviour
{
    [SerializeField] Image img;
    Vector2 curOffSet = Vector2.zero;
    void Update()
    {
        
        curOffSet.Set(curOffSet.x - 0.002f < -1 ? 0 : curOffSet.x - 0.002f, curOffSet.y - 0.002f < -1 ? 0 : curOffSet.x - 0.002f);
        img.material.SetTextureOffset("_MainTex", curOffSet);
    }
}
