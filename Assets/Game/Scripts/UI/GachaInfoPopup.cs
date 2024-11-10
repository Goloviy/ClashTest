using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaInfoPopup : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] TextMeshProUGUI tmpTitle;
    [SerializeField] Button btnClose;

    private SmallList<EquipmentRewardItem> _data;

    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewPrefab;
    private void Awake()
    {
        _data = new SmallList<EquipmentRewardItem>();
        btnClose.onClick.AddListener(OnClose);
    }
    void Start()
    {
        //Application.targetFrameRate = 60;
        scroller.Delegate = this;
        //this.gameObject.SetActive(false);
    }
    private void OnClose()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        this.gameObject.SetActive(false);
    }

    public void Open(GachaType gachaType)
    {

        this.gameObject.SetActive(true);
        var GachaDataBox = GameData.Instance.staticData.gachaData.GetSystemData(gachaType);
        //I2.Loc.LocalizationManager.GetTranslation(gachaType.ToString());
        tmpTitle.text = gachaType.ToString();
        _data.Clear();

        for (int i = 0; i < GachaDataBox.superEquipments.Length; i++)
        {
            _data.Add(new EquipmentRewardItem()
            {
                count = 1,
                equipmentData = GachaDataBox.superEquipments[i],
                rarity = GachaDataBox.rarityMax
            });
        }
        for (int i = 0; i < GachaDataBox.equipments.Length; i++)
        {
            _data.Add(new EquipmentRewardItem()
            {
                count = 1,
                equipmentData = GachaDataBox.equipments[i],
                rarity = GachaDataBox.rarityMax
            });
        }
        scroller.ReloadData();
    }

    #region EnhancedScroller Handlers
    /// <summary>
    /// This will be the prefab of each cell in our scroller. Note that you can use more
    /// than one kind of cell, but this example just has the one type.
    /// </summary>
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        GachaInfoCellView cellView = scroller.GetCellView(cellViewPrefab) as GachaInfoCellView;

        cellView.name = "Cell " + dataIndex.ToString();

        cellView.SetData(_data[dataIndex]);

        return cellView;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 160f;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data.Count;
    }
    #endregion
}
