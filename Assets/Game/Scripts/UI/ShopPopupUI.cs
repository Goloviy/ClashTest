using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopupUI : PopupUI
{
    [Title("UI BlackMarket Old")]
    [SerializeField] ItemShopUI prefabItem;
    [SerializeField] Transform tfBlackMarket;
    //[SerializeField] TextMeshProUGUI tmpGold;
    //[SerializeField] TextMeshProUGUI tmpGem;
    int countItemBlackMarket = 6;
    ShopItem[] listItem;
    ItemShopUI[] listItemUI = new ItemShopUI[6];

    [Title("Panels")]
    [SerializeField] GameObject goGachaPanel; 
    [SerializeField] GameObject goPremiumPanel; 
    [SerializeField] GameObject goStandardPanel;
    
    [Title("Buttons Naviagtions")]
    [SerializeField] Button btnGacha;
    [SerializeField] Button btnPremium;
    [SerializeField] Button btnStandard;


    protected override void Awake()
    {
        base.Awake();
        btnGacha.onClick.AddListener(OnClickGacha);
        btnPremium.onClick.AddListener(OnClickPremium);
        btnStandard.onClick.AddListener(OnClickStartdard);

        OnClickGacha();
    }

    private void OnClickStartdard()
    {
        SoundController.Instance.PlaySound( SOUND_TYPE.UI_BUTTON_CLICK);
        goGachaPanel.SetActive(false);
        goPremiumPanel.SetActive(false);
        goStandardPanel.SetActive(true);
    }

    private void OnClickPremium()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        goGachaPanel.SetActive(false);
        goPremiumPanel.SetActive(true);
        goStandardPanel.SetActive(false);
    }

    private void OnClickGacha()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        goGachaPanel.SetActive(true);
        goPremiumPanel.SetActive(false);
        goStandardPanel.SetActive(false);
    }

    public override void Open()
    {
        base.Open();
        //InitBlackMarket();
    }


    #region Black Market old
    private void InitBlackMarket()
    {
        GetBlackMarketItem();
        SetupUI();

    }
    private void SetupUI()
    {
        ClearUI();
        for (int i = 0; i < countItemBlackMarket; i++)
        {
            CreateElementUI(i);
        }

        void CreateElementUI(int index)
        {
            var item = Instantiate(prefabItem, tfBlackMarket);
            item.Init(listItem[index]);
            listItemUI[index] = item;
        }
    }

    private void ClearUI()
    {
        foreach (var item in listItemUI)
        {
            if (null != item)
            {
                Destroy(item.gameObject);
            }
        }
        listItemUI = new ItemShopUI[6];
    }
    private void GetBlackMarketItem()
    {
        listItem = GameData.Instance.staticData.GetListShopItem();
    }
    #endregion Black Market old
}
