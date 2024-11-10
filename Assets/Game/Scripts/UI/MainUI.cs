using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public interface IMananagerUI
{
    void Open(PopupType type, bool isOverride = false);
    void Back();
}
public class MainUI : MonoBehaviour, IMananagerUI
{
    float refreshTime = 0;
    ICanUpLevel character;
    float curFillAmout;
    [Header("References Components")]
    [SerializeField] TextMeshProUGUI tmpTimer;
    [SerializeField] Button btnPause;
    [SerializeField] Button btnTest;
    [SerializeField] RectTransform fillerExp;
    [SerializeField] RectTransform fillerBossHp;
    [SerializeField] Image barExp;
    [SerializeField] Image barBossHp;
    [SerializeField] TextMeshProUGUI tmpCountMonster;
    [SerializeField] TextMeshProUGUI tmpGoldCollected;
    [SerializeField] TextMeshProUGUI tmpBossName;
    [SerializeField] TextMeshProUGUI tmpLevel;
    [SerializeField] TextMeshProUGUI tmpWarning;
    [SerializeField] GameObject goWarning;
    [SerializeField] Image imgWarning;
    [Header("References Popup")]
    [SerializeField] GameObject goBlackPanel;
    [SerializeField] PopupLearnSkill popupLearnSkill;
    //[SerializeField] GachaPopupUI popupGacha;
    float startTime;
    Dictionary<PopupType, PopupUI> dicts;
    PopupType curPopup;
    PopupType lastPopup = PopupType.NONE;
    PopupType defaulPopup = PopupType.NONE;
    string STR_WARNING_BOSS_ASSAULT = "BOSS ASSAULT";
    string STR_WARNING_SUDDEN_ATTACK = "SUDDEN ATTACK";
    string STR_LEVEL = "Lv.";
    private void Awake()
    {
        startTime = Time.time;
        barExp.fillAmount = curFillAmout;
        dicts = new Dictionary<PopupType, PopupUI>();
        List<PopupUI> popups = this.transform.parent.GetComponentsInChildren<PopupUI>(true).ToList();
        foreach (var popup in popups)
        {
            popup.Init(this);
            dicts.Add(popup.popupType, popup);
        }
        btnPause.onClick.AddListener(Pause);
        btnTest.onClick.AddListener(OnClickTest);
    }
    private void Start()
    {
        InitMainUI();
    }
    private void InitMainUI()
    {
        UpdateStateBar(false);
        tmpGoldCollected.text = GameDynamicData.GoldReceived.ToShortString();
        tmpCountMonster.text = GameDynamicData.KillCount.ToShortString();
    }
    private void OnClickTest()
    {
        Open(PopupType.GACHA_UI);
    }

