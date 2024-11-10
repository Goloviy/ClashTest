using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarSprite : MonoBehaviour
{
    [SerializeField] Transform tfFill;

    IAlive unit;
    int curHpMax;
    int curHp;
    public void Init(IAlive unit, float marginY, float marginX)
    {
        this.transform.position = this.transform.parent.position + Vector3.one * marginY + Vector3.right * marginX ;
        this.unit = unit;
    }
    private void Update()
    {
        if (curHp != unit.HpRemainning || curHpMax != unit.HpMax)
        {
            curHp = unit.HpRemainning;
            curHpMax = unit.HpMax;
            UpdateBar();
        }
    }
    private void UpdateBar()
    {
        var scale = tfFill.localScale;
        scale.x = (float)curHp / curHpMax;
        tfFill.localScale = scale;
    }
}
