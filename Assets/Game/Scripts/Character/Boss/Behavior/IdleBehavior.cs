using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IdleBehavior : EnemyBehavior
{
    [SerializeField] int timeWait = 1500;
    public override async void Select(Action<EnemyBehaviorState> action)
    {
        base.Select(action);
        PlayAnim(true);
        await Task.Delay(timeWait);
        FinishBehavior();
    }
}