    private void Pause()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        Time.timeScale = 0;
        Open(PopupType.PAUSE);
    }
    public void Open(PopupType type, bool isOverride = false)
    {
        lastPopup = curPopup;
        curPopup = type;
        PopupUI popupUI;
        if (dicts.TryGetValue(curPopup, out popupUI))
            popupUI.Open();
        if (curPopup != lastPopup)
        {
            if (dicts.TryGetValue(lastPopup, out popupUI) && !isOverride)
                popupUI.Close();
        }

    }
    public void Back()
    {
        if (lastPopup == PopupType.NONE)
        {
            return;
        }
        var temp = lastPopup;
        lastPopup = curPopup;
        curPopup = temp;
        PopupUI popupUI;
        if (dicts.TryGetValue(curPopup, out popupUI))
            popupUI.Open();
        if (dicts.TryGetValue(lastPopup, out popupUI))
            popupUI.Close();
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_TAKE_EXP, UpdateSliderExp);
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_LEVELUP, OnLevelUp);
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_TAKE_GACHA, OnTakeGacha);
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_TAKE_GACHA_RV, OnTakeGachaRV);
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_DIE, OnCharacterDie);
        EventDispatcher.Instance.RegisterListener(EventID.BOSS_DIE, OnBossDie);
        EventDispatcher.Instance.RegisterListener(EventID.BOSS_APPEAR, OnBossAppear);
        EventDispatcher.Instance.RegisterListener(EventID.BOSS_TAKE_DAMAGE, OnBossTakeDamage);
        EventDispatcher.Instance.RegisterListener(EventID.MONSTER_DIE, OnMonsterDie);
        EventDispatcher.Instance.RegisterListener(EventID.CHARACTER_TAKE_GOLD, OnTakeGold);
        EventDispatcher.Instance.RegisterListener(EventID.CAMPAIGN_FINISH, OnCampaignFinish);
        EventDispatcher.Instance.RegisterListener(EventID.WARNING_BOSS_ASSAULT, OnWarningBoss);
        EventDispatcher.Instance.RegisterListener(EventID.WARNING_SUDDEN_ATTACK, OnWarningSuddenAttack);
    }

    private async void OnWarningSuddenAttack(Component arg1, object arg2)
    {
        goWarning.SetActive(true);
        tmpWarning.text = STR_WARNING_SUDDEN_ATTACK;
        imgWarning.DOColor(Color.black, 0.5f).SetEase(Ease.Linear).SetLoops(6, LoopType.Yoyo);
        tmpWarning.DOColor(Color.red, 0.5f).SetEase(Ease.Linear).SetLoops(6, LoopType.Yoyo);
        await Task.Delay(3000);
        goWarning.SetActive(false);

    }

    private async void OnWarningBoss(Component arg1, object arg2)
    {
        goWarning.SetActive(true);
        tmpWarning.text = STR_WARNING_BOSS_ASSAULT;
        imgWarning.DOColor(new Color(0f,0f,0f,0.2f), 0.5f).SetEase(Ease.Linear).SetLoops(8, LoopType.Yoyo);
        tmpWarning.DOColor(Color.red, 0.5f).SetEase(Ease.Linear).SetLoops(8, LoopType.Yoyo);
        await Task.Delay(5000);
        goWarning.SetActive(false);

    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_TAKE_EXP, UpdateSliderExp);
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_LEVELUP, OnLevelUp);
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_TAKE_GACHA, OnTakeGacha);
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_TAKE_GACHA_RV, OnTakeGachaRV);
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_DIE, OnCharacterDie);
        EventDispatcher.Instance.RemoveListener(EventID.BOSS_DIE, OnBossDie);
        EventDispatcher.Instance.RemoveListener(EventID.BOSS_APPEAR, OnBossAppear);
        EventDispatcher.Instance.RemoveListener(EventID.BOSS_TAKE_DAMAGE, OnBossTakeDamage);
        EventDispatcher.Instance.RemoveListener(EventID.MONSTER_DIE, OnMonsterDie);
        EventDispatcher.Instance.RemoveListener(EventID.CHARACTER_TAKE_GOLD, OnTakeGold);
        EventDispatcher.Instance.RemoveListener(EventID.CAMPAIGN_FINISH, OnCampaignFinish);
        EventDispatcher.Instance.RemoveListener(EventID.WARNING_BOSS_ASSAULT, OnWarningBoss);
        EventDispatcher.Instance.RemoveListener(EventID.WARNING_SUDDEN_ATTACK, OnWarningSuddenAttack);
    }
    private void OnCampaignFinish(Component arg1, object arg2)
    {
        PopupGamePlayEnd(true);
    }

    private void OnTakeGold(Component arg1, object arg2)
    {

        tmpGoldCollected.text = GameDynamicData.GoldReceived.ToShortString();

    }

    private void OnMonsterDie(Component arg1, object arg2)
    {
        tmpCountMonster.text = GameDynamicData.KillCount.ToShortString();
    }

    private void OnBossTakeDamage(Component arg1, object arg2)
    {
        //UpdateStateBar(true);
        barBossHp.fillAmount = (float)arg2;
    }

    private void OnBossAppear(Component arg1, object arg2)
    {
        UpdateStateBar(true);
        tmpBossName.text = "Boss-A";
    }
    private void UpdateStateBar(bool showBossBar)
    {
        fillerBossHp.gameObject.SetActive(showBossBar);
        fillerExp.gameObject.SetActive(!showBossBar);
    }
    private void OnBossDie(Component arg1, object arg2)
    {
        UpdateStateBar(false);
        tmpBossName.text = "---";

    }

    private async void OnCharacterDie(Component arg1, object arg2)
    {
        await Task.Delay(1200);
        Time.timeScale = 0;
        if (GameDynamicData.IsAvailableRevive)
        {
            //GameDynamicData.AvailableRevive = false;
            OpenRevive();
        }
        else
        {
            PopupGamePlayEnd(false);
        }
    }
    private void PopupGamePlayEnd(bool isSurviveSuccess)
    {
        GameDynamicData.IsSurviveSuccess = isSurviveSuccess;
        Time.timeScale = 0;
        Open(PopupType.EndGamePlayScene);
    }
    private void OpenRevive()
    {
        //await Task.Delay(100);
        Time.timeScale = 0;
        Open(PopupType.REVIVE);
    }

    private void OnTakeGacha(Component arg1, object arg2)
    {
        Time.timeScale = 0;
        Open(PopupType.GACHA_UI);
    }
    private void OnTakeGachaRV(Component arg1, object arg2)
    {
        Time.timeScale = 0;
        Open(PopupType.GACHA_UI_RV);
    }
    private void OnLevelUp(Component arg1, object arg2)
    {
        FillFullExp();
        tmpLevel.text = string.Concat(STR_LEVEL, character.CurrentLevel.ToShortString());
        Time.timeScale = 0;
        //await Task.Delay(100);
        ShowPopupLearnSkill();
    }
    private void ShowPopupLearnSkill()
    {
        //popupLearnSkill.Open();
        Open(PopupType.LEARN_SKILL);
    }
    private void FillFullExp()
    {
        barExp.fillAmount = 1f;
    }
    private void UpdateSliderExp(Component arg1, object arg2)
    {
        curFillAmout = (float)character.CurrentExp / character.NextLevelExp;
        barExp.fillAmount = curFillAmout;
    }

    // Update is called once per frame
    void Update()
    {
        CounterTimer();
        SearchingCharacter();
    }
    
    private void CounterTimer() {
        if (Time.time - refreshTime >= 1)
        {
            refreshTime = Time.time - startTime;
            TimeSpan ts = TimeSpan.FromSeconds(InGameManager.Instance.TotalTimePlay);

            tmpTimer.text = ts.ToString("mm\\:ss");
        }
    }
    private void SearchingCharacter()
    {
        if (character == null)
        {
            var canUpLevel = GameObject.FindGameObjectWithTag(StringConst.TAG_PLAYER);
            if (canUpLevel)
                character = canUpLevel.GetComponent<ICanUpLevel>();
            tmpLevel.text = string.Concat(STR_LEVEL, character.CurrentLevel.ToShortString());
        }
    }
}
