using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldElement : BulletBase
{
    Vector3 originScale;

    public override void Init(SkillBase owner)
    {
        originScale = this.transform.localScale;
        this.transform.localScale = Vector3.zero;
        base.Init(owner);
    }
    public void Active()
    {
        this.collider.enabled = true;
        this.transform.DOScale(originScale, 0.35f).SetEase(Ease.Linear);
    }
    public void Deactive()
    {
        this.collider.enabled = false;
        this.transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.Linear);
    }
    protected override void DespawnThis()
    {

    }
}
