using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrameDebug : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmp;
    float refreshTime = 0f;
    float deltaTime = 0.5f;
    private void Update()
    {
        if (Time.time - refreshTime > deltaTime)
        {
            int fps = Mathf.RoundToInt(1 / Time.deltaTime);
            tmp.text = fps.ToString();
            refreshTime = Time.time;
        }

    }
}
