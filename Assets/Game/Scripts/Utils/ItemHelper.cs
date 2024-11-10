using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemHelper 
{
    /// <summary>
    /// string = id + rarity
    /// </summary>
    public static Dictionary<string, List<UserEquipment>> dictMergeableGroup;
    public static bool IsInventoryChanged = true;
    public static int CalculateStat(int baseStat, float rarityK, int level, ItemType type)
    {
        if (baseStat == 0)
        {
            return 0;
        }
        return Mathf.RoundToInt( baseStat * rarityK + (level - 1) * (rarityK + 2));
    }
    public static int CalculateEnchantRequireGold(int nextLevel, ItemType type)
    {
        return nextLevel * 1000;
    }
    public static int CalculateEnchantRequireDesign(int nextLevel, ItemType type)
    {
        return Mathf.Max(1, (nextLevel / 5) * 3);
    }
    public static ResourceEnchant DownAllLevelEnchant(int curLevel, ItemType type)
    {
        ResourceEnchant resourceEnchant = new ResourceEnchant() { countDesign = 0, gold = 0 };
        for (int i = curLevel; i > 1; i--)
        {

            var resource = DownLevelEnchant(i, type) ;
            resourceEnchant.countDesign += resource.countDesign;
            resourceEnchant.gold += resource.gold;
        }
        return resourceEnchant;
    }
    public static ResourceEnchant DownLevelEnchant(int curLevel, ItemType type)
    {
        ResourceEnchant resourceEnchant = new ResourceEnchant() { countDesign = 0, gold = 0 };
        resourceEnchant.countDesign = CalculateEnchantRequireDesign(curLevel, type);
        resourceEnchant.gold = CalculateEnchantRequireGold(curLevel, type);
        return resourceEnchant;
    }
    public static long CalcultePriceEquiment(int basePrice, Rarity rarity)
    {
        int r = (int)rarity;
        int price = Mathf.RoundToInt(basePrice * Mathf.Pow(3, r));
        return price;
    }

    /// <summary>
    /// From user'equipments . Get list equimentID, which can merge ( >= 3 equipment same)
    /// Only auto merge with rarity epic below
    /// </summary>
    private static void UpdateMergeable()
    {
        dictMergeableGroup = new Dictionary<string, List<UserEquipment>>();
        List<UserEquipment> equipments = GameData.Instance.playerData.saveData.ListEquipments;
        foreach (var userEquipment in equipments)
        {
            if ((int)userEquipment.rarity >= (int)Rarity.Elite)
            {
                //Item rarity not auto merge
                continue;
            }
            List<UserEquipment> groupMerge;
            if (dictMergeableGroup.TryGetValue(userEquipment.GetKey(), out groupMerge))
            {
                groupMerge.Add(userEquipment);
                dictMergeableGroup[userEquipment.GetKey()] = groupMerge;
            }
            else
            {
                groupMerge = new List<UserEquipment>();
                groupMerge.Add( userEquipment);
                dictMergeableGroup.Add(userEquipment.GetKey(), groupMerge);
            }
        }
        //remove group item not enough merge
        List<string> removeKeys = new List<string>();
        foreach (var key in dictMergeableGroup.Keys)
        {
            //var listEquipment = dictMergeableGroup[key];
            //var equipment = listEquipment[0];
            //var rarityData = GameData.Instance.staticData.GetRarity(equipment.rarity);
            //var mergeType = rarityData.mergeType;
            //var subItemCount = rarityData.subCount;
            //if (subItemCount > listEquipment.Count - 1)
            //{
            //    removeKeys.Add(key);
            //    continue;
            //}
            //if (mergeType == MergeType.SAME_ITEM_AND_RARITY)
            //{

            //}
            //else if (mergeType == MergeType.ONLY_SAME_RARITY)
            //{

            //}
            if (dictMergeableGroup[key].Count < 3)
            {
                removeKeys.Add(key);
            }
        }
        foreach (var removeKey in removeKeys)
        {
            dictMergeableGroup.Remove(removeKey);
        }
    }
    public static bool CheckMergeable(UserEquipment _userEquipment)
    {
        if (IsInventoryChanged)
        {
            UpdateMergeable();
            IsInventoryChanged = false;
        }
        foreach (var value in dictMergeableGroup.Values)
        {
            if (value.Contains(_userEquipment))
            {
                return true;
            }
        }
        return false;
    }
}
public struct ResourceEnchant
{
    public int gold;
    public int countDesign;
}