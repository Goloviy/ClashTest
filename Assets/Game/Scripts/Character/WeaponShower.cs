using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShower : MonoBehaviour
{
    [SerializeField] FindOpponentSystem findOpponentSystem;
    [SerializeField] SkeletonAnimation skeletonAnimation;
    [SerializeField] EquipmentShowerData[] itemDatas;
    bool isShowWeapon = false;
    //int indexWeapon = -1;
    string ikBoneName = "target";
    EquipmentShowerData curItemData;
    Spine.Bone targetBone;
    Spine.Bone flipBone;
    bool isTurnLeft = true;
    private void OnEnable()
    {
        skeletonAnimation.gameObject.SetActive(false);
        EventDispatcher.Instance.RegisterListener(EventID.SKILL_LEVEL_UP_AFTER, OnLevelupSkill);
        EventDispatcher.Instance.RegisterListener(EventID.MAIN_WEAPON_ACTION, OnMainWeaponAction);

    }

    private void OnMainWeaponAction(Component arg1, object arg2)
    {
        if (curItemData != null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, curItemData.animName, false);
        }
    }
    private void Update()
    {
        if (curItemData == null)
        {
            return;
        }
        if (curItemData.isIk && targetBone != null)
        {
            var point = findOpponentSystem.GetMainDirect();
            targetBone.SetPositionSkeletonSpace(point);
            if (flipBone != null)
            {
                if (point.x < 0 && !isTurnLeft)
                {
                    isTurnLeft = true;
                    FlipBone();
                }
                else if (point.x > 0 && isTurnLeft)
                {

                    isTurnLeft = false;
                    FlipBone();
                }
            }
        }
        else if (curItemData.isFlipSpine && !curItemData.isIk)
        {
            var point = findOpponentSystem.GetMainDirect();
            if (point.x < 0 && !isTurnLeft)
            {
                isTurnLeft = true;
                FlipSpine();
            }
            else if (point.x > 0 && isTurnLeft)
            {

                isTurnLeft = false;
                FlipSpine();
            }
        }

        void FlipSpine()
        {
            var curScale = skeletonAnimation.transform.localScale;
            curScale.x = isTurnLeft ? 1f : -1f;
            skeletonAnimation.transform.localScale = curScale;
        }
        void FlipBone()
        {
            flipBone.ScaleX = isTurnLeft ? 1f : -1f;

        }
    }
    private void OnLevelupSkill(Component arg1, object arg2)
    {
        //if (isShowWeapon)
        //    return;

        var skillLearn = (SkillName) arg2;
        DebugCustom.Log("skillLearn :" + skillLearn.ToString());
        for (int i = 0; i < itemDatas.Length; i++)
        {
            if(itemDatas[i].skill == skillLearn)
            {
                curItemData = itemDatas[i];
                //indexWeapon = i;
                skeletonAnimation.gameObject.SetActive(true);
                isShowWeapon = true;
                skeletonAnimation.AnimationState.SetAnimation(0, itemDatas[i].animName, false);
                if (curItemData.isIk)
                {
                    targetBone = skeletonAnimation.Skeleton.FindBone(ikBoneName);
                    flipBone = skeletonAnimation.Skeleton.FindBone(curItemData.nameBoneFlip);
                }
            }
        }
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.SKILL_LEVEL_UP_AFTER, OnLevelupSkill);
        EventDispatcher.Instance.RemoveListener(EventID.MAIN_WEAPON_ACTION, OnLevelupSkill);
        
    }

}
[System.Serializable]
public class EquipmentShowerData
{
    public bool isIk;
    public bool isFlipSpine;
    public SkillName skill;
    public string animName;
    [ShowIf("isIk", true)]
    public string nameBoneFlip;
    
}