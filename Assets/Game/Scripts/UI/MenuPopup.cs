using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPopup : MonoBehaviour
{
    [SerializeField] Button btnShop;
    [SerializeField] Button btnBag;
    [SerializeField] Button btnPlayGame;
    [SerializeField] Button btnChallenges;
    [SerializeField] Button btnTalen;

    [SerializeField] Button btnBuyStamina;
    [SerializeField] Button btnBuyGem;
    [SerializeField] Button btnBuyGold;

    [SerializeField] Image imgExpFill;

    [SerializeField] TextMeshProUGUI tmpPlayerLevel;
    [SerializeField] TextMeshProUGUI tmpPlayerName;
    [SerializeField] TextMeshProUGUI tmpStamina;
    [SerializeField] TextMeshProUGUI tmpGem;
    [SerializeField] TextMeshProUGUI tmpGold;
    [SerializeField] TextMeshProUGUI tmpCdTimerRegenStamina;

    [SerializeField] GameObject goFooter;
    [SerializeField] GameObject goFooterBG;
    private void Awake()
    {
        tmpCdTimerRegenStamina.gameObject.SetActive(false);
        btnShop.onClick.AddListener(OnClickShop);
        btnBag.onClick.AddListener(OnClickBag);
        btnPlayGame.onClick.AddListener(OnClickCampaign);
        btnChallenges.onClick.AddListener(OnClickChallenge);
        btnTalen.onClick.AddListener(OnClickTalent);

        btnBuyGem.onClick.AddListener(OnClickGem);
        btnBuyStamina.onClick.AddListener(OnClickStamina);
        btnBuyGold.onClick.AddListener(OnClickGold);

    }
    
    private void OnClickTalent()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        AlertPanel.Instance.ShowNotice("Comming Soon", null);
    }

    private void OnClickChallenge()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        AlertPanel.Instance.ShowNotice("Comming Soon", null);
    }

    private void OnClickGold()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        UIManagerHome.Instance.Open(PopupType.SHOP);
    }

    private void OnClickStamina()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        UIManagerHome.Instance.Open(PopupType.BUY_STAMINA, true);
       
    }

    private void OnClickGem()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        UIManagerHome.Instance.Open(PopupType.SHOP);
        
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.USER_ITEM_CHANGE, ItemChange);
        EventDispatcher.Instance.RegisterListener(EventID.USER_COLLECT_ACCOUNT_EXP, CollectAccountExp);
        EventDispatcher.Instance.RegisterListener(EventID.USER_OPEN_CHAPTER_CHEST, OnOpenChapterChest);
        EventDispatcher.Instance.RegisterListener(EventID.TIME_TICK_SECONDS, OnTickSeconds);
        EventDispatcher.Instance.RegisterListener(EventID.USER_START_MINE_STAMINA, StartMineStamina);
        EventDispatcher.Instance.RegisterListener(EventID.USER_FINISH_MINE_STAMINA, FinishMineStamina);
    }

    private void CollectAccountExp(Component arg1, object arg2)
    {
        UpdateUIPanelAccountLevel();
    }
    private void UpdateUIPanelAccountLevel()
    {
        tmpPlayerLevel.text = GameData.Instance.playerData.saveData.AccountLevel.ToString();
        var expLevelup = GameData.Instance.playerData.saveData.GetExpAccountLevelup(GameData.Instance.playerData.saveData.AccountLevel);
        float fill = Mathf.Clamp01(GameData.Instance.playerData.saveData.AccountExp / expLevelup);
        imgExpFill.fillAmount = fill;
    }
    private void StartMineStamina(Component arg1, object arg2)
    {
        tmpCdTimerRegenStamina.gameObject.SetActive(true);

    }
    private void FinishMineStamina(Component arg1, object arg2)
    {
        tmpCdTimerRegenStamina.gameObject.SetActive(false);
    }
    private void OnTickSeconds(Component arg1, object arg2)
    {

        OnUpdateMinningStamina();
    }

    void OnUpdateMinningStamina()
    {
        if (GameData.Instance.playerData.saveData.IsMineStamina())
        {
            GameData.Instance.playerData.saveData.CalculateMineStamina();
            tmpCdTimerRegenStamina.gameObject.SetActive(true);
            tmpCdTimerRegenStamina.text = DateTimeOffset.FromUnixTimeSeconds(GameData.Instance.playerData.saveData.TimeFinishMineStamina - DateTimeOffset.UtcNow.ToUnixTimeSeconds()).ToString("mm:ss");


        }
        else
        {
            tmpCdTimerRegenStamina.gameObject.SetActive(false);

        }
    }
    private void OnOpenChapterChest(Component arg1, object arg2)
    {
        bool isActiveFooter = (bool)arg2;
        goFooter.SetActive(isActiveFooter);
        goFooterBG.SetActive(isActiveFooter);
    }

    private void ItemChange(Component arg1, object arg2)
    {
        if (!ReferenceEquals(tmpGem, null))
        {

            tmpGem.text = GameData.Instance.playerData.GetCurrencyValue(Currency.GEM).ToShortString();
            tmpGold.text = GameData.Instance.playerData.GetCurrencyValue(Currency.GOLD).ToShortStringK();

            StringBuilder builder = new StringBuilder();
            builder.Append(GameData.Instance.playerData.GetCurrencyValue(Currency.STAMINA));
            builder.Append("/");
            builder.Append(GameConfigData.Instance.MaxStamina);
            tmpStamina.text = builder.ToString();
        }
        Currency currency = (Currency)arg2;


    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.USER_ITEM_CHANGE, ItemChange);
        EventDispatcher.Instance.RemoveListener(EventID.USER_COLLECT_ACCOUNT_EXP, CollectAccountExp);
        EventDispatcher.Instance.RemoveListener(EventID.USER_OPEN_CHAPTER_CHEST, OnOpenChapterChest);
        EventDispatcher.Instance.RemoveListener(EventID.TIME_TICK_SECONDS, OnTickSeconds);
        EventDispatcher.Instance.RemoveListener(EventID.USER_START_MINE_STAMINA, StartMineStamina);
        EventDispatcher.Instance.RemoveListener(EventID.USER_FINISH_MINE_STAMINA, FinishMineStamina);

    }
    //private void StaminaChange(Component arg1, object arg2)
    //{


    //}

    private void Start()
    {
        UIManagerHome.Instance.Open(PopupType.CAMPAIGN);
        tmpPlayerLevel.text = GameData.Instance.playerData.saveData.AccountLevel.ToString();
        UpdateUIPanelAccountLevel();
        UpdateCurrency();
        void UpdateCurrency()
        {
            tmpGem.text = GameData.Instance.playerData.GetCurrencyValue(Currency.GEM).ToShortString();
            tmpGold.text = GameData.Instance.playerData.GetCurrencyValue(Currency.GOLD).ToShortStringK();
            StringBuilder builder = new StringBuilder();
            builder.Append(GameData.Instance.playerData.GetCurrencyValue(Currency.STAMINA));
            builder.Append("/");
            builder.Append(GameConfigData.Instance.MaxStamina);
            tmpStamina.text = builder.ToString();
        }
    }
    private void OnClickCampaign()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        UIManagerHome.Instance.Open(PopupType.CAMPAIGN);
    }

    private void OnClickBag()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        UIManagerHome.Instance.Open(PopupType.BAG);
    }

    private void OnClickShop()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        UIManagerHome.Instance.Open(PopupType.SHOP);
    }
}
