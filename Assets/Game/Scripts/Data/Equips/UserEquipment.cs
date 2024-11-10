using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserEquipment : UserItem
{
    public int enchantLevel;
    public UserEquipment(string id, Rarity rarity, int enchantLevel) : base(id,rarity, 1)
    {
        this.enchantLevel = enchantLevel;
    }
}
public class ShopCurrencyItem : ShopItem
{
    public Currency sellCurrency;
    public long sellValue;
    public ShopCurrencyItem(Currency sellCurrency, long sellValue, string itemId, Rarity rarity, Currency priceCurrency, long priceValue) : base (itemId, rarity, priceCurrency, priceValue)
    {
        this.sellCurrency = sellCurrency;
        this.sellValue = sellValue;
        this.itemId = itemId;
        this.rarity = rarity;
        this.priceCurrency = priceCurrency;
        this.priceValue = priceValue;
    }
}
public class ShopItem : ICloneable
{
    public string id;
    public string itemId;
    public Rarity rarity;
    public Currency priceCurrency;
    public long priceValue;
    public ShopItem(string itemId, Rarity rarity, Currency priceCurrency, long priceValue)
    {
        this.itemId = itemId;
        this.rarity = rarity;
        this.priceCurrency = priceCurrency;
        this.priceValue = priceValue;
    }

    public object Clone()
    {
        id = System.Guid.NewGuid().ToString();
        return MemberwiseClone();
    }
}
public class UserItem : ICloneable
{
    public string id;
    public string itemId;
    public Rarity rarity;
    public int count;
    public UserItem(string itemId, Rarity rarity, int count)
    {
        this.itemId = itemId;
        this.rarity = rarity;
        this.count = count;
        id = System.Guid.NewGuid().ToString();
    }

    public object Clone()
    {
        id = System.Guid.NewGuid().ToString();
        return MemberwiseClone();
    }
}
//public class ItemShop : UserItem
//{
//    public int count;
//    public ItemShop(string id, Rarity rarity, int count) : base(id, rarity)
//    {
//        this.count = count;
//    }
//}