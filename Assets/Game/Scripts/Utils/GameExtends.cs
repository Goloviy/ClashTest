using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class GameExtends 
{
    public static string GetKey(this UserEquipment userEquipment)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(userEquipment.itemId);
        builder.Append(userEquipment.rarity);
        return builder.ToString();
    }
    public static int ToRewardChapter (this int rewardChestProgress)
    {
        if (rewardChestProgress == 0)
        {
            return 1;
        }
        else
        {
            return rewardChestProgress / 100;
        }
    }

    public static Currency ToCurrency(this ItemType design)
    {
        switch (design)
        {
            case ItemType.WEAPON:
                return Currency.DESIGN_WEAPON;
            case ItemType.GLOVES:
                return Currency.DESIGN_GLOVES;
            case ItemType.HELMET:
                return Currency.DESIGN_HELMET;

            case ItemType.ARMOR:
                return Currency.DESIGN_ARMOR;

            case ItemType.BOOTS:
                return Currency.DESIGN_BOOTS;

            case ItemType.BELT:
                return Currency.DESIGN_BELT;
            default:
                return Currency.NONE;
        }
    }
}
