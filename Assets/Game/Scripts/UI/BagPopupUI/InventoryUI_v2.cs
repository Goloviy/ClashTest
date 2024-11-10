using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI_v2 : MonoBehaviour, IEnhancedScrollerDelegate
{
    SortInventoryType sortType = SortInventoryType.NONE;

    private SmallList<UserItem> _data;

    public GameObject goNotiEmpty;
    public EnhancedScroller scroller;
    public InventoryRowCellView cellViewPrefab;
    [SerializeField] Button btnSort;
    [SerializeField] Button btnMerge;
    [SerializeField] int numberOfCellsPerRow = 5;
    int maxSort = 3;
    int minSort = 1;

    bool isInit = false;
    private void Awake()
    {
        _data = new SmallList<UserItem>();
        btnSort.onClick.AddListener(OnClickSort);
        btnMerge.onClick.AddListener(OnClickMerge);
    }

    private void OnClickMerge()
    {
        UIManagerHome.Instance.Open(PopupType.MERGE);
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

    }

    private void OnClickSort()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        ChangeSortType();
        UpdateData();
        scroller.ReloadData();
    }


    public void Init()
    {
        if (!isInit)
        {
            scroller.Delegate = this;
            isInit = true;
        }
        sortType = SortInventoryType.ENCHANT_LEVEL;
        UpdateData();

        scroller.ReloadData();
    }
    private void UpdateData()
    {
        _data.Clear();
        List<UserEquipment> equipments;
        if (sortType == SortInventoryType.ENCHANT_LEVEL)
        {
            equipments = GameData.Instance.playerData.saveData.ListAcquired.OrderByDescending(x => x.enchantLevel).ThenByDescending(x => x.rarity).ToList();

        }
        else if (sortType == SortInventoryType.RARITY)
        {
            equipments = GameData.Instance.playerData.saveData.ListAcquired.OrderByDescending(x => x.rarity).ThenByDescending(x => x.itemId).ToList();
        }
        else
        {
            equipments = GameData.Instance.playerData.saveData.ListAcquired.OrderByDescending(x => x.enchantLevel).ThenByDescending(x => x.rarity).ToList();

        }

        foreach (var equipment in equipments)
        {
            _data.Add(equipment);
        }

        var pieces = CreatePiecesData();
        foreach (var piece in pieces)
        {
            _data.Add(piece);
        }
        var isEmpty = _data.Count <= 0;
        goNotiEmpty.gameObject.SetActive(isEmpty);
    }
    private void ChangeSortType()
    {
        int curSort = (int)sortType;
        if (curSort < maxSort)
        {
            curSort++;
        }else
        {
            curSort = minSort;
        }
        sortType = (SortInventoryType)curSort;
    }
    
    private List<UserItem> CreatePiecesData()
    {
        List<UserItem> list = new List<UserItem>();
        foreach (var typeCurrency in GameStaticData.CurrencyInBag)
        {
            var piece = CreateData(typeCurrency);
            if (piece.count > 0)
            {
                list.Add(piece);
            }

        }

        //var pieceA = CreateData(Currency.DESIGN_ARMOR);
        //var pieceBe = CreateData(Currency.DESIGN_BELT);
        //var pieceBo = CreateData(Currency.DESIGN_BOOTS);
        //var pieceG = CreateData(Currency.DESIGN_GLOVES);
        //var pieceH = CreateData(Currency.DESIGN_HELMET);
        //var pieceW = CreateData(Currency.DESIGN_WEAPON);
        //if (pieceA.count > 0)
        //{
        //    list.Add(pieceA);
        //}
        //if (pieceBe.count > 0)
        //{
        //    list.Add(pieceBe);
        //}
        //if (pieceBo.count > 0)
        //{
        //    list.Add(pieceBo);
        //}
        //if (pieceG.count > 0)
        //{
        //    list.Add(pieceG);
        //}
        //if (pieceH.count > 0)
        //{
        //    list.Add(pieceH);
        //}
        //if (pieceW.count > 0)
        //{
        //    list.Add(pieceW);
        //}
        return list;

        UserItem CreateData(Currency currency)
        {
            var value = GameData.Instance.playerData.GetCurrencyValue(currency);
            var itemData = GameData.Instance.staticData.GetCurrencyData(currency);
            var rarityData = GameData.Instance.staticData.GetRarity(itemData.rarity);
            return new UserItem(itemData.modelId, rarityData.type, (int)value);
        }
    }

    #region EnhancedScroll Delegate
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        InventoryRowCellView cellView = scroller.GetCellView(cellViewPrefab) as InventoryRowCellView;


        cellView.name = "Cell " + (dataIndex * numberOfCellsPerRow).ToString() + " to " + ((dataIndex * numberOfCellsPerRow) + numberOfCellsPerRow - 1).ToString();

        // pass in a reference to our data set with the offset for this cell
        cellView.SetData(ref _data, dataIndex * numberOfCellsPerRow);
        //cellView.SetData(_data[dataIndex]);
        return cellView;


    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 105f;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return Mathf.CeilToInt((float)_data.Count / (float)numberOfCellsPerRow);
    }
    #endregion

}
