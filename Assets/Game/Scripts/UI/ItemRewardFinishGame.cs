using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRewardFinishGame : MonoBehaviour
{
    [SerializeField] Image imgRarity;
    [SerializeField] Image imgIcon;
    [SerializeField] TextMeshProUGUI tmpCount;

    public void Init(Sprite rarity, Sprite icon, long count)
    {
        imgRarity.overrideSprite = rarity;
        imgIcon.overrideSprite = icon;
        tmpCount.text = string.Concat("x", count.ToShortStringK());
    }

}
