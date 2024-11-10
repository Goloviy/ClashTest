using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    int ProgressTut { get => PlayerPrefs.GetInt("tutprogress", 0); set { PlayerPrefs.SetInt("tutprogress", value); } }

    [SerializeField] TutorialLevel[] tutLevels;

    private void Start()
    {
        //first tut
        if (ProgressTut == 0)
        {
            tutLevels[ProgressTut].Show(null);
            ProgressTut++;
        }
    }

}
