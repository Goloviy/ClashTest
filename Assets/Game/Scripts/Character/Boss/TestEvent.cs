using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : MonoBehaviour
{
    SkeletonAnimation skeAnim;
    [SpineAnimation(dataField: "skeAnim")]
    public string animationName = "";
    private void Start()
    {
        skeAnim = GetComponent<SkeletonAnimation>();
        skeAnim.state.Event += OnEvent;
    }

    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        
        if (e.Data.Name.Equals("fire"))
        {
            DebugCustom.Log("fire");
        }
        else if (e.Data.Name.Equals("start"))
        {
            DebugCustom.Log("start");
        }
        else if (e.Data.Name.Equals("end"))
        {
            DebugCustom.Log("end");
        }
    }
}
