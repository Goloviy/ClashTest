using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MergeSuccessPopupUI : MonoBehaviour
{
    [Header("Single Items")]
    [SerializeField] ItemInventory resultItemSingle;
    [SerializeField] TextMeshProUGUI tmpTitle;
    [SerializeField] TextMeshProUGUI tmpStat;
    [SerializeField] TextMeshProUGUI tmpStat2;
    [SerializeField] TextMeshProUGUI tmpMaxLevel;
    [SerializeField] TextMeshProUGUI tmpMaxLevel2;
    [Header("Multi Items")]
    [SerializeField] GameObject goResultMultiItem;
    [SerializeField] Transform tfContainer;
    [SerializeField] ItemInventory prefabNewItem;
    [SerializeField] Button btnClose;
    [SerializeField] Button btnClose2;
    bool isSingleOutput;
    const string STAT_EMPTY = "---";
    float itemH = 100f;
    private void Awake()
    {
        btnClose.onClick.AddListener(Close);
        btnClose2.onClick.AddListener(Close);

    }
    private void OriginPopupState()
    {
        var rtf = tfContainer.transform as RectTransform;
        rtf.localPosition = Vector3.zero;
        this.resultItemSingle.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(false);
        btnClose2.gameObject.SetActive(false);
        tmpTitle.gameObject.SetActive(false);
        goResultMultiItem.gameObject.SetActive(false);
    }
    public void Open(UserEquipment inputUserEquipment, UserEquipment outputUserEquipment)
    {
        OriginPopupState();
        if (outputUserEquipment != null && inputUserEquipment != null)
        {
            ShowResultSingle(inputUserEquipment, outputUserEquipment);
        }
        this.gameObject.SetActive(true);
    }
    public void Open(params UserEquipment[] outputUserEquipments)
    {
        int maxLine = Mathf.CeilToInt( (float)outputUserEquipments.Length / 5);
        if (tfContainer is RectTransform rtf)
        {
            var size = rtf.sizeDelta;
            size.y = (maxLine + 1) * itemH;
            rtf.sizeDelta = size;
        }
        OriginPopupState();
        if (outputUserEquipments.Length <= 0)
        {
            return;
        }
        ShowMultiResult(outputUserEquipments);
        this.gameObject.SetActive(true);
    }

    private async void ShowResultSingle(UserEquipment inputUserEquipment, UserEquipment outputUserEquipment)
    {
        tmpMaxLevel.text = STAT_EMPTY;
        tmpMaxLevel2.text = STAT_EMPTY;
        tmpStat.text = STAT_EMPTY;
        tmpStat2.text = STAT_EMPTY;
        var oldItemData = GameData.Instance.staticData.GetEquipmentData(inputUserEquipment.itemId) as EquipmentData;
        var oldRarityData = GameData.Instance.staticData.GetRarity(inputUserEquipment.rarity);
        var newItemData = GameData.Instance.staticData.GetEquipmentData(outputUserEquipment.itemId) as EquipmentData;
        var newRarityData = GameData.Instance.staticData.GetRarity(outputUserEquipment.rarity);
        resultItemSingle.Init(outputUserEquipment);
        resultItemSingle.gameObject.SetActive(true);
        await Task.Delay(500);
        //string txtTit = I2.Loc.LocalizationManager.GetTranslation(newItemData.title);
        tmpTitle.text = newItemData.title;
        tmpTitle.color = newRarityData.color;
        tmpTitle.gameObject.SetActive(true);
        await Task.Delay(300);
        tmpMaxLevel.text = oldRarityData.enchantMaxLevel.ToString();
        tmpMaxLevel2.text = newRarityData.enchantMaxLevel.ToString();
        tmpMaxLevel.gameObject.SetActive(true);
        tmpMaxLevel2.gameObject.SetActive(true);
        await Task.Delay(300);
        bool isHp = newItemData.hpBase > 0;
        var baseStat = isHp ? oldItemData.hpBase : oldItemData.atkBase;
        var oldStat = ItemHelper.CalculateStat(baseStat, oldRarityData.rarityK, inputUserEquipment.enchantLevel, oldItemData.slot);
        var newStat = ItemHelper.CalculateStat(baseStat, newRarityData.rarityK, outputUserEquipment.enchantLevel, newItemData.slot);
        tmpStat.text = oldStat.ToString();
        tmpStat2.text = newStat.ToString();
        tmpStat.gameObject.SetActive(true);
        tmpStat2.gameObject.SetActive(true);
        btnClose.gameObject.SetActive(true);
    }
    private async void ShowMultiResult(UserEquipment[] outputUserEquipments)
    {
        ClearPopupMultiItem();
        goResultMultiItem.gameObject.SetActive(true);
        int timeDelay;
        if (outputUserEquipments.Length > 10)
            timeDelay = 50;
        else
            timeDelay = 250;
        for (int i = 0; i < outputUserEquipments.Length; i++)
        {
            await Task.Delay(timeDelay);
            var item = Instantiate(prefabNewItem, tfContainer);
            item.Init(outputUserEquipments[i]);
        }
        btnClose2.gameObject.SetActive(true);
    }
    private void ClearPopupMultiItem()
    {
        var count = tfContainer.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var go = tfContainer.transform.GetChild(i).gameObject;
            Destroy(go);
        }
    }
    public void Close()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        this.gameObject.SetActive(false);
        OriginPopupState();
    }
}
