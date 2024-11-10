using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaRewardItemMini : MonoBehaviour
{

    [SerializeField] Image imgIcon;
    [SerializeField] Image imgIconBG;
    [SerializeField] TextMeshProUGUI tmpCount;
    UserEquipment elementData;
    Action action;
    public void Init(UserEquipment elementData, Action OnFinish)
    {
        this.action = OnFinish;
        this.elementData = elementData;
        tmpCount.gameObject.SetActive(false);
        UpdateImage();
    }
    public void Init(Sprite sprBorder, Sprite sprIcon, string strCount, Action OnFinish)
    {
        this.action = OnFinish;
        imgIcon.overrideSprite = sprIcon;
        imgIconBG.overrideSprite = sprBorder;
        int parseCount;
        if (int.TryParse(strCount, out parseCount))
        {
            if (parseCount > 1)
            {
                tmpCount.gameObject.SetActive(true);
                tmpCount.text = String.Concat("x", strCount);
            }
            else
            {
                tmpCount.gameObject.SetActive(false);
            }
        }
        else
        {
            //not parse by convert to K
            tmpCount.text = strCount;
            tmpCount.gameObject.SetActive(true);
        }

    }
    private void UpdateImage()
    {
        var data = GameData.Instance.staticData.GetEquipmentData(elementData.itemId);
        var rarityData = GameData.Instance.staticData.GetRarity(elementData.rarity);
        imgIcon.overrideSprite = data.spriteIcons[(int)elementData.rarity];
        imgIconBG.overrideSprite = rarityData.border;
    }
}
