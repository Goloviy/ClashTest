using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInformationPopupUI : PopupUI
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI tmpTitle;
    [SerializeField] TextMeshProUGUI tmpStat;
    [SerializeField] TextMeshProUGUI tmpLevel;
    [SerializeField] TextMeshProUGUI tmpDescriptions;
    [SerializeField] TextMeshProUGUI tmpRarity;
    [SerializeField] TextMeshProUGUI tmpEnchantGold;
    [SerializeField] TextMeshProUGUI tmpEnchantDesign;
    [SerializeField] GradeSkillElementUI prefabGradeSkill;
    [SerializeField] Transform tfGradeSkill;
    [SerializeField] Button btnEquip;
    [SerializeField] Button btnUnEquip;
    [SerializeField] Button btnLevelUp;
    [SerializeField] Button btnLevelUpMax;
    [SerializeField] Button btnRecycle;
    [SerializeField] Button btnClose;
    [SerializeField] Image imgIcon;
    [SerializeField] Image imgBgIcon;
    [SerializeField] Image imgRarity;
    [SerializeField] Image imgItemDesign;
    [SerializeField] Image imgRequireDesign;
    [SerializeField] Image imgStat;
    [SerializeField] GameObject groupGold;
    [SerializeField] GameObject groupDesign;

    [SerializeField] Sprite sprAtkIcon;
    [SerializeField] Sprite sprHpIcon;

    EquipmentData equipmentData;
    UserEquipment userEquipment;

    const string STR_SLASH = "/";
    const string STR_ADD = "+";
    const string STR_LEVEL = "Level: ";
    
    //temp
    List<GradeSkillElementUI> gradeElements;
    bool canEnchant = false;
    bool canEquip = false;
    protected override void Awake()
    {
        base.Awake();
        gradeElements = new List<GradeSkillElementUI>();
        btnClose.onClick.AddListener(ClickClose);
        btnEquip.onClick.AddListener(ClickEquip);
        btnUnEquip.onClick.AddListener(ClickUnEquip);
        btnLevelUp.onClick.AddListener(ClickEnchantLevel);
        btnLevelUpMax.onClick.AddListener(ClickEnchantMaxLevel);
        btnRecycle.onClick.AddListener(ClickRecycle);
    }


    public override void Open()
    {
        base.Open();
        Init(GameDynamicData.SelectedEquipment, GameDynamicData.SelectedGearCanEquip);
    }
    public void Init(UserEquipment userEquipment, bool canEquip)
    {
        this.canEquip = canEquip;
        ClearGradeSkill();
        this.userEquipment = userEquipment;
        equipmentData = GameData.Instance.staticData.GetEquipmentData(userEquipment.itemId) as EquipmentData;
        UpdateUI();
    }
    private void UpdateUI()
    {
        UpdateBasicInfo();
        UpdateGradeSkill();
        UpdateRarity();
        UpdateEnchant();
        UpdateGroupButton();
        UpdateStat();
    }
    private void UpdateStat()
    {
        imgStat.overrideSprite = equipmentData.atkBase > 0 ? sprAtkIcon : sprHpIcon;
        int baseStat = equipmentData.atkBase > 0 ? equipmentData.atkBase : equipmentData.hpBase;
        var rarityData = GameData.Instance.staticData.GetRarity(userEquipment.rarity);
        int totalStat = ItemHelper.CalculateStat(baseStat, rarityData.rarityK, userEquipment.enchantLevel, equipmentData.slot);
        tmpStat.text = String.Concat(STR_ADD, totalStat.ToShortString());    
    }
    private void UpdateGroupButton()
    {
        var rarityData = GameData.Instance.staticData.GetRarity(userEquipment.rarity);
        int levelMax = rarityData.enchantMaxLevel;
        int curLevelEnchant = userEquipment.enchantLevel;
        btnLevelUp.gameObject.SetActive(curLevelEnchant < levelMax);
        btnLevelUpMax.gameObject.SetActive(curLevelEnchant < levelMax);
        btnEquip.gameObject.SetActive(canEquip);
        btnUnEquip.gameObject.SetActive(!canEquip);
    }
    private void UpdateBasicInfo()
    {
        var rarityData = GameData.Instance.staticData.GetRarity(userEquipment.rarity);
        var designData = GameData.Instance.staticData.GetSlot(equipmentData.slot);
        //level
        int levelMax = rarityData.enchantMaxLevel;
        int curLevelEnchant = userEquipment.enchantLevel;
        StringBuilder builder = new StringBuilder();
        builder.Append(STR_LEVEL);
        builder.Append(curLevelEnchant);
        builder.Append(STR_SLASH);
        builder.Append(levelMax);
        tmpLevel.text = builder.ToString();

        //Icons
        int index = 0;
        if (equipmentData.spriteIcons.Length > (int)userEquipment.rarity)
            index = (int)userEquipment.rarity;
        imgIcon.overrideSprite = equipmentData.spriteIcons[index];
        //imgBgIcon.color = rarityData.color;
        imgBgIcon.overrideSprite = rarityData.border;
        imgRarity.overrideSprite = rarityData.borderEquipmentInfo;
        imgItemDesign.overrideSprite = designData.icon;
        imgItemDesign.overrideSprite = designData.icon;
        //text
        //string txtDes = I2.Loc.LocalizationManager.GetTranslation(equipmentData.descriptions);
        tmpDescriptions.text = equipmentData.descriptions;
        //string txtTit = I2.Loc.LocalizationManager.GetTranslation(equipmentData.title);
        tmpTitle.text = equipmentData.title;
    }
    private void UpdateGradeSkill()
    {
        ClearGradeSkill();
        var skills = equipmentData.gradeSkills;
        for (int i = 0; i < skills.Length; i++)
        {
            var elementSkillUI = Instantiate(prefabGradeSkill, tfGradeSkill);
            elementSkillUI.Init(skills[i], userEquipment.rarity);
            gradeElements.Add(elementSkillUI);
        }
    }
    private void UpdateRarity()
    {
        var rarityData = GameData.Instance.staticData.GetRarity(userEquipment.rarity);
        //string txtTit = I2.Loc.LocalizationManager.GetTranslation(rarityData.title);
        tmpRarity.text = rarityData.title;
        //tmpRarity.color = rarityData.color;

    }
    private void UpdateEnchant()
    {
        var rarityData = GameData.Instance.staticData.GetRarity(userEquipment.rarity);
        //level
        int levelMax = rarityData.enchantMaxLevel;
        int curLevelEnchant = userEquipment.enchantLevel;
        bool isMaxLevel = curLevelEnchant >= levelMax;
        groupDesign.SetActive(!isMaxLevel);
        groupGold.SetActive(!isMaxLevel);
        canEnchant = !isMaxLevel;
        if (isMaxLevel)
            return;
        var slotData = GameData.Instance.staticData.GetSlot(equipmentData.slot);
        var designData = GameData.Instance.staticData.GetCurrencyData(slotData.designRequire);
        imgRequireDesign.overrideSprite = designData.icon;

        int nextLevel = userEquipment.enchantLevel + 1;
        int requireGold = ItemHelper.CalculateEnchantRequireGold(nextLevel, equipmentData.slot);
        int requireDesign = ItemHelper.CalculateEnchantRequireDesign(nextLevel, equipmentData.slot);
        tmpEnchantGold.gameObject.SetActive(requireGold > 0);
        tmpEnchantDesign.gameObject.SetActive(requireDesign > 0);
        long pGold = GameData.Instance.playerData.GetCurrencyValue( Currency.GOLD);
        long pDesign = GameData.Instance.playerData.GetCurrencyValue(equipmentData.slot.ToCurrency());
        bool isEnoughGold = pGold >= requireGold;
        bool isEnoughDesign = pDesign >= requireDesign;
        //tmpEnchantGold.color = !isEnoughGold ? Color.red : Color.cyan;
        //tmpEnchantDesign.color = !isEnoughDesign ? Color.red : Color.cyan;
        StringBuilder builder = new StringBuilder();
        builder.Append(pGold.ToShortStringK());
        builder.Append(STR_SLASH);
        builder.Append(requireGold.ToShortStringK());
        tmpEnchantGold.text = builder.ToString();
        builder.Clear();
        builder.Append(pDesign);
        builder.Append(STR_SLASH);
        builder.Append(requireDesign);
        tmpEnchantDesign.text = builder.ToString();
        canEnchant = isEnoughGold && isEnoughDesign && !isMaxLevel;
    }
    private void ClearGradeSkill()
    {
        foreach (var go in gradeElements)
        {
            Destroy(go.gameObject);
        }
        gradeElements.Clear();
    }
    public void ClickEquip()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        GameData.Instance.playerData.Equip(userEquipment);
        EventDispatcher.Instance.PostEvent(EventID.CHANGE_EQUIPMENT);
        ClickClose();
    }
    public void ClickUnEquip()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        GameData.Instance.playerData.UnEquip(userEquipment);
        EventDispatcher.Instance.PostEvent(EventID.CHANGE_EQUIPMENT);
        ClickClose();
    }
    private void ClickRecycle()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        var resourceRecycle = ItemHelper.DownAllLevelEnchant(userEquipment.enchantLevel, equipmentData.slot);
        var currencyDesign = equipmentData.slot.ToCurrency();

        //change item level to origin;
        userEquipment.enchantLevel = 1;
        GameData.Instance.playerData.saveData.IsDirty = true;
        //add resource recycle
        //GameData.Instance.playerData.saveData.ModifyGold(resourceRecycle.gold);
        GameData.Instance.playerData.AddCurrency(currencyDesign, resourceRecycle.countDesign);
        GameData.Instance.playerData.AddCurrency(Currency.GOLD, resourceRecycle.gold);
        //GameData.Instance.playerData.saveData.AddItemToBag(new UserItem(itemDesign.id, Rarity.Common, resourceRecycle.countDesign));
        Init(GameDynamicData.SelectedEquipment, true);

        EventDispatcher.Instance.PostEvent(EventID.ENCHANT_LEVEL_EQUIPMENT);

    }
    public void ClickEnchantLevel()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (canEnchant)
        {
            GameSystem.Instance.equipmentSystem.EnchantEquipment(userEquipment, false);
            //load lai giao dien
            Init(GameDynamicData.SelectedEquipment, true);
        }
        else
        {
            string text = I2.Loc.LocalizationManager.GetTranslation(DictString.NOT_ENOUGH_SOURCE);
            AlertPanel.Instance.ShowNotice(DictString.NOT_ENOUGH_SOURCE, null);
        }
    }

    //private void EnchantEquipment(bool isAuto)
    //{
    //    bool isEnoughGold = false;
    //    bool isEnoughDesign = false;
    //    bool isMaxLevel = true;
    //    do
    //    {
    //        int nextLevel = userEquipment.enchantLevel + 1;
    //        int requireGold = ItemHelper.CalculateEnchantRequireGold(nextLevel, equipmentData.slot);
    //        int requireDesign = ItemHelper.CalculateEnchantRequireDesign(nextLevel, equipmentData.slot);
    //        int pGold = GameData.Instance.playerData.saveData.Gold;
    //        int pDesign = GameData.Instance.playerData.saveData.GetCountDesign(equipmentData.slot);

    //        var rarityData = GameData.Instance.staticData.GetRarity(userEquipment.rarity);
    //        int levelMax = rarityData.enchantMaxLevel;
    //        int curLevelEnchant = userEquipment.enchantLevel;
    //        isEnoughGold = pGold >= requireGold;
    //        isEnoughDesign = pDesign >= requireDesign;
    //        isMaxLevel = curLevelEnchant >= levelMax;
    //        if (isEnoughGold && isEnoughDesign && !isMaxLevel)
    //        {
    //            GameData.Instance.playerData.AddCurrency(Currency.GOLD, -requireGold);
    //            var itemDesign = GameData.Instance.staticData.items.GetDataItemDesign(equipmentData.slot);
    //            GameData.Instance.playerData.saveData.RemoveDesignInBag(itemDesign.id, requireDesign);
    //            GameData.Instance.playerData.saveData.IsDirty = true;
    //            userEquipment.enchantLevel++;

    //        }
    //        else
    //        {
    //            DebugCustom.Log("End Auto Upgrade Level");
    //            break;
    //        }
    //    } while (isAuto);
    //    //tinh toan lai equipments stat
    //    GameData.Instance.playerData.tempData.CalculateStats();
    //    EventDispatcher.Instance.PostEvent(EventID.ENCHANT_LEVEL_EQUIPMENT);
    //}
    
    public void ClickEnchantMaxLevel()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (canEnchant)
        {
            GameSystem.Instance.equipmentSystem.EnchantEquipment(userEquipment, true);
            //load lai giao dien
            Init(GameDynamicData.SelectedEquipment, true);
        }
        else
        {
            //string text = I2.Loc.LocalizationManager.GetTranslation(DictString.NOT_ENOUGH_SOURCE);

            AlertPanel.Instance.ShowNotice(DictString.NOT_ENOUGH_SOURCE, null);
        }
    }
    private void ClickClose()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        UIManagerHome.Instance.Back();
    }
}
