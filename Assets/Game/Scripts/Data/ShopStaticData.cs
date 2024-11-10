using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopStaticData 
{
    ItemIapGemData[] premiumItems;
    //Dictionary<string, ItemIapGemData> dictItemPremium;

    ItemStandardData[] standardItems;
    //Dictionary<string, ItemStandardData> dictItemStandard;
    public ShopStaticData()
    {
        premiumItems = Resources.LoadAll<ItemIapGemData>(StringConst.PATH_PREMIUM_DATA);
        //dictItemPremium = new Dictionary<string, ItemIapGemData>();
        //foreach (var item in premiumItems)
        //{
        //    dictItemPremium.Add(item.productId, item);
        //}

        standardItems = Resources.LoadAll<ItemStandardData>(StringConst.PATH_STANDARD_DATA);
        //dictItemStandard = new Dictionary<string, ItemStandardData>();
        //foreach (var item in standardItems)
        //{
        //    dictItemStandard.Add(item.productId, item);
        //}
    }
    public ItemIapGemData[] PremiumItems => premiumItems;
    public ItemStandardData[] StandardItems => standardItems;
    //public ItemIapGemData GetPremiumItem(string productId)
    //{
    //    ItemIapGemData data = null;
    //    if (!dictItemPremium.TryGetValue(productId, out data))
    //    {
    //        DebugCustom.LogError("Premium Items doesn't available :" + productId);
    //    }
    //    return data;
    //}
}
