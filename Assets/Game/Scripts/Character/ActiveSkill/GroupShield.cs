using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroupShield : MonoBehaviour
{
    List<Collider2D> colliders;
    List<ShieldElement> shields;
    float timeRotate = 1.5f;
    ShieldSkill skillBase;
    float refreshTime;
    bool isActiveShield = false;
    bool isInit;

    private void Update()
    {
        if (!isInit)
        {
            return;
        }
        if (!isActiveShield)
        {
            if (Time.time - refreshTime > skillBase.CurTimeInterval)
            {
                refreshTime = Time.time;
                ActiveShieldElements(true);
            }
        }
        else
        {
            if (Time.time - refreshTime > skillBase.CurActiveTime)
            {
                refreshTime = Time.time;
                ActiveShieldElements(false);
            }
        }
    }
    private void ActiveShieldElements(bool enable)
    {
        isActiveShield = enable;
        foreach (var shield in shields)
        {
            if (enable)
                shield.Active();
            else
                shield.Deactive();
        }
    }
    public void Init(ShieldSkill skill)
    {
        colliders = GetComponentsInChildren<Collider2D>().ToList();
        shields = new List<ShieldElement>();
        this.skillBase = skill;
        foreach (var item in colliders)
        {
            var element = item.gameObject.AddComponent<ShieldElement>();
            element.Init(skill);
            shields.Add(element);
        }
        this.transform.DORotate(-Vector3.forward * 360, 2f, RotateMode.FastBeyond360)
                        .SetLoops(-1)
                        .SetEase(Ease.Linear);
        ActiveShieldElements(false);
        refreshTime = Time.time + 1f;
        isInit = true;

    }
}
