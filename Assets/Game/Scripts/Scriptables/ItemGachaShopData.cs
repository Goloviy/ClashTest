using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemGachaShopData", menuName = "ScriptableObjects/Shop/ItemGachaShopData", order = 1)]

public class ItemGachaShopData : ScriptableObject
{
    public SkeletonDataAsset spineChest;
    public string title = "Pack";
    public GachaType gachaType;
    public Sprite iconSprite;
    public Currency currencyPrice;
    public int currencyValue;
    public string content = "Open x1";
    public bool isSecondOption;
    [ShowIf("isSecondOption", true)]
    public Currency currencyPrice2;
    [ShowIf("isSecondOption", true)]

    public int currencyValue2;
    [ShowIf("isSecondOption", true)]

    public string content2 = "Open x10";

}
