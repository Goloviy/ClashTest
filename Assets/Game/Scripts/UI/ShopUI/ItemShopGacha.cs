using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopGacha : MonoBehaviour
{
    /// <summary>
    /// Id for saved game
    /// </summary>
    public string packageId;
    private bool IsPlayVideoRewardToday  => long.Parse( PlayerPrefs.GetString(packageId, "0")) > DateTimeOffset.UtcNow.ToUnixTimeSeconds(); 
    
    private void SetTimeNextReward()
    {
        PlayerPrefs.SetString(packageId, DateTimeOffset.UtcNow.AddHours(23).ToUnixTimeSeconds().ToString());
    }

    [SerializeField] Button btn;
    [SerializeField] Button btn2;
    [SerializeField] Button btnRewardVideo;
    [SerializeField] Button btninfo;
    [SerializeField] GachaType gachaType;
    [SerializeField] TextMeshProUGUI tmpTitle;
    [SerializeField] TextMeshProUGUI tmpPrice1;
    [SerializeField] TextMeshProUGUI tmpContent1;
    [SerializeField] TextMeshProUGUI tmpPrice2;
    [SerializeField] TextMeshProUGUI tmpContent2;
    [SerializeField] Image iconCurrency1;
    [SerializeField] Image iconCurrency2;
    [SerializeField] Image iconChest;
    ItemGachaShopData itemData;

    [SerializeField] GachaInfoPopup goInforGachaPanel;
    private void OnEnable()
    {
        itemData = GameData.Instance.staticData.gachaData.GetDataItemShop(gachaType);
        if (itemData)
        {
            var currency = GameData.Instance.staticData.currenciesData.GetData(itemData.currencyPrice);
            iconCurrency1.overrideSprite = currency.icon;
            tmpPrice1.text = string.Concat("", itemData.currencyValue.ToShortString());
            tmpContent1.text = itemData.content;
            tmpTitle.text = itemData.title;
            btn.onClick.AddListener(OnClickOption1);
            btninfo.onClick.AddListener(OnClickInfo);
            btnRewardVideo.gameObject.SetActive(!IsPlayVideoRewardToday);
            btnRewardVideo.onClick.AddListener(OnClickRV);

            if (itemData.isSecondOption)
            {
                var currency2 = GameData.Instance.staticData.currenciesData.GetData(itemData.currencyPrice2);
                iconCurrency2.overrideSprite = currency2.icon;
                tmpPrice2.text = string.Concat("", itemData.currencyValue2.ToShortString());
                tmpContent2.text = itemData.content2;
                btn2.onClick.AddListener(OnClickOption2);
            }

            //btn2.gameObject.SetActive(itemData.isSecondOption);
            //iconCurrency2.gameObject.SetActive(itemData.isSecondOption);
            //tmpPrice2.gameObject.SetActive(itemData.isSecondOption);
            //tmpContent2.gameObject.SetActive(itemData.isSecondOption);
        }
    }

    private void OnClickRV()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        OnRewardSuccess(1);
    }

    private void OnRVFail()
    {
        
    }
    void OnRewardSuccess(int point)
    {
        btnRewardVideo.gameObject.SetActive(false);
        SetTimeNextReward();
        GameSystem.Instance.OpenGacha(itemData.gachaType, 1);
        GameDynamicData.curGachaType = itemData.gachaType;
        UIManagerHome.Instance.Open(PopupType.OPEN_GACHA_EQUIPMENT, true);
        GameData.Instance.playerData.saveData.SavePlayerData();
    }
    private void OnClickInfo()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        goInforGachaPanel.Open(gachaType);
    }

    private void OnDisable()
    {
        if (itemData)
        {
            btn.onClick.RemoveListener(OnClickOption1);
            if (itemData.isSecondOption)
            {
                btn2.onClick.RemoveListener(OnClickOption2);
            }
            itemData = null;
        }
    }
    private void OnClickOption1()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        if (itemData)
        {
            var currency = GameData.Instance.staticData.currenciesData.GetData(itemData.currencyPrice);
            var playerHave = GameData.Instance.playerData.saveData.GetNumber(itemData.currencyPrice);
            var requireValue = itemData.currencyValue;
            if (playerHave >= requireValue)
            {
                GameSystem.Instance.OpenGacha(itemData.gachaType, 1);
                GameDynamicData.curGachaType = itemData.gachaType;
                UIManagerHome.Instance.Open(PopupType.OPEN_GACHA_EQUIPMENT, true);
                GameData.Instance.playerData.saveData.AddCurrency(itemData.currencyPrice, -requireValue);
                GameData.Instance.playerData.saveData.SavePlayerData();
            }
            else
            {
                DebugCustom.Log("Not Enough " + itemData.currencyPrice + requireValue);
            }
        }

    }

    private void OnClickOption2()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (itemData && itemData.isSecondOption)
        {
            var currency = GameData.Instance.staticData.currenciesData.GetData(itemData.currencyPrice2);
            var playerHave = GameData.Instance.playerData.saveData.GetNumber(itemData.currencyPrice2);
            var requireValue = itemData.currencyValue2;
            if (playerHave >= requireValue)
            {
                GameDynamicData.curGachaType = itemData.gachaType;
                GameSystem.Instance.OpenGacha(itemData.gachaType, 10);
                UIManagerHome.Instance.Open(PopupType.OPEN_GACHA_EQUIPMENT, true);
                GameData.Instance.playerData.saveData.AddCurrency(itemData.currencyPrice2, -requireValue);
                GameData.Instance.playerData.saveData.SavePlayerData();
            }
            else
            {
                //string text = I2.Loc.LocalizationManager.GetTranslation(DictString.NOT_ENOUGH_SOURCE);
                AlertPanel.Instance.ShowNotice(DictString.NOT_ENOUGH_SOURCE, null);
                //DebugCustom.Log("Not Enough " + itemData.currencyPrice2 + requireValue);
            }
        }
    }
}
