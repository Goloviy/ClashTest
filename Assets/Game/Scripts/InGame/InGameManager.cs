using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameManager : Singleton<InGameManager>
{
    float startTime;
    ChapterLevelData chapterLevelData;
    public float TotalTimePlay { get; private set; } = 0f;
    bool isEndGame = false;
    bool isCharacterDie = false;
    [SerializeField] GameObject prefabFlash;
    private void Awake()
    {
        ApplySetting();
        startTime = Time.time;
        chapterLevelData = GameData.Instance.staticData.GetChapterLevel(GameData.Instance.playerData.saveData.BestChapterLevel);
        GameDynamicData.GoldReceived = 0;
        GameDynamicData.KillCount = 0;
        GameDynamicData.dictStringDamage = new Dictionary<int, string>();
    }
    private void OnEnable()
    {
        GameDynamicData.curUnitInflictDamage = -1;
        GameDynamicData.dictStringDamage = new Dictionary<int, string>();
        GameDynamicData.PlayerPlayGame = true;
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_DIE, OnCharacterDie);
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_REVIVE, OnCharacterRevive);
        EventDispatcher.Instance.RegisterListener(EventID.CAMPAIGN_FINISH, OnCampaignFinish);
        EventDispatcher.Instance.RegisterListener(EventID.GAME_OVER, OnGameOver);
        //EventDispatcher.Instance.RegisterListener(EventID.USER_UNLOCK_NEW_CHAPTER, OnUnlockNewChapter);
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_TAKE_BOMB, OnCharacterTakeBomb);
    }
    private void OnDisable()
    {
        GameDynamicData.PlayerPlayGame = false;
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_DIE, OnCharacterDie);
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_REVIVE, OnCharacterRevive);
        EventDispatcher.Instance.RemoveListener(EventID.CAMPAIGN_FINISH, OnCampaignFinish);
        EventDispatcher.Instance.RemoveListener(EventID.GAME_OVER, OnGameOver);
        //EventDispatcher.Instance.RemoveListener(EventID.USER_UNLOCK_NEW_CHAPTER, OnUnlockNewChapter);
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_TAKE_BOMB, OnCharacterTakeBomb);
    }
    private void OnCharacterTakeBomb(Component arg1, object arg2)
    {
        Instantiate(prefabFlash, GameDynamicData.mainCharacter.transform.position, Quaternion.identity) ;
    }

    //private void OnUnlockNewChapter(Component arg1, object arg2)
    //{
    //    if (GameDynamicData.SelectChapterLevel == GameData.Instance.playerData.saveData.BestChapterLevel - 1)
    //    {
    //        GameDynamicData.SelectChapterLevel = GameData.Instance.playerData.saveData.BestChapterLevel - 1;
    //    }
    //}

    private void OnGameOver(Component arg1, object arg2)
    {
        //check Best time
        int curTimePlay = Mathf.RoundToInt( TotalTimePlay);
        int lastBestTimePlay = GameData.Instance.playerData.saveData.GetBestTimeSurviveByChapter(GameDynamicData.SelectChapterLevel);
        bool isNewBestTime = curTimePlay > lastBestTimePlay;
        if (isNewBestTime)
        {
            GameData.Instance.playerData.saveData.SetBestTimeSurvive(GameDynamicData.SelectChapterLevel, curTimePlay);
            EventDispatcher.Instance.PostEvent(EventID.USER_NEW_BEST_TIME_CHAPTER);
        }
        //collect basic reward chapter
        CollectReward(GameMode.CAMPAIGN, false, GameDynamicData.SelectChapterLevel) ;
    }
    private void CollectReward(GameMode gameMode, bool isWin, int finishChapter)
    {
        if (gameMode == GameMode.CAMPAIGN)
        {
            //bool isFinishNewChapter = finishChapter >= GameData.Instance.playerData.saveData.BestChapterLevel;
            if (isWin)
            {
                var curChapterData = GameData.Instance.staticData.GetChapterLevel(finishChapter);
                foreach (var rewardItem in curChapterData.rewards)
                {
                    GameData.Instance.playerData.AddCurrency(rewardItem.currency, rewardItem.number);
                }
            }
            GameData.Instance.playerData.saveData.SavePlayerData();
        }
    }
    public void OnRvSuccess(int multiply = 3)
    {
        for (int i = 0; i < multiply - 1; i++)
        {
            CollectReward(GameMode.CAMPAIGN, true, GameDynamicData.SelectChapterLevel);
        }
    }
    private void OnCampaignFinish(Component arg1, object arg2)
    {
        isEndGame = true;
        var curChapterData = GameData.Instance.staticData.GetChapterLevel(GameDynamicData.SelectChapterLevel);
        GameData.Instance.playerData.saveData.SetBestTimeSurvive(GameDynamicData.SelectChapterLevel, curChapterData.timePlay);
        GameData.Instance.playerData.saveData.FinishChapter(GameDynamicData.SelectChapterLevel);
        CollectReward(GameMode.CAMPAIGN, true, GameDynamicData.SelectChapterLevel);
    }

    private void OnCharacterRevive(Component arg1, object arg2)
    {
        isCharacterDie = false;
    }

    private void OnCharacterDie(Component arg1, object arg2)
    {
        isCharacterDie = true;
    }

    private void ApplySetting()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        TickTimer();
    }
    /// <summary>
    /// Pause counter time when boss appearence
    /// </summary>
    private void TickTimer()
    {
        if (!SpawnerMananger.Instance.IsCombatBoss || isEndGame || isCharacterDie)
        {
            TotalTimePlay += Time.deltaTime;
        }
    }

}
