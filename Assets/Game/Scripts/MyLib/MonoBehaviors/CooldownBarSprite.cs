using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBarSprite : MonoBehaviour
{
    [SerializeField] Transform tfFill;

    ICoolDown unit;
    float curCooldown;
    float curTimeRemainning;
    bool isRefresh = true;
    public void Init(ICoolDown unit, float marginY, float marginX)
    {
        this.transform.position = this.transform.parent.position + Vector3.one * marginY + Vector3.right * marginX;
        this.unit = unit;
    }
    private void Update()
    {
        if (curTimeRemainning != unit.TimeRemainning || curCooldown != unit.TimeCoolDown)
        {
            curTimeRemainning = unit.TimeRemainning;
            curCooldown = unit.TimeCoolDown;
            UpdateBar();
        }
    }
    private void UpdateBar()
    {
        curTimeRemainning = curTimeRemainning <= 0f ? 0 : curTimeRemainning;
        var scale = tfFill.localScale;
        var fill = Mathf.Min(curTimeRemainning / curCooldown, 1);
        scale.x = fill;
        tfFill.localScale = scale;
        if (isRefresh && fill > 0.8f)
        {
            EventDispatcher.Instance.PostEvent(EventID.MAIN_WEAPON_ACTION);
            isRefresh = false;
        }
        if (!isRefresh && fill < 0.8f)
        {
            isRefresh = true;
        }
    }
}
