using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData 
{
    public SkillsStaticData skillsData;
    public RaritiesStaticData raritiesData;
    //public EquipmentsStaticData equipmentsData;
    public ItemsStaticData items;
    public CurrencyStaticData currenciesData;
    public SlotStaticData designsData;
    public ChapterLevelStaticData levelsData;
    public EnemyData enemyData;
    public ChallengeStaticData challengeData;
    public ModeStaticData modeData;
    public GachaStaticData gachaData;
    public ShopStaticData shopData;
    public StaticData()
    {
        skillsData = new SkillsStaticData();
        raritiesData = new RaritiesStaticData();
        items = new ItemsStaticData();
        currenciesData = new CurrencyStaticData();
        designsData = new SlotStaticData();
        levelsData = new ChapterLevelStaticData();
        enemyData = new EnemyData();
        challengeData = new ChallengeStaticData();
        modeData = new ModeStaticData();
        gachaData = new GachaStaticData();
        shopData = new ShopStaticData();
    }
    public ChapterLevelData GetChapterLevel(int level)
    {
        return levelsData.GetData(level);
    }
    public RarityData GetRarity(Rarity rarity)
    {
        return raritiesData.GetData(rarity);
    }
    public CurrencyData GetCurrencyData(Currency currency)
    {
        return currenciesData.GetData(currency);
    }
    public SlotData GetSlot(ItemType designType)
    {
        return designsData.GetData(designType);
    }
    #region Item
    public ItemBagData GetEquipmentData(string id)
    {
        return items.GetData(id);
    }
    public EquipmentData GetRandomEquipmentData(bool isContainSuper = false)
    {
        return items.GetRandomEquipment(isContainSuper);
    }
    public ShopItem[] GetListShopItem()
    {
        ShopItem[] list = new ShopItem[6];
        int countEquipment = 3;
        for (int i = 0; i < list.Length; i++)
        {
            ShopItem shopItem;
            if (i < countEquipment)
            {
                var userItem = GetRandomUserItemShop();
                EquipmentData equipmentData = GameData.Instance.staticData.GetEquipmentData(userItem.itemId) as EquipmentData;
                int countEquip = 1;
                var priceValue = ItemHelper.CalcultePriceEquiment(equipmentData.basePrice, userItem.rarity) * countEquip;
                shopItem = new ShopItem(userItem.itemId,
                    userItem.rarity, 
                    Currency.GEM,
                    priceValue);
            }
            else
            {
                var sellCurrency = GetRandomDesign();
                var designData = GetCurrencyData(sellCurrency);
                var sellAmount = UnityEngine.Random.Range(1, 7) * 5;
                var priceValue = sellAmount * GameConfigData.Instance.PriceDesign;
                shopItem = new ShopCurrencyItem(sellCurrency,
                    sellAmount,
                    designData.id,
                    designData.rarity,
                    Currency.GOLD,
                    priceValue);
            }
            list[i] = shopItem;
        }
        return list;


        Currency GetRandomDesign()
        {
            Currency currency = (Currency)UnityEngine.Random.Range(80, 86);
            return currency;
        }
        UserEquipment GetRandomUserItemShop()
        {
            UserEquipment userItem;
            var itemData = GetRandomEquipmentData();
            Rarity rarity;
            int level = 1;

            float ucommon = 1f;
            float rare = 0.6f;
            float elite = 0.3f;
            float rd = UnityEngine.Random.Range(0, 1f);
            if (rd < elite)
            {
                rarity = Rarity.Elite;
            }
            else if (rd < rare)
            {

                rarity = Rarity.Rare;
            }
            else
            {
                rarity = Rarity.Uncommon;
            }
            userItem = new UserEquipment(itemData.id, rarity, level);

            return userItem;
        }
    }

    #endregion Item
}
