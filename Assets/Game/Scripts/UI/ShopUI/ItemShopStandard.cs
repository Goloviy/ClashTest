using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopStandard : MonoBehaviour
{
    [SerializeField] Sprite[] SpriteBGs;

    [SerializeField] TextMeshProUGUI tmpProductValue; 
    [SerializeField] TextMeshProUGUI tmpPrice;
    [SerializeField] Image iconProduct;
    [SerializeField] Image imgBG;
    [SerializeField] Image iconPrice;
    ItemStandardData itemData;
    [SerializeField] Button btn;
    private void OnEnable()
    {
        btn.onClick.AddListener(OnClickBuyPack);
    }

    private void OnClickBuyPack()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        if (itemData)
        {
            //var currencyPriceData = GameData.Instance.staticData.currenciesData.GetData(itemData.currencyPrice);
            var playerHave = GameData.Instance.playerData.saveData.GetNumber(itemData.currencyPrice);
            var requireValue = itemData.priceValue;
            if (playerHave >= requireValue)
                OnBuySuccess();
            else
                OnBuyFail();
        }
    }
    private void OnBuySuccess()
    {
        var requireValue = itemData.priceValue;
        var productValue = itemData.productValue;
        GameData.Instance.playerData.saveData.AddCurrency(itemData.currencyPrice, -requireValue);
        GameData.Instance.playerData.saveData.AddCurrency(itemData.currencyProduct, productValue);

    }
    private void OnBuyFail()
    {

    }
    private void OnDisable()
    {

        btn.onClick.RemoveListener(OnClickBuyPack);
    }
    public void Init(ItemStandardData itemData)
    {
        this.itemData = itemData;
        tmpProductValue.text = itemData.productValue.ToShortString();
        tmpPrice.text = itemData.priceValue.ToShortString();
        //var currencyProductData = GameData.Instance.staticData.GetCurrencyData(itemData.currencyProduct);
        iconProduct.overrideSprite = itemData.icon;
        var currencyPriceData = GameData.Instance.staticData.GetCurrencyData(itemData.currencyPrice);
        iconPrice.overrideSprite = currencyPriceData.icon;

        if (itemData.gradeLevel < SpriteBGs.Length)
            imgBG.overrideSprite = SpriteBGs[itemData.gradeLevel];
        else
            imgBG.overrideSprite = SpriteBGs[0];
    }
}
