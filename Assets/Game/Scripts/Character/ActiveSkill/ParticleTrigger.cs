using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [HideInInspector] public SkillBase owner = null;
    protected Collider2D collider;

    Vector3 originScale;
    [SerializeField] bool isDuringDamage;
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        originScale = this.transform.localScale;
    }

    protected void OnDespawned(SpawnPool pool)
    {        
        this.transform.localScale = originScale;
    }
    public virtual void Init(SkillBase owner)
    {
        this.owner = owner;
        ScaleBySkill();
        collider.enabled = true;
    }
    protected virtual void ScaleBySkill()
    {
        this.transform.localScale = originScale * CharacterStatusHelper.CalculateBulletScale(owner);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDuringDamage)
        {
            collider.enabled = false;
        }
    }
}
