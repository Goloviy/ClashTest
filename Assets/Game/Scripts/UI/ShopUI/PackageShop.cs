using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PackageShop : MonoBehaviour, IProgductIapPackage
{
    [SerializeField] bool isIapPackage = false;
    [ShowIf("isIapPackage", true)]
    public string iapPackageId = string.Empty;
    [ShowIf("isIapPackage", true)]
    [Tooltip("Localize IAP price")]
    public TextMeshProUGUI tmpPrice;

    [ShowIf("isOneTimePurchase", true)]
    public bool isOneTimePurchase = false;
    [ShowIf("isOneTimePurchase", true)]
    public string productId;
    public string ProductId => productId;

    [SerializeField] CurrencyRewardItem priceData;

    [SerializeField] CurrencyRewardItem[] packItems;
    [SerializeField] EquipmentRewardItem[] packEquipments;
    /// <summary>
    /// Id for saved game
    /// </summary>
    public string packageId;
    private bool IsPurchased => PlayerPrefs.GetInt(packageId, 0) == 1;

    int selectedCount = 1;
    bool isWaitting = false;

    //Purchaser purchaser;
    protected void AssignNewUID()
    {
        packageId = System.Guid.NewGuid().ToString();
    }

    [Button(ButtonSizes.Gigantic)]
    private void ResetId()
    {
        AssignNewUID();
    }

    private void OnEnable()
    {
        HandleOneTimePurchase();
        UIUpdate();
    }
    private void UIUpdate()
    {
        //if (isIapPackage)
        //{
        //    tmpPrice.text = purchaser.GetPriceLocalize(productId);
        //}
    }
    public void Init()
    {
        //this.purchaser = purchaser;
    }
    public void OnClickBuy(int count)
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        if (IsPurchased && isOneTimePurchase)
        {
            return;
        }
        if (isWaitting)
        {
            return;
        }
        if (isIapPackage)
        {
            selectedCount = count;
            isWaitting = true;
            //purchaser.BuyPackage(iapPackageId);
        }
        else
        {
            OnNormalBuy(count);
        }
    }
    private void OnNormalBuy(int count)
    {
        if (BuyPack(count))
        {
            OnBuyPackageSuccess(count);
        }
        else
        {
            //Buy Fail
            //var txt = I2.Loc.LocalizationManager.GetTranslation(DictString.NOT_ENOUGH_SOURCE);
            AlertPanel.Instance.ShowNotice(string.Concat(DictString.NOT_ENOUGH_SOURCE, priceData.number * count) , null);
        }

    }
    private void OnBuyPackageSuccess(int count)
    {
        Collect(count);
        PlayerPrefs.SetInt(packageId, 1);

        HandleOneTimePurchase();
        //var txt = I2.Loc.LocalizationManager.GetTranslation(DictString.BUY_SUCCESS);
        AlertPanel.Instance.ShowNotice(DictString.BUY_SUCCESS, null);
    }
    private void Collect(int count = 1)
    {
            foreach (var collectItem in packItems)
            {
                GameData.Instance.playerData.AddCurrency(collectItem.currency, collectItem.number * count);
            }
            foreach (var uEquipment in packEquipments)
            {
                GameData.Instance.playerData.saveData.AddEquipmentToBag(uEquipment.equipmentData.id, uEquipment.rarity, uEquipment.count * count);
            }
    }
    private bool BuyPack(int count = 1)
    {
        var totalPrice = priceData.number * count;
        if (GameData.Instance.playerData.CheckEnoughCurrency(priceData.currency, priceData.number))
        {
            GameData.Instance.playerData.saveData.AddCurrency(priceData.currency, -totalPrice);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void HandleOneTimePurchase()
    {
        if (isOneTimePurchase)
        {
            if (!IsPurchased)
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                DebugCustom.Log("Deactive Purchased Package");
                this.gameObject.SetActive(false);
            }
        }
    }

    public void PurchaseSuccess()
    {
        isWaitting = false;
        OnBuyPackageSuccess(selectedCount);
    }
    public void PurchaseFail()
    {
        if (isWaitting)
        {
            isWaitting = false;
            //var txt = I2.Loc.LocalizationManager.GetTranslation(DictString.BUY_FAIL);
            AlertPanel.Instance.ShowNotice(DictString.BUY_FAIL, null);
        }

    }
}
