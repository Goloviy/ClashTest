using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInformationPopupUI : PopupUI
{
    [SerializeField] TextMeshProUGUI tmpTitle;
    [SerializeField] TextMeshProUGUI tmpDescriptions;
    [SerializeField] Button btnClose;

    protected override void Awake()
    {
        base.Awake();
        btnClose.onClick.AddListener(OnClickClose);

    }
    public override void Open()
    {
        base.Open();
        Init(GameDynamicData.SelectedItem);
    }
    public void Init(UserItem userItem)
    {
        //this.userItem = userItem;
        var itemData = GameData.Instance.staticData.GetEquipmentData(userItem.itemId);
        //var rarity = GameData.Instance.staticData.GetRarity(userItem.rarity);
        //string txtTit = I2.Loc.LocalizationManager.GetTranslation(itemData.title);
        tmpTitle.text = itemData.title;
        //tmpTitle.color = rarity.color;
        //string txtDes = I2.Loc.LocalizationManager.GetTranslation(itemData.descriptions);
        tmpDescriptions.text = itemData.descriptions;
    }

    private void OnClickClose()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        UIManagerHome.Instance.Back();
    }
}
