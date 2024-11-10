using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoolDown
{
    float TimeRemainning { get; set; }
    float TimeCoolDown { get; set; }

    bool IsEnable { get; set; }
}