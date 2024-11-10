using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatrolPopupUI : PopupUI
{
    [SerializeField] TextMeshProUGUI tmpQuickPatrolCount;
    [SerializeField] TextMeshProUGUI tmpGoldMine;
    [SerializeField] TextMeshProUGUI tmpExpMine;
    [SerializeField] TextMeshProUGUI tmpMaxAfk;
    [SerializeField] TextMeshProUGUI tmpTotalAfk;
    [SerializeField] ItemRewardFinishGame prefabItem;
    [SerializeField] Transform parent;
    [SerializeField] Button btnClaim;
    [SerializeField] Button btnQuickEarnings;
    [SerializeField] Button btnClose;

    ChapterLevelData levelData;
    GameObject[] items;
    protected override void Awake()
    {
        base.Awake();
        btnClaim.onClick.AddListener( OnClickClaim);
        btnClose.onClick.AddListener( OnClickClose);
        btnQuickEarnings.onClick.AddListener( OnClickQuickEarnings);
        if (parent.childCount > 0)
            parent.GetChild(0).gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.USER_COLLECT_PATROL_REWARD, OnCollectPatrolSuccess);
    }

    private void OnCollectPatrolSuccess(Component arg1, object arg2)
    {
        tmpQuickPatrolCount.text = GameData.Instance.playerData.saveData.QuickPatrolCount.ToString();
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.USER_COLLECT_PATROL_REWARD, OnCollectPatrolSuccess);

    }
    private void OnClickClose()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        UIManagerHome.Instance.Open(PopupType.CAMPAIGN);
    }

    private void OnClickQuickEarnings()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (GameData.Instance.playerData.saveData.CanPatrol())
        {
            QuickEarningSuccess();
        }
        else
        {
            //var txt = I2.Loc.LocalizationManager.GetTranslation(DictString.NOT_ENOUGH_STAMINA_TO_QUICK_PATROL);
            AlertPanel.Instance.ShowNotice(DictString.NOT_ENOUGH_STAMINA_TO_QUICK_PATROL, null);
        }
        //Close();
    }
    private void QuickEarningSuccess()
    {
        GameData.Instance.playerData.saveData.CollectPatrol(true);
        UIManagerHome.Instance.Open(PopupType.GROUP_ITEM_REWARD);
    }
    private void OnClickClaim()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (GameData.Instance.playerData.saveData.AllowCollectPatrol())
        {
            GameData.Instance.playerData.saveData.CollectPatrol(false);
            UIManagerHome.Instance.Open(PopupType.GROUP_ITEM_REWARD);
        }
        else
        {
            DebugCustom.Log("Not enough requires patrol");
        }
    }

    public override void Close()
    {
        ClearItem();
        base.Close();
    }
    private void ClearItem()
    {
        foreach (var item in items)
        {
            GameObject.Destroy(item);
        }
        Array.Clear(items, 0, items.Length);
    }
    public override void Open()
    {
        base.Open();
        btnClaim.enabled = GameData.Instance.playerData.saveData.AllowCollectPatrol();
        levelData = GameData.Instance.staticData.GetChapterLevel(GameData.Instance.playerData.saveData.BestChapterLevel);
        tmpQuickPatrolCount.text = GameData.Instance.playerData.saveData.QuickPatrolCount.ToString();
        LoadCurrentRewards();
        InitUIStatic();
    }
    private void LoadCurrentRewards()
    {
        items = new GameObject[4];
        var rewardData = GameData.Instance.playerData.saveData.GetRewardPatrol(false);
        int gold = rewardData.Item1;
        int exp = rewardData.Item2;
        int numberEquipment = rewardData.Item3;
        int numberPieceDesign = rewardData.Item4;

        CreateItem(Currency.GOLD, gold, 0);
        CreateItem(Currency.ACC_EXP, exp, 1);
        CreateItem(Currency.RANDOM_EQUIPMENT, numberEquipment, 2);
        CreateItem(Currency.RANDOM_DESIGN, numberPieceDesign, 3);

        void CreateItem(Currency currency, int value, int slot)
        {
            var currencyData = GameData.Instance.staticData.GetCurrencyData(currency);
            var rarityData = GameData.Instance.staticData.GetRarity(currencyData.rarity);
            var newItem = Instantiate(prefabItem, parent);
            newItem.Init(rarityData.border, currencyData.icon, value);
            items[slot] = newItem.gameObject;
        }
    }

    private void InitUIStatic()
    {
        tmpGoldMine.text = string.Concat(levelData.afkEarningGoldPerMin * 60, "/h");
        tmpExpMine.text = string.Concat(levelData.afkEarningExpPerMin * 60, "/h");
        tmpMaxAfk.text = string.Concat("Max ", GameConfigData.Instance.PatrolMaxAfkTimeMinutes / 60, "h AFK");
        tmpTotalAfk.text = GameData.Instance.playerData.saveData.GetCurrentTimeReward(false).MinToHoursString();


    }
}
