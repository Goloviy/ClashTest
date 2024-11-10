using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    [SerializeField] float deltaTimeNormalAttack = 0.25f;
    protected float lastTimeDealDamage = Mathf.NegativeInfinity;

    [HideInInspector] public SkillBase owner = null;
    bool isInit;
    public void Init(SkillBase owner)
    {
        this.owner = owner;
        isInit = true;
    }
}
