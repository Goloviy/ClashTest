using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCage : MonoBehaviour
{

    [SerializeField] float range;
    private void Awake()
    {
        GameDynamicData.BirdCage = this;
    }
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.BOSS_DIE, OnBossDie);
    }
    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.BOSS_DIE, OnBossDie);

    }
    private void OnBossDie(Component arg1, object arg2)
    {
        GameDynamicData.BirdCage = null;
        Destroy(this.gameObject);
    }

    public Vector3 GetRandomPosAroundPlayer()
    {
        var rdPos = Vector3.zero;
        do
        {
            rdPos = (Vector3)Random.insideUnitCircle * (4f) + GameDynamicData.mainCharacter.transform.position;
        } while (!ValidPos(rdPos));
        return rdPos;
    }
    public Vector3 GetRandomPosInsdeCage()
    {
        var rdPos = Vector3.zero;
        do
        {
            rdPos = (Vector3)Random.insideUnitCircle * range + GameDynamicData.mainCharacter.transform.position;
        } while (!ValidPos(rdPos));
        return rdPos;
    }
    public bool ValidPos(Vector3 targetPos)
    {
        var distance = Vector3.Distance(this.transform.position, targetPos);
        return distance < range;
    }
    private void OnDrawGizmos()
    {
        
        Gizmos.DrawWireSphere(this.transform.position, range);

    }
}
