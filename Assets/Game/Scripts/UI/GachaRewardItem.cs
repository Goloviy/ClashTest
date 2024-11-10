using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaRewardItem : MonoBehaviour
{
    [SerializeField] bool isMoveUp = true;
    [SerializeField] float moveX = -120f;
    [SerializeField] Image imgIcon;
    [SerializeField] Image imgIconBG;
    [SerializeField] TextMeshProUGUI tmpTitle;
    [SerializeField] TextMeshProUGUI tmpRarity;
    [SerializeField] GameObject goContent;
    [SerializeField] GameObject goItem;
    UserEquipment elementData;
    Action action;

    bool isSkip = false;
    public void OnSkip()
    {
        DOTween.Kill(GetInstanceID());
        isSkip = true;
        //OnNextStep();
    }

    public void Init(UserEquipment elementData, Action OnFinish)
    {
        this.action = OnFinish;
        this.elementData = elementData;
        LoadUI();
        goContent.SetActive(false);
        goItem.SetActive(true);
        //imgIconBG.gameObject.SetActive(false);
        goItem.transform.DORotate(new Vector3(0, 720, 0), 0.8f, RotateMode.FastBeyond360).SetUpdate(true).onComplete += OnRollFinish;
        if (isMoveUp)
        {
            this.transform.DOLocalMoveY(150f, 1.2f).SetUpdate(true).SetId(GetInstanceID()).onComplete += OnMoveUpFinish;
        }
        else
        {
            OnMoveUpFinish();
        }
    }

    private void OnRollFinish()
    {
        var data = GameData.Instance.staticData.GetEquipmentData(elementData.itemId);
        var rarityData = GameData.Instance.staticData.GetRarity(elementData.rarity);
        imgIconBG.gameObject.SetActive(true);
        imgIcon.overrideSprite = data.spriteIcons[(int)elementData.rarity];
        imgIconBG.overrideSprite = rarityData.border;
    }

    private void OnMoveUpFinish()
    {
        //goItem.transform.DOLocalMoveX(moveX, 0.3f).SetUpdate(true).SetId(GetInstanceID()).onComplete += OnMoveLeftFinish;
        OnMoveLeftFinish();
    }

    private async void OnMoveLeftFinish()
    {
        goContent.SetActive(true);
        await Task.Delay(3000);
        OnNextStep();
    }

    private void OnNextStep()
    {
        if (!isSkip)
        {
            this.action?.Invoke();
        }
    }

    private void LoadUI()
    {
        var data = GameData.Instance.staticData.GetEquipmentData(elementData.itemId);
        var rarityData = GameData.Instance.staticData.GetRarity(elementData.rarity);
        //string txtTit = I2.Loc.LocalizationManager.GetTranslation(data.title);
        tmpTitle.text = data.title;
        tmpTitle.color = rarityData.color;
        //string txtTit2 = I2.Loc.LocalizationManager.GetTranslation(rarityData.title);
        tmpRarity.text = rarityData.title;
        tmpRarity.color = rarityData.color;
    }
}
