using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFase : MonoBehaviour
{
    [SerializeField] Button btnFinish;
    Action actionFinish;
    bool isEnd = false;
    private void Awake()
    {
        btnFinish.onClick.AddListener(OnClick);
    }
    public void Init(Action actionFinish)
    {
        this.actionFinish = actionFinish;
    }
    private void OnClick()
    {
        if (!isEnd)
        {
            actionFinish?.Invoke();
            isEnd = true;
        }
    }
}
