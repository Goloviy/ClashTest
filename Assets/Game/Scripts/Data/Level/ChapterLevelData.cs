using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ChapterLevelData", menuName = "ScriptableObjects/ChapterLevelData", order = 1)]
public class ChapterLevelData : BaseScriptObject
{
    public Sprite icon;
    public Sprite bg;
    public int level = 1;
    /// <summary>
    /// seconds
    /// </summary>
    [Tooltip("Total time play chapter . (Seconds)")]
    public int timePlay = 900;
    /// <summary>
    /// Load from localize Drive = "tit.(level)"
    /// </summary>
    public string title = "ChapterData/tit.";
    /// <summary>
    /// Load from localize Drive = "des.(level)"
    /// </summary>
    public string des = "ChapterData/des.";
    public int afkEarningGoldPerMin = 100;
    public int afkEarningExpPerMin = 30;
    public float afkEarningPieceUpgradePerMin = 0.01f;
    public float afkEarningItemPerMin = 0.0022f;
    //public int finishGoldReward = 2500;
    //public int finishExpReward = 500;
    //public int finishDesignReward = 10;

    [Tooltip("alway reward when clear map")]
    public CurrencyRewardItem[] rewards;
    ///// <summary>
    ///// max time afk by minute
    ///// </summary>
    //public int timeAfkMax = 960;
    [Tooltip("Claim when achive target")]

    public ChapterChest[] chapterChests;
    [InlineEditor] public WaveMonsterData[] waves;
    [InlineEditor] public MiniBossContainer[] miniBoss;
    [InlineEditor] public BossContainer[] listBoss;
}
[System.Serializable]
public class ChapterChest
{
    [Tooltip("Split id incude : IdChapter - IndexReward (etc : 101, 201, 401..)")]
    public int idChest = 101;
    public string description = "Survive for more than 5 min";
    public float timeSurvive = 120f;
    public CurrencyRewardItem currencyReward;
}
