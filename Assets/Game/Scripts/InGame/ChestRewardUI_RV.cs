using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestRewardUI_RV : ChestRewardUI
{
    [SerializeField] Button btnRV;
    protected override void Awake()
    {
        base.Awake();
        btnRV.onClick.AddListener(OnClickRV);
        btnRV.gameObject.SetActive(true);
        btnSkip.gameObject.SetActive(false);
    }

    private void OnClickRV()
    {
        OnRVSuccess(1);
    }

    private void OnRVFail()
    {
        
    }

    private void OnRVSuccess(int obj)
    {
        btnRV.gameObject.SetActive(false);
        resultData = AddMoreSkill(2, resultData);
        if (resultData != null && resultData.Length >= indexShow)
        {
            CreateSingleItem(resultData[indexShow++]);
        }
    }

    public override void Open()
    {
        if (GameDynamicData.mainCharacter == null)
        {
            return;
        }
        if (originPosChest == Vector3.zero)
        {
            originPosChest = tfChest.localPosition;
        }
        tfChest.localPosition = originPosChest;
        canSkip = false;
        isEndReward = false;
        spineChest.AnimationState.SetAnimation(0, animClose, false);
        btnClose.gameObject.SetActive(false);
        indexShow = 0;
        //base.Open();
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_OPEN_POPUP);
        originScale = Vector3.one;
        canSelect = false;
        if (tfBlackPanel)
            tfBlackPanel.gameObject.SetActive(true);
        if (OpenAnim == OpenPopupAnim.ZOOM_IN)
        {
            this.transform.localScale = originScale * 0.3f;
            this.gameObject.SetActive(true);
            this.transform.DOScale(originScale, 0.35f).SetUpdate(true).onComplete += () => { canSelect = true; };
        }
        else
        {
            canSelect = true;
            this.gameObject.SetActive(true);
        }



        resultData = GetResultSkill(1);
        if (resultData != null && resultData.Length > 0)
        {
            CreateSingleItem(resultData[indexShow++]);
        }
    }
    protected override void OnClickSkip()
    {
        base.OnClickSkip();
        btnSkip.gameObject.SetActive(false);
        btnRV.gameObject.SetActive(false);
    }
    protected override void OnNextSingleItem()
    {
        if (curItem)
        {
            GameObject.Destroy(curItem.gameObject);
            curItem = null;
        }
        if (indexShow < resultData.Length)
        {

            CreateSingleItem(resultData[indexShow++]);
        }
        else
        {
            btnSkip.gameObject.SetActive(true);
        }
    }
}
