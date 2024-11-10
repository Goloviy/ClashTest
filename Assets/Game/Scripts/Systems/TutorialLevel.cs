using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    [SerializeField] TutorialFase[] listFase;
    public int levelTut = 1;
    int curIndex = 0;
    Action finishTut;
    public void Show(Action finishTut)
    {
        this.gameObject.SetActive(true);
        this.finishTut = finishTut;
        ShowFase();
    }
    private void ShowFase()
    {
        listFase[curIndex].Init(OnFinish);
    }

    private void OnFinish()
    {
        curIndex++;
        if (curIndex < listFase.Length)
        {
            ShowFase();
        }
        else
        {
            this.gameObject.SetActive(false);
            finishTut?.Invoke();
        }
    }
}
