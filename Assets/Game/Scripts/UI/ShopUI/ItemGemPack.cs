using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGemPack : MonoBehaviour
{
    [SerializeField] Sprite[] SpriteBGs;
    public TextMeshProUGUI tmpTitle;
    public TextMeshProUGUI tmpGemCount;
    public Image imgIcon;
    public Image imgBG;
    public TextMeshProUGUI tmpContent;
    const string CONTENT = "Purchase Pack Gem with only: <size=80>";
    public Button btn;
    ItemIapGemData itemData;
    public void Init(ItemIapGemData itemData)
    {
        this.itemData = itemData;
        if (itemData.gradeLevel < SpriteBGs.Length)
            imgBG.overrideSprite = SpriteBGs[itemData.gradeLevel];
        else
            imgBG.overrideSprite = SpriteBGs[0];
            
        tmpTitle.text = itemData.title;
        tmpGemCount.text = itemData.gem.ToShortString();
        tmpContent.text = String.Concat(CONTENT, GetPrice(itemData.productId));
        imgIcon.overrideSprite = itemData.spr;
    }
    public string GetPrice(string productID)
    {
        return String.Empty;
    }
    private void OnEnable()
    {
        btn.onClick.AddListener(OnClickBuyPack);
    }
    private void OnDisable()
    {
        
        btn.onClick.RemoveListener(OnClickBuyPack);
    }

    private void OnClickBuyPack()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (itemData)
        {
            OnBuySuccess();
        }
    }
    private void OnBuySuccess()
    {
        GameData.Instance.playerData.saveData.AddCurrency(Currency.GEM, itemData.gem);
    }
    private void OnBuyFail()
    {

    }
}
