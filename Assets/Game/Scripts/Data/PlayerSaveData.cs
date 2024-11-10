using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStat
{
    public int hp = 0;
    public int atk = 0;
    public int def = 0;
    public int critRate = 0;
    public float critDamage = 0;
    public float moveSpeed = 0;
}

public class PlayerSaveData
{
    public int AccountLevel { get; private set; } = GameConfigData.Instance.StartAccountLevel;
    public int AccountExp { get; private set; } = 0;
    public int QuickPatrolCount { get; private set; } = 3;
    public int BestChapterLevel { get; private set; } = GameConfigData.Instance.StartChapterLevel;
    public long LastTimePatrolCollect { get; private set; } 
    public long TimeFinishMineStamina { get; private set; } 
    public int CollectChapterChestProgress { get; private set; } = 0;
    /// <summary>
    /// Time survive per chapter (by seconds)
    /// </summary>
    public int[] ChapterSurviveTime { get; private set; }
    public List<UserEquipment> ListEquipped { get; private set; }
    public List<UserEquipment> ListAcquired { get; private set; }
    List<UserEquipment> listEquipments;
    public Dictionary<Currency , long> DictCurrency { get; private set; }
    public List<UserEquipment> ListEquipments 
    { 
        get 
        {
            listEquipments.Clear();
            listEquipments.AddRange(ListEquipped);
            listEquipments.AddRange(ListAcquired);
            return listEquipments;
        }
    }
    //public List<UserItem> ListItem { get; private set; }
    public bool IsDirty { get; set; }
    public PlayerSaveData()
    {
        ListEquipped = new List<UserEquipment>();
        ListAcquired = new List<UserEquipment>();
        //ListItem = new List<UserItem>();
        CollectChapterChestProgress = 0;
        listEquipments = new List<UserEquipment>();
        DictCurrency = new Dictionary<Currency, long>();
        ChapterSurviveTime = new int[GameConfigData.Instance.MaxChapter];
        LoadSavedData();
    }
    private void CreateArraySurviveTime()
    {
        ChapterSurviveTime = new int[GameConfigData.Instance.MaxChapter];
        for (int i = 0; i < GameConfigData.Instance.MaxChapter; i++)
        {
            ChapterSurviveTime[i] = 0;
        }
    }

