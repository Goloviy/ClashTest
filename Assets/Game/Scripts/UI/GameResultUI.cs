using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultUI : PopupUI
{
    [SerializeField] GameObject[] winObjects;
    [SerializeField] GameObject[] loseObjects;
    [SerializeField] Button btnFinish;
    [SerializeField] Button btnRewardX2;
    [SerializeField] TextMeshProUGUI tmpResult;
    [SerializeField] TextMeshProUGUI tmpChapter;
    [SerializeField] TextMeshProUGUI tmpSurviveTime;
    [SerializeField] TextMeshProUGUI tmpBestSurviveTime;
    [SerializeField] TextMeshProUGUI tmpKillCount;
    const string STR_RESULT_WIN = "WIN";
    const string STR_RESULT_LOSE = "LOSE";
    [SerializeField] ItemRewardFinishGame prefabItem;
    [SerializeField] Transform tfParent;
    protected override void Awake()
    {
        base.Awake();
    }
    private void OnEnable()
    {
        btnFinish.onClick.AddListener(OnFinish);
        btnRewardX2.onClick.AddListener(OnRewardX3);
        UpdateUI();
        //ShowInter();
    }

    private void OnRewardX3()
    {
        OnRvSuccess(1);
    }

    private void OnRvFail()
    {
        
    }

    private void OnRvSuccess(int obj)
    {
        InGameManager.Instance.OnRvSuccess(3);
        btnRewardX2.gameObject.SetActive(false);
    }

    private void ShowInter()
    {
        //if (GameDynamicData.IsSurviveSuccess)
        //{
        //    AdManager.Instance.ShowInterstitialAd("FinishChapter");

        //}
        //else
        //{
        //    AdManager.Instance.ShowInterstitialAd("GameOver");

        //}
    }
    private void OnDisable()
    {
        btnFinish.onClick.RemoveListener(OnFinish);
    }
    private void UpdateUI()
    {
        foreach (var go in winObjects)
        {
            go.SetActive(GameDynamicData.IsSurviveSuccess);
        }
        foreach (var go in loseObjects)
        {
            go.SetActive(!GameDynamicData.IsSurviveSuccess);
        }
        btnRewardX2.gameObject.SetActive(GameDynamicData.IsSurviveSuccess);
        tmpResult.text = GameDynamicData.IsSurviveSuccess ? STR_RESULT_WIN : STR_RESULT_LOSE;
        tmpChapter.gameObject.SetActive(GameDynamicData.CurGameMode == GameMode.CAMPAIGN);
        tmpChapter.text = String.Concat("Chapter ", GameDynamicData.SelectChapterLevel);
        TimeSpan ts = TimeSpan.FromSeconds(InGameManager.Instance.TotalTimePlay);
        tmpSurviveTime.text = ts.ToString("mm\\:ss");
        int maxTime = Mathf.Max( Mathf.RoundToInt( InGameManager.Instance.TotalTimePlay), 
            GameData.Instance.playerData.saveData.GetBestTimeSurviveByChapter(GameDynamicData.SelectChapterLevel));
        TimeSpan ts2 = TimeSpan.FromSeconds(maxTime);
        tmpBestSurviveTime.text = ts2.ToString("mm\\:ss");
        tmpKillCount.text = GameDynamicData.KillCount.ToString("0,0");
        if (GameDynamicData.IsSurviveSuccess)
        {
            //init reward items
            var dataChapter = GameData.Instance.staticData.GetChapterLevel(GameDynamicData.SelectChapterLevel);
            foreach (var itemData in dataChapter.rewards)
            {
                var newItem = Instantiate(prefabItem, tfParent);
                var currencyData = GameData.Instance.staticData.GetCurrencyData(itemData.currency);
                var rarityData = GameData.Instance.staticData.GetRarity(currencyData.rarity);
                newItem.Init(rarityData.border, currencyData.icon, itemData.number);
            }
        }

    }
    private void OnFinish()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        Time.timeScale = 1;
        GameData.Instance.playerData.saveData.SavePlayerData();
        SceneManager.LoadScene("Home");
    }
    
}
