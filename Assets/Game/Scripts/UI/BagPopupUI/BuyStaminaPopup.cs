using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyStaminaPopup : PopupUI
{
    [SerializeField] Button btnBuy;
    [SerializeField] Button btnViewVid;
    [SerializeField] Button btnClose;
    [SerializeField] int gemBuy = 100;
    [SerializeField] int energy = 15;
    [SerializeField] TextMeshProUGUI tmpBuyEnergy;
    [SerializeField] TextMeshProUGUI tmpBuyPrice;
    [SerializeField] private AdmobAdsScript rewardAd;
    private int idRewards = 0;
    protected override void Awake()
    {
        base.Awake();
        btnBuy.onClick.AddListener(OnClickBuy);
        btnViewVid.onClick.AddListener(OnClickViewVid);
        btnClose.onClick.AddListener(OnClickClose);
        tmpBuyEnergy.text = String.Concat("x ", energy);
        tmpBuyPrice.text = String.Concat("x ", gemBuy);
        rewardAd.collectRewards += OnViewSuccess;
    }

    private void OnClickClose()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        OnClose();
    }

    private void OnClickViewVid()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
    }

    private void OnViewSuccess(int _id)
    {

        if (idRewards == _id)
        {
            GameData.Instance.playerData.AddCurrency(Currency.STAMINA, 5);
            OnClose();
        }
    }

    private void OnClickBuy()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        //string text = I2.Loc.LocalizationManager.GetTranslation(DictString.BUY_STAMINA_QUESTION);
        AlertPanel.Instance.ShowNotice(DictString.BUY_STAMINA_QUESTION, OnClickOk, null, String.Empty);
    }

    private void OnClickCancel()
    {
        OnClose();
    }

    private void OnClickOk()
    {
        if (GameData.Instance.playerData.CheckEnoughCurrency(Currency.GEM, gemBuy))
        {
            GameData.Instance.playerData.AddCurrency(Currency.GEM, -gemBuy);
            GameData.Instance.playerData.AddCurrency(Currency.STAMINA, energy);
        }
        else
        {
            //string text = I2.Loc.LocalizationManager.GetTranslation(DictString.NOT_ENOUGH_GEM);
            AlertPanel.Instance.ShowNotice(DictString.NOT_ENOUGH_GEM, null);
        }
        OnClose();
    }
    private void OnClose()
    {
        UIManagerHome.Instance.Back();
    }
}
