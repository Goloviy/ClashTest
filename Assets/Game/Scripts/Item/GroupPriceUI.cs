using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroupPriceUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpPrice;
    [SerializeField] Image imgCurrency;
    public void Init(Currency currency, long price)
    {
        
        var currencyData = GameData.Instance.staticData.GetCurrencyData(currency);
        if (currencyData)
        {
            imgCurrency.overrideSprite = currencyData.icon;
            tmpPrice.text = price.ToShortString();
            var rarityData = GameData.Instance.staticData.GetRarity(currencyData.rarity);
            tmpPrice.color = rarityData.color;
        }

    }
}
