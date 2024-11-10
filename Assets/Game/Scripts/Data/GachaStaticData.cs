using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaStaticData 
{
    ItemGachaShopData[] arrayGacha;
    GachaEquipmentSystemData[] arrayGachaContainer;
    Dictionary<GachaType, ItemGachaShopData> dictItemShop;
    Dictionary<GachaType, GachaEquipmentSystemData> dictGachaSystem;
    public GachaStaticData()
    {
        arrayGacha = Resources.LoadAll<ItemGachaShopData>(StringConst.PATH_SHOP_GACHA_DATA);
        dictItemShop = new Dictionary<GachaType, ItemGachaShopData>();
        foreach (var gacha in arrayGacha)
        {
            dictItemShop.Add(gacha.gachaType, gacha);
        }
        Array.Clear(arrayGacha, 0, arrayGacha.Length);

        arrayGachaContainer = Resources.LoadAll<GachaEquipmentSystemData>(StringConst.PATH_SYSTEM_GACHA_DATA);
        dictGachaSystem = new Dictionary<GachaType, GachaEquipmentSystemData>();
        foreach (var gachaSystemData in arrayGachaContainer)
        {
            dictGachaSystem.Add(gachaSystemData.gachaType, gachaSystemData);
        }
        Array.Clear(arrayGachaContainer, 0, arrayGachaContainer.Length);
    }
    public ItemGachaShopData GetDataItemShop(GachaType gachaType)
    {
        ItemGachaShopData data = null;
        if (!dictItemShop.TryGetValue(gachaType, out data))
        {
            DebugCustom.LogError("Pool Gacha doesn't available :" + gachaType);
        }
        return data;
    }
    public GachaEquipmentSystemData GetSystemData(GachaType gachaType)
    {
        GachaEquipmentSystemData data = null;
        if (!dictGachaSystem.TryGetValue(gachaType, out data))
        {
            DebugCustom.LogError("Pool System Gacha doesn't available :" + gachaType);
        }
        return data;
    }
}
