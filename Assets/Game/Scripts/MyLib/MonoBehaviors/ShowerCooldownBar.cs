using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerCooldownBar : MonoBehaviour
{
    [SerializeField] CooldownBarSprite prefabCdBarSprite;
    [SerializeField] float marginY = -1f;
    [SerializeField] float marginX = 0f;
    ICoolDown unit;
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.SKILL_LEVEL_UP_AFTER, OnSkillLevelup);
    }
    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.SKILL_LEVEL_UP_AFTER, OnSkillLevelup);

    }
    private void OnSkillLevelup(Component arg1, object arg2)
    {
        if (unit == null || (unit != null && !unit.IsEnable))
        {
            unit = GetComponentInChildren<ICoolDown>();
            if (unit != null)
            {
                var cdBar = Instantiate(prefabCdBarSprite, this.transform);
                cdBar.Init(unit, marginY, marginX);
            }
        }

    }
}
