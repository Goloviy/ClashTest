using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI tmpTitle;

    [SerializeField] Image imgIcon;
    [SerializeField] Image imgBG;
    [SerializeField] GroupPriceUI groupPriceUI;
    Button btn;

    const string SLASH = " x";
    ShopItem shopItem;
    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickBuy);
    }
    public void Init(ShopItem shopItem)
    {
        if (shopItem is ShopCurrencyItem currencyItem)
        {
            this.shopItem = currencyItem;
            InitItem();
        }
        else
        {
            this.shopItem = shopItem;
            InitEquipment();
        }
    }
    private void InitUI(string title, Sprite icon, Sprite bg, Color color, Currency priceCurrency, long priceValue, long sellValue)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(title);
        if (sellValue > 1)
        {
            builder.Append(SLASH);
            builder.Append(sellValue);
        }
        tmpTitle.text = builder.ToString() ;
        imgIcon.overrideSprite = icon;
        imgBG.color = color;
        groupPriceUI.Init(priceCurrency, priceValue);
    }
    public void InitItem()
    {
        if (shopItem is ShopCurrencyItem currencyItem)
        {
            var currencyData = GameData.Instance.staticData.GetCurrencyData(currencyItem.sellCurrency);
            var rarityData = GameData.Instance.staticData.GetRarity(shopItem.rarity);
            //string currencyTitle = I2.Loc.LocalizationManager.GetTranslation(currencyData.titKey);
            InitUI(currencyData.title, currencyData.icon, null, rarityData.color, shopItem.priceCurrency, shopItem.priceValue, currencyItem.sellValue);
        }


    }
    public void InitEquipment()
    {
        var itemData = GameData.Instance.staticData.GetEquipmentData(shopItem.itemId);
        var rarityData = GameData.Instance.staticData.GetRarity(shopItem.rarity);
        int index = 0;
        if (itemData.spriteIcons.Length > (int)shopItem.rarity)
            index = (int)shopItem.rarity;
        //string txtTit = I2.Loc.LocalizationManager.GetTranslation(itemData.title);
        InitUI(itemData.title, itemData.spriteIcons[index], null, rarityData.color, shopItem.priceCurrency, shopItem.priceValue, 1);

    }

    /// <summary>
    /// Buy Item
    /// </summary>
    public void OnClickBuy()
    {
        if (GameData.Instance.playerData.CheckEnoughCurrency(shopItem.priceCurrency, shopItem.priceValue))
        {
            //buy success
            GameData.Instance.playerData.AddCurrency(shopItem.priceCurrency, -shopItem.priceValue);
            OnBuySuccess();
            DebugCustom.Log("Buy Success");
        }
        else
        {
            DebugCustom.Log("Not enough money");
            //not enough money
        }
    }
    private void OnBuySuccess()
    {

        if (shopItem is ShopCurrencyItem currencyItem)
        {
            GameData.Instance.playerData.AddCurrency(currencyItem.sellCurrency, currencyItem.sellValue);
        }
        else
        {
            GameData.Instance.playerData.saveData.AddEquipmentToBag(shopItem.itemId, shopItem.rarity);
        }
        GameData.Instance.playerData.saveData.SavePlayerData();

    }
}
