using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickTimer : Singleton<TickTimer>
{
    int lastTimeTick = 0;
    private void Update()
    {
        if (Time.time - lastTimeTick >= 1f && !GameDynamicData.PlayerPlayGame)
        {
            lastTimeTick = Mathf.FloorToInt(Time.time);
            EventDispatcher.Instance.PostEvent(EventID.TIME_TICK_SECONDS, lastTimeTick);
        }
    }
}
