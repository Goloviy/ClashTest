using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemChapterChest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpTitle;
    [SerializeField] TextMeshProUGUI tmpDescriptions;
    [SerializeField] TextMeshProUGUI tmpRewardCount;
    [SerializeField] Image imgRewardIcon;
    [SerializeField] Image bgIcon;
    [SerializeField] Button btnClaim;
    [SerializeField] GameObject goComplete;
    const string TITLE = "Chapter ";
    bool isClickClaim = false;
    ChapterChest chapterChest;
    Action OnNextChapter;
    public void Init(int chapterLevel, ChapterChest chapterChest, Action OnNextChapter)
    {
        this.OnNextChapter = OnNextChapter;
        this.chapterChest = chapterChest;
        btnClaim.onClick.AddListener(OnClickClaim);
        btnClaim.gameObject.SetActive(false);
        tmpTitle.text = string.Concat(TITLE, chapterLevel);
        tmpDescriptions.text = chapterChest.description;
        goComplete.SetActive(false);
        var rewardData = GameData.Instance.staticData.GetCurrencyData(chapterChest.currencyReward.currency);
        if (rewardData)
        {
            imgRewardIcon.overrideSprite = rewardData.icon;
            tmpRewardCount.text = String.Concat("x", chapterChest.currencyReward.number.ToShortString());
            var rarityData = GameData.Instance.staticData.GetRarity(rewardData.rarity);
            bgIcon.overrideSprite = rarityData.border;
        }
        var curProgress = GameData.Instance.playerData.saveData.CollectChapterChestProgress;
        var chapterOfChest = chapterChest.idChest.ToRewardChapter();
        int timeSurvive = GameData.Instance.playerData.saveData.GetBestTimeSurviveByChapter(chapterOfChest);
        bool isCollected = curProgress >= chapterChest.idChest;
        if (timeSurvive >= chapterChest.timeSurvive /*|| isClearAll*/)
        {
            if (isCollected /*|| isClearAll*/)
            {
                goComplete.SetActive(true);
                btnClaim.gameObject.SetActive(false);
            }
            else
            {
                goComplete.SetActive(false);
                btnClaim.gameObject.SetActive(true);

            }
        }
    }

    private void OnClickClaim()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (!isClickClaim)
        {
            GameData.Instance.playerData.AddCurrency(
                chapterChest.currencyReward.currency, 
                chapterChest.currencyReward.number
                );
            isClickClaim = true;
            goComplete.SetActive(true);
            btnClaim.gameObject.SetActive(false);
            bool isNextChapter = GameData.Instance.playerData.saveData.SetRewardProgress(chapterChest.idChest);
            if (isNextChapter)
            {
                OnNextChapter?.Invoke();
            }
        }
    }
}
