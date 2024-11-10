using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaInfoCellView : EnhancedScrollerCellView
{
    [SerializeField] Image iconEquipment;
    [SerializeField] Image bgRarity;
    [SerializeField] TextMeshProUGUI tmpTitle;

    public void SetData(EquipmentRewardItem equipmentRewardItem)
    {
        iconEquipment.overrideSprite = equipmentRewardItem.equipmentData.spriteIcons[(int)equipmentRewardItem.rarity];
        bgRarity.overrideSprite = GameData.Instance.staticData.GetRarity(equipmentRewardItem.rarity).border;
            //I2.Loc.LocalizationManager.GetTranslation( equipmentRewardItem.equipmentData.title);
        tmpTitle.text = equipmentRewardItem.equipmentData.title;
    }
}
