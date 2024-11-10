using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignPopupUI : PopupUI
{
    [SerializeField] GameObject goAlertPatrol;
    [SerializeField] GameObject goAlertChapterChest;

    [Title("Chapter Info")]
    [SerializeField] TextMeshProUGUI tmpTitle;
    [SerializeField] TextMeshProUGUI tmpTimeSurvivalMax;
    [SerializeField] Image imgChapterIcon;

    [Title("Chapter Chests")]
    [SerializeField] Button btnStartChapter;
    [SerializeField] Button btnChapterChest;
    [SerializeField] Button btnCloseChapterChest;
    [SerializeField] Button btnOpenSelectChapter;
    [SerializeField] Button btnPatrol;

    [Title("Panels")]
    [SerializeField] Transform tfMain;
    [SerializeField] Transform tfChapterChest;
    protected override void Awake()
    {
        base.Awake();
        btnStartChapter.onClick.AddListener(OnClickStartChapter);
        btnChapterChest.onClick.AddListener(OnClickChapterChest);
        btnCloseChapterChest.onClick.AddListener(OnClickCloseChapterChest);
        btnOpenSelectChapter.onClick.AddListener(OnClickSelectChapter);
        btnPatrol.onClick.AddListener(OnClickPatrol);
        //btnPatrol.enabled = GameData.Instance.playerData.saveData.IsActivePatrol();
    }
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.TIME_TICK_SECONDS, OnSecondTick);
    }

    private void OnSecondTick(Component arg1, object arg2)
    {
        UpdatePatrol();
        UpdateChapterChest();
    }

    private void UpdateChapterChest()
    {
        var chapterLevelData = GameSystem.Instance.GetChapterDataByProgressCollectReward();
        foreach (var chapterChest in chapterLevelData.chapterChests)
        {
            var curProgress = GameData.Instance.playerData.saveData.CollectChapterChestProgress;
            var chapterOfChest = chapterChest.idChest.ToRewardChapter();
            int timeSurvive = GameData.Instance.playerData.saveData.GetBestTimeSurviveByChapter(chapterOfChest);
            bool isCollected = curProgress >= chapterChest.idChest;
            if (timeSurvive >= chapterChest.timeSurvive && !isCollected)
            {
                goAlertChapterChest.SetActive(true);
            }
            else
            {
                goAlertChapterChest.SetActive(false);
            }
        }

    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.TIME_TICK_SECONDS, OnSecondTick);
        
    }
    void UpdatePatrol()
    {
        goAlertPatrol.SetActive(GameData.Instance.playerData.saveData.IsPatrolFull());
    }
    private void OnClickPatrol()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (GameData.Instance.playerData.saveData.IsActivePatrol())
        {
            UIManagerHome.Instance.Open(PopupType.PATROL, true);

        }
        else
        {
            //string text = I2.Loc.LocalizationManager.GetTranslation(DictString.REQUIRE_LEVEL);
            AlertPanel.Instance.ShowNotice(String.Concat(DictString.REQUIRE_LEVEL + (GameConfigData.Instance.PatrolStartLevel - 1)), null);
        }
    }

    private void OnClickSelectChapter()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        UIManagerHome.Instance.Open(PopupType.SELECT_CHAPTER);
    }

    private void OnClickChapterChest()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        tfMain.gameObject.SetActive(false);
        tfChapterChest.gameObject.SetActive(true);
        EventDispatcher.Instance.PostEvent(EventID.USER_OPEN_CHAPTER_CHEST, false);
    }
    private void OnClickCloseChapterChest()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        tfMain.gameObject.SetActive(true);
        tfChapterChest.gameObject.SetActive(false);
        EventDispatcher.Instance.PostEvent(EventID.USER_OPEN_CHAPTER_CHEST, true);
    }
    public override void Open()
    {
        base.Open();
        var ChapterData = GameData.Instance.staticData.GetChapterLevel(GameDynamicData.SelectChapterLevel);
        //string chapterTitle = I2.Loc.LocalizationManager.GetTranslation(ChapterData.titKey);
        tmpTitle.text = ChapterData.title;
        StringBuilder builder = new StringBuilder();
        builder.Append(StringConst.LONGEST_SURVIVED);
        builder.Append("0");
        builder.Append(StringConst.SECONDS);
        tmpTimeSurvivalMax.text = builder.ToString();
        imgChapterIcon.overrideSprite = ChapterData.icon;
        tfMain.gameObject.SetActive(true);
        tfChapterChest.gameObject.SetActive(false);
    }

    private void OnClickStartChapter()
    {
        int level = GameDynamicData.SelectChapterLevel;
        GameSystem.Instance.StartPlayMode(GameMode.CAMPAIGN, level);
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

    }
}