    #region Patrol
    public bool IsActivePatrol()
    {
        return BestChapterLevel >= GameConfigData.Instance.PatrolStartLevel;
    }
    /// <summary>
    /// Collect time munium is 10 minutes
    /// </summary>
    /// <returns></returns>
    public bool AllowCollectPatrol()
    {
        if (IsFirstPatrol())
        {
            return true;
        }
        else
        {
            int minitesMinium = 10;
            var curUtcTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long patrolTime = curUtcTime - LastTimePatrolCollect;
            long millisecondsMinium = minitesMinium.MinToMilliseconds();
            if (patrolTime > millisecondsMinium)
                return true;
            else
                return false;
        }
    }
    public bool IsPatrolFull()
    {
        return LastTimePatrolCollect + GameConfigData.Instance.PatrolMaxAfkTimeMinutes.MinToMilliseconds() > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
    public void CalculateMineStamina()
    {
        //DateTimeOffset.FromUnixTimeSeconds(GameData.Instance.playerData.saveData.TimeMineStamina - DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        if (GameData.Instance.playerData.saveData.IsMineStamina())
        {
            long totalMineTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - (TimeFinishMineStamina - GameConfigData.Instance.TimeMineStaminaMinutes * 60);
            var mineMinutes = totalMineTime / 60;
            var staminaMineNumber = mineMinutes / GameConfigData.Instance.TimeMineStaminaMinutes;
            //DebugCustom.Log("totalMineTime" + totalMineTime);
            //DebugCustom.Log("mineMinutes" + mineMinutes);
            //DebugCustom.Log("staminaMineNumber" + staminaMineNumber);
            if (staminaMineNumber >= 1)
            {
                var total = staminaMineNumber + GetNumber(Currency.STAMINA);
                if (total >= GameConfigData.Instance.MaxStamina)
                {
                    //fill max stamina
                    GameData.Instance.playerData.AddCurrency(Currency.STAMINA, GameConfigData.Instance.MaxStamina - GetNumber(Currency.STAMINA));
                    GameData.Instance.playerData.saveData.EndMineStamina();
                }
                else
                {
                    //not max stamina
                    GameData.Instance.playerData.AddCurrency(Currency.STAMINA, staminaMineNumber);
                    var timeChangeToStamina = staminaMineNumber * GameConfigData.Instance.TimeMineStaminaMinutes * 60;
                    DebugCustom.Log("Last TimeFinishMineStamina" + GameData.Instance.playerData.saveData.TimeFinishMineStamina);
                    GameData.Instance.playerData.saveData.TimeFinishMineStamina = GameData.Instance.playerData.saveData.TimeFinishMineStamina + timeChangeToStamina;
                    DebugCustom.Log("TimeFinishMineStamina" + GameData.Instance.playerData.saveData.TimeFinishMineStamina);
                }

            }

        }
    }
    public bool IsMineStamina()
    {
        if (TimeFinishMineStamina == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool CollectPatrol(bool isQuickEarnings)
    {
        if (isQuickEarnings)
        {
            AddCurrency(Currency.STAMINA, -15);
            QuickPatrolCount--;
            CreateRewardPatrol(true);
            EventDispatcher.Instance.PostEvent(EventID.USER_COLLECT_PATROL_REWARD);
            IsDirty = true;
            return true;
        }
        else
        {
            //first collect
            if (IsFirstPatrol())
            {
                FirstClickPatrol();
                ResetPatrolTimming();
                EventDispatcher.Instance.PostEvent(EventID.USER_COLLECT_PATROL_REWARD);
                IsDirty = true;
                return true;
            }
            else if (!AllowCollectPatrol())
            {
                DebugCustom.Log("Need 10 minutes");
                return false;
            }
            else
            {
                CreateRewardPatrol();
                ResetPatrolTimming();
                EventDispatcher.Instance.PostEvent(EventID.USER_COLLECT_PATROL_REWARD);
                IsDirty = true;
                return true;
            }
        }
    }
    public bool CanPatrol()
    {
        return QuickPatrolCount > 0 && (int)GetNumber(Currency.STAMINA) >= 15;
    }
    /// <summary>
    /// Return total minutes Afk
    /// </summary>
    /// <param name="isQuick"></param>
    /// <returns></returns>
    public int GetCurrentTimeReward(bool isQuick)
    {
        int timePatrolMinutes = 0;
        if (isQuick)
        {
            timePatrolMinutes = GameConfigData.Instance.PatrolQuickPatrolTimeMinutes;
        }
        else if (IsFirstPatrol())
        {
            timePatrolMinutes = GameConfigData.Instance.PatrolFirstRewardTimeMinutes;
        }
        else
        {
            var curUtcTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long timePatrolMilliseconds = curUtcTime - LastTimePatrolCollect;
            int maxTimePatrolMilliseconds = (int)GameConfigData.Instance.PatrolMaxAfkTimeMinutes.MinToMilliseconds();
            if (timePatrolMilliseconds > maxTimePatrolMilliseconds)
            {
                timePatrolMinutes = GameConfigData.Instance.PatrolMaxAfkTimeMinutes;
            }
            else
            {
                timePatrolMinutes = timePatrolMilliseconds.MillisecondsToMin();
            }
        }
        return timePatrolMinutes;
    }
    public Tuple<int,int,int,int> GetRewardPatrol(bool isQuick)
    {
        int timePatrolMinutes = 0;

        int gold = 0;
        int exp = 0;
        int numberEquipment = 0;
        int numberPieceDesign = 0;

        var dataChapter = GameData.Instance.staticData.GetChapterLevel(BestChapterLevel);
        timePatrolMinutes = GetCurrentTimeReward(isQuick);

        gold = dataChapter.afkEarningGoldPerMin * timePatrolMinutes;
        exp = dataChapter.afkEarningExpPerMin * timePatrolMinutes;
        numberEquipment = Mathf.RoundToInt(dataChapter.afkEarningItemPerMin * timePatrolMinutes);
        numberPieceDesign = Mathf.RoundToInt(dataChapter.afkEarningPieceUpgradePerMin * timePatrolMinutes);
        return new Tuple<int, int, int, int>(gold, exp, numberEquipment, numberPieceDesign);
    }
    private void FirstClickPatrol()
    {
        CreateRewardPatrol();
    }
    private bool IsFirstPatrol()
    {
        return LastTimePatrolCollect == -1;
    }
    private void CreateRewardPatrol(bool isQuick = false, Rarity rarity = Rarity.Common)
    {
        var rewardData = GetRewardPatrol(isQuick);
        int gold = rewardData.Item1;
        int exp = rewardData.Item2;
        int numberEquipment = rewardData.Item3;
        int numberPieceDesign = rewardData.Item4;
        GameData.Instance.playerData.AddCurrency(Currency.ACC_EXP, exp);
        GameData.Instance.playerData.AddCurrency(Currency.GOLD, gold);
        AddCurrency(Currency.RANDOM_DESIGN, numberPieceDesign);
        UserEquipment[] userEquips = new UserEquipment[numberEquipment];
        //add random equipments
        for (int i = 0; i < numberEquipment; i++)
        {
            EquipmentData eData = GameData.Instance.staticData.GetRandomEquipmentData();
            var userE = AddEquipmentToBag(eData.id, rarity, 1);
            userEquips[i] = userE;
        }
        SaveDataReward(userEquips, gold, exp);
    }
    private void SaveDataReward(UserEquipment[] userEquips, int gold, int exp)
    {
        Array.Clear(GameDynamicData.groupRewardPopupEquipData, 0, GameDynamicData.groupRewardPopupEquipData.Length);
        GameDynamicData.groupRewardPopupEquipData = new Tuple<Sprite, Sprite, string>[userEquips.Length + 2];
        int i = 0;

        var goldData = GameData.Instance.staticData.GetCurrencyData(Currency.GOLD);
        var goldRarityData = GameData.Instance.staticData.GetRarity(goldData.rarity);
        GameDynamicData.groupRewardPopupEquipData[i++] = new Tuple<Sprite, Sprite, string>(goldRarityData.border, goldData.icon, gold.ToShortStringK());

        var expData = GameData.Instance.staticData.GetCurrencyData(Currency.ACC_EXP);
        var expRarityData = GameData.Instance.staticData.GetRarity(expData.rarity);
        GameDynamicData.groupRewardPopupEquipData[i++] = new Tuple<Sprite, Sprite, string>(expRarityData.border, expData.icon, exp.ToShortStringK());

        foreach (var userE in userEquips)
        {
            var rarityData = GameData.Instance.staticData.GetRarity(userE.rarity);
            var equipData = GameData.Instance.staticData.GetEquipmentData(userE.itemId);
            GameDynamicData.groupRewardPopupEquipData[i++] = new Tuple<Sprite, Sprite, string>(rarityData.border, equipData.spriteIcons[(int)userE.rarity], userE.count.ToShortStringK());
        }
    }
    private void ResetPatrolTimming()
    {
        var utcTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        LastTimePatrolCollect = utcTime;

    }
    private void StartMineStamina()
    {
        if (IsMineStamina())
        {
            //DebugCustom.Log("Already minning");
            return;
        }
        var utcTime = DateTimeOffset.UtcNow.AddMinutes(GameConfigData.Instance.TimeMineStaminaMinutes).ToUnixTimeSeconds();
        TimeFinishMineStamina = utcTime;
    }
    private void EndMineStamina()
    {
        TimeFinishMineStamina = -1;
    }
    #endregion Patrol
    public void FinishChapter(int chapter)
    {
        if (chapter == BestChapterLevel)
        {
            //unlock new Chapter
            BestChapterLevel++;
            //select Chapter new
            GameDynamicData.SelectChapterLevel = BestChapterLevel;
            EventDispatcher.Instance.PostEvent(EventID.USER_UNLOCK_NEW_CHAPTER);
            DebugCustom.Log("Unlock new Chapter Level");
        }
        else
        {
            //no unlock new Chapter

        }
    }
    public int GetExpAccountLevelup(int curLevel = 1)
    {
        int baseAccExp = 10;
        return baseAccExp + Mathf.CeilToInt((curLevel - 1) * baseAccExp * 0.2f);
    }
    private void CollectAccountExp(int amountExp)
    {
        var totalExp = AccountExp + amountExp;
        var nextExp = GetExpAccountLevelup(AccountLevel);
        if (totalExp >= nextExp)
        {
            AccountLevel++;
            var remainExp = totalExp - nextExp;
            AccountExp = remainExp;
        }
        else
        {
            AccountExp += totalExp;
        }
        EventDispatcher.Instance.PostEvent(EventID.USER_COLLECT_ACCOUNT_EXP);
    }
    public void LoadSavedData()
    {
        LoadDataEquipped();
        LoadDataAcquired();
        LoadDataSurviveTime();
        LoadDataCurrency();
        QuickPatrolCount = PlayerPrefs.GetInt(PrefsKey.PLAYER_QUICK_PATROL_COUNT, 3);
        BestChapterLevel = PlayerPrefs.GetInt(PrefsKey.PLAYER_BEST_CHAPTER_LEVEL, GameConfigData.Instance.StartChapterLevel);
        AccountLevel = PlayerPrefs.GetInt(PrefsKey.PLAYER_ACCOUNT_LEVEL, GameConfigData.Instance.StartAccountLevel);
        AccountExp = PlayerPrefs.GetInt(PrefsKey.PLAYER_ACCOUNT_EXP, 0);
        CollectChapterChestProgress = PlayerPrefs.GetInt(PrefsKey.PLAYER_COLLECT_CHAPTER_CHEST, 0);
        LastTimePatrolCollect = long.Parse( PlayerPrefs.GetString(PrefsKey.PLAYER_LAST_TIME_PATROL, "-1"));
        TimeFinishMineStamina = long.Parse( PlayerPrefs.GetString(PrefsKey.PLAYER_TIME_MINE_STAMINA, "-1"));
        void LoadDataEquipped()
        {
            var jData = PlayerPrefs.GetString(PrefsKey.PLAYER_SLOTS, string.Empty);
            if (jData != string.Empty)
                ListEquipped = JsonConvert.DeserializeObject<List<UserEquipment>>(jData);
            else
                ListEquipped = new List<UserEquipment>();
        }

        void LoadDataAcquired()
        {
            var jDataBag = PlayerPrefs.GetString(PrefsKey.PLAYER_EQUIPMENT_BAG, string.Empty);
            if (jDataBag != string.Empty)
                ListAcquired = JsonConvert.DeserializeObject<List<UserEquipment>>(jDataBag);
            else
                ListAcquired = new List<UserEquipment>();
        }

        void LoadDataSurviveTime()
        {
            var jsonSurviveTime = PlayerPrefs.GetString(PrefsKey.PLAYER_SURVIVE_CHAPTER_TIME, string.Empty);
            if (jsonSurviveTime.Equals(string.Empty))
                CreateArraySurviveTime();
            else
                ChapterSurviveTime = JsonConvert.DeserializeObject<int[]>(jsonSurviveTime);
        }

        void LoadDataCurrency()
        {
            var jData3 = PlayerPrefs.GetString(PrefsKey.PLAYER_CURRENCY, string.Empty);
            if (jData3 != string.Empty)
                DictCurrency = JsonConvert.DeserializeObject<Dictionary<Currency, long>>(jData3);
            else
            {
                DictCurrency = new Dictionary<Currency, long>();
                AddCurrency(Currency.GOLD, GameConfigData.Instance.StartAccountGold);
                AddCurrency(Currency.GEM, GameConfigData.Instance.StartAccountGem);
                AddCurrency(Currency.STAMINA, GameConfigData.Instance.MaxStamina);
            }

        }
    }
    public async void SavePlayerData()
    {
        PlayerPrefs.SetInt(PrefsKey.PLAYER_QUICK_PATROL_COUNT, QuickPatrolCount);
        PlayerPrefs.SetInt(PrefsKey.PLAYER_BEST_CHAPTER_LEVEL, BestChapterLevel);
        PlayerPrefs.SetInt(PrefsKey.PLAYER_ACCOUNT_LEVEL, AccountLevel);
        PlayerPrefs.SetInt(PrefsKey.PLAYER_ACCOUNT_EXP, AccountExp);
        PlayerPrefs.SetInt(PrefsKey.PLAYER_COLLECT_CHAPTER_CHEST, CollectChapterChestProgress);
        PlayerPrefs.SetString(PrefsKey.PLAYER_LAST_TIME_PATROL, LastTimePatrolCollect.ToString());
        PlayerPrefs.SetString(PrefsKey.PLAYER_TIME_MINE_STAMINA, TimeFinishMineStamina.ToString());
        await Task.Delay(50);

        string jData = JsonConvert.SerializeObject(ListEquipped);
        PlayerPrefs.SetString(PrefsKey.PLAYER_SLOTS, jData);
        string jData2 = JsonConvert.SerializeObject(ListAcquired);
        PlayerPrefs.SetString(PrefsKey.PLAYER_EQUIPMENT_BAG, jData2);
        await Task.Delay(50);

        string jData4 = JsonConvert.SerializeObject(DictCurrency);
        PlayerPrefs.SetString(PrefsKey.PLAYER_CURRENCY, jData4);
        string jData5 = JsonConvert.SerializeObject(ChapterSurviveTime);
        PlayerPrefs.SetString(PrefsKey.PLAYER_SURVIVE_CHAPTER_TIME, jData5);
        IsDirty = false;
    }

    #region Bag 

    public UserEquipment RemoveEquipmentInBag(UserEquipment uEquipment)
    {
        if (ListAcquired.Contains(uEquipment))
        {
            ListAcquired.Remove(uEquipment);
            IsDirty = true;
            ItemHelper.IsInventoryChanged = true;
            return uEquipment;
        }
        return null;
    }
    public bool IsEquipped(UserEquipment userEquipment)
    {
        return ListEquipped.Contains(userEquipment);
    }

    public UserEquipment AddEquipmentToBag(string equipmentId, Rarity rarity, int level = 1)
    {
        EquipmentData equipmentData = GameData.Instance.staticData.GetEquipmentData(equipmentId) as EquipmentData;
        if (!equipmentData)
        {
            DebugCustom.LogError("Not available Equipment id :" + equipmentData.id);
            return null;
        }
        var newUserEquipment = new UserEquipment(equipmentData.id, rarity, level);
        ListAcquired.Add(newUserEquipment);
        ItemHelper.IsInventoryChanged = true;
        IsDirty = true;
        return newUserEquipment;
    }
    public UserItem AddItemToBag(UserItem userItem, bool isNewItem = true)
    {
        UserItem newUserItem;
        if (isNewItem)
        {
            newUserItem = userItem.Clone() as UserItem;
        }
        else
        {
            newUserItem = userItem;
        }
        
        var itemData = GameData.Instance.staticData.GetEquipmentData(newUserItem.itemId);
        if (!itemData)
        {
            DebugCustom.LogError("Not available Item id :" + newUserItem.itemId);
            return null;
        }
        if (newUserItem is UserEquipment uEquipment)
        {
            ListAcquired.Add(uEquipment);
        }

        ItemHelper.IsInventoryChanged = true;
        IsDirty = true;
        return newUserItem;
    }
    #endregion Bag

    #region Slot Equipment
    private bool CheckSlotEmpty(ItemType slot)
    {
        foreach (var equipped in ListEquipped)
        {
            var equippedData = GameData.Instance.staticData.GetEquipmentData(equipped.itemId);
            if (slot == equippedData.slot)
            {
                return false;
            }
        }
        return true;
    }
    public UserEquipment UnEquip(ItemType slot)
    {
        foreach (var equipped in ListEquipped)
        {
            var equippedData = GameData.Instance.staticData.GetEquipmentData(equipped.itemId);
            if (slot == equippedData.slot)
            {
                ListEquipped.Remove(equipped);
                AddItemToBag(equipped, false);
                IsDirty = true;
                GameSystem.Instance.equipmentSystem.CalculateEquipmentStats();
                return equipped;
            }
        }
        return null;
    }
    public UserEquipment UnEquip(UserEquipment userEquipment)
    {
        foreach (var equipped in ListEquipped)
        {
            if (equipped.Equals(userEquipment))
            {
                ListEquipped.Remove(equipped);
                AddItemToBag(userEquipment, false);
                IsDirty = true;
                GameSystem.Instance.equipmentSystem.CalculateEquipmentStats();
                return equipped;
            }
        }
        return null;
    }

    public void Equip(UserEquipment equipmentToEquip)
    {
        var equipmentStaticData = GameData.Instance.staticData.GetEquipmentData(equipmentToEquip.itemId);
        //check slot available and remove it
        if (!CheckSlotEmpty(equipmentStaticData.slot))
        {
            //remove item in Slot then add to bag
            UnEquip(equipmentStaticData.slot);
        }
        //add item to Slot then remove in Bag;
        ListEquipped.Add(equipmentToEquip);
        RemoveEquipmentInBag(equipmentToEquip);
        GameSystem.Instance.equipmentSystem.CalculateEquipmentStats();
        IsDirty = true;
    }
    public UserEquipment GetEquipInSlot(ItemType slot)
    {
        foreach (var equipped in ListEquipped)
        {
            var equippedData = GameData.Instance.staticData.GetEquipmentData(equipped.itemId);
            if (slot == equippedData.slot)
            {
                return equipped;
            }
        }
        return null;
    }
    #endregion Slot Equipment

    public bool SetRewardProgress(int id)
    {
        bool isChangeChapter = false;
        DebugCustom.Log("SetRewardProgress :" + id);
        CollectChapterChestProgress = id;
        var collectedChapter = CollectChapterChestProgress.ToRewardChapter();
        while (CheckNextChapterReward() &&
            collectedChapter < BestChapterLevel)
        {
            CollectChapterChestProgress = (collectedChapter + 1) * 100;
            //DebugCustom.Log("Up Chapter Reward :" + CollectChapterChestProgress);
            isChangeChapter = true;
        }
        IsDirty = false;
        return isChangeChapter;
        bool CheckNextChapterReward()
        {
            var chestProgress = GameData.Instance.playerData.saveData.CollectChapterChestProgress;
            var collectedChapter = chestProgress.ToRewardChapter();
            var chapterData = GameData.Instance.staticData.GetChapterLevel(collectedChapter);
            int maxProgress = 0;
            foreach (var chest in chapterData.chapterChests)
            {
                int progress = chest.idChest;
                if (progress > maxProgress)
                {
                    maxProgress = progress;
                }
            }
            return chestProgress >= maxProgress;
        }
    }
    public int GetBestTimeSurviveByChapter(int chapter)
    {
        return ChapterSurviveTime[chapter - 1];

    }
    public void SetBestTimeSurvive(int chapter, int time)
    {
        ChapterSurviveTime[chapter - 1] = time;
    }
    public long GetNumber(Currency currency)
    {
        long number;
        if (DictCurrency.TryGetValue(currency, out number))
        {
            return number;
        }
        else
        {
            DictCurrency.Add(currency, 0);
            return 0;
        }
    }
    /// <summary>
    /// excep only design random currency
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="value"></param>
    public void AddCurrency(Currency currency, long value)
    {

        if (currency == Currency.RANDOM_DESIGN)
        {
            List<CurrencyRewardItem> currencies = new List<CurrencyRewardItem>();
            for (int i = 0; i < value; i++)
            {
                var designCurrency = RandomDesign();
                int count = 1;
                currencies.Add(new CurrencyRewardItem() { currency = designCurrency, number = count });
                _AddCurrency(designCurrency, count);
            }
            GameDynamicData.rdDesignItems = currencies;
        }
        else if (currency == Currency.RANDOM_EQUIPMENT)
        {
            UserEquipment[] equips = new UserEquipment[value];
            for (int i = 0; i < value; i++)
            {
                var equipRoll = GameData.Instance.staticData.GetRandomEquipmentData();
                var uEquipment = AddEquipmentToBag(equipRoll.id, Rarity.Common, 1);
                equips[i] = uEquipment;
            }
            GameDynamicData.cachedUserEquipmentCollected = equips;
        }
        else if (currency == Currency.ACC_EXP)
        {
            CollectAccountExp((int)value);
        }
        else
        {
            _AddCurrency(currency, value);
        }
        IsDirty = true;
    }

    private Currency RandomDesign()
    {
        Currency armor = Currency.DESIGN_ARMOR;
        Currency belt = Currency.DESIGN_BELT;
        Currency boots = Currency.DESIGN_BOOTS;
        Currency gloves = Currency.DESIGN_GLOVES;
        Currency helmet = Currency.DESIGN_HELMET;
        Currency weapon = Currency.DESIGN_WEAPON;
        int rd = UnityEngine.Random.Range(0, 6);
        switch (rd)
        {
            case 0:
                return armor;
            case 1:
                return belt;
            case 2:
                return boots;
            case 3:
                return gloves;
            case 4:
                return helmet;
            default:
                return weapon;
        }
    }
    private void _AddCurrency(Currency currency, long value)
    {
        long number;
        if (DictCurrency.TryGetValue(currency, out number))
        {
            if (number + value >= 0)
            {
                DictCurrency[currency] = number + value;
            }
        }
        else
        {
            DictCurrency.Add(currency, value);
        }
        HandleChangeCurrency(currency);
        IsDirty = true;
        EventDispatcher.Instance.PostEvent(EventID.USER_ITEM_CHANGE, currency);

    }
    private void HandleChangeCurrency(Currency currency)
    {
        if (currency == Currency.STAMINA)
        {
            long number;
            if (DictCurrency.TryGetValue(currency, out number))
            {
                if (number < GameConfigData.Instance.MaxStamina)
                {
                    StartMineStamina();
                    EventDispatcher.Instance.PostEvent(EventID.USER_START_MINE_STAMINA);

                }
                else
                {
                    EndMineStamina();
                    EventDispatcher.Instance.PostEvent(EventID.USER_FINISH_MINE_STAMINA);
                }
            }
        }
    }
}
