using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDynamicData
{
    public static bool IsAvailableRevive = true;
    public static bool PlayerPlayGame = false;
    public static MainCharacter mainCharacter;
    //public static AttackDataProcessed curAttackDataProcessed;
    public static int curUnitInflictDamage;
    public static Dictionary<int, string> dictStringDamage = new Dictionary<int, string>();
    //public static string curAttackDataProcessedDamage;
    public static int healAmount;
    public static UserEquipment SelectedEquipment;
    public static bool SelectedGearCanEquip;
    public static UserItem SelectedItem;

    public static int hpMonster = 1;
    public static int atkMonster = 1;
    public static float mSpeedMonster = 1f;

    public static BirdCage BirdCage { get; internal set; }
    //public static bool AvailableRevive = true;
    public static int GoldReceived = 0;
    public static int KillCount = 0;

    public static GameMode CurGameMode = GameMode.CAMPAIGN;
    public static bool IsSurviveSuccess = false;

    public static UserEquipment[] cachedUserEquipmentCollected;
    public static GachaType curGachaType = GachaType.COMMON;
    public static List<CurrencyRewardItem> rdDesignItems;
    public static List<CurrencyRewardItem> rdEquipments;

    public static int SelectChapterLevel = GameConfigData.Instance.StartChapterLevel;

    /// <summary>
    /// Sprite sprBorder, Sprite sprIcon, string strCount
    /// </summary>
    public static Tuple<Sprite,Sprite,string>[] groupRewardPopupEquipData = new Tuple<Sprite, Sprite, string>[0];
    
}
