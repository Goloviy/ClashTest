using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MergeInventoryPanelUI : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] ItemMergeUI prefabItem;
    [SerializeField] MergeRowCellView cellViewPrefab;
    [SerializeField] RectTransform rectContainer;
    [SerializeField] RectTransform rectEquipments;
    //[SerializeField] RectTransform rectItems;
    List<ItemMergeUI> listElementUI;
    /// <summary>
    /// string is itemMergeUI-id
    /// </summary>
    Dictionary<string, ItemMergeUI> dictEquipment;
    //List<ItemMergeUI> listItem;
    float heightItem = 300;
    float heightLine = 100;
    [HideInInspector] public ItemMergeUI curClickElement;
    Action<string> OnClickItem;
    SortInventoryType sortType = SortInventoryType.NONE;
    [SerializeField] Button btnSort;
    int maxSort = 3;
    int minSort = 1;
    int numberOfCellsPerRow = 5;
    [SerializeField] EnhancedScroller scroller;
    SmallList<UserItem> _data;
    bool isInit = false;
    private void Awake()
    {
        _data = new SmallList<UserItem>();
        dictEquipment = new Dictionary<string, ItemMergeUI>();
        listElementUI = new List<ItemMergeUI>();
        //listItem = new List<ItemMergeUI>();
        btnSort.onClick.AddListener(OnClickSort);
    }

    private void OnClickSort()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        ChangeSortType();
        UpdateData();
        scroller.ReloadData();
    }

    public void Init(Action<string> OnClickItem)
    {
        if (!isInit)
        {
            scroller.Delegate = this;
        }
        this.OnClickItem = OnClickItem;
        sortType = SortInventoryType.RARITY;
        UpdateData();
        scroller.ReloadData();
    }
    private void ChangeSortType()
    {
        int curSort = (int)sortType;
        if (curSort < maxSort)
        {
            curSort++;
        }
        else
        {
            curSort = minSort;
        }
        sortType = (SortInventoryType)curSort;
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
    }
    //private void InitEquipments(List<UserEquipment> items)
    //{
    //    // reuser item

    //    int i = 0;
    //    foreach (var item in items)
    //    {
    //        if (listElementUI.Count > i)
    //        {
    //            listElementUI[i].gameObject.SetActive(true);
    //            listElementUI[i].Init(item, ClickItemCallBack);
    //        }
    //        else
    //        {
    //            var newItem = Instantiate( prefabItem, rectEquipments.transform);
    //            newItem.Init(item, ClickItemCallBack);
    //            listElementUI.Add(newItem);
    //            dictEquipment.Add(newItem.id, newItem);
    //        }
    //        i++;
    //    }
    //    //hide item 
    //    if (listElementUI.Count > items.Count)
    //    {
    //        int countDelta = listElementUI.Count - items.Count;
    //        for (int j = listElementUI.Count - countDelta; j < listElementUI.Count; j++)
    //        {
    //            listElementUI[j].gameObject.SetActive(false);
    //        }
    //    }
    //}

    private void ClickItemCallBack(string id)
    {
        curClickElement = dictEquipment[id];
        OnClickItem?.Invoke(id);
    }
    public void RemoveSelectedItem(string id)
    {
        if (!string.IsNullOrEmpty(id))
            dictEquipment[id].Unselect();
    }
    public ItemMergeUI SelectItem(string id)
    {
        ItemMergeUI itemUI;
        if(dictEquipment.TryGetValue(id, out itemUI))
        {
            dictEquipment[id].Select();
            return itemUI;
        }
        return null;
    }

    public ItemMergeUI GetItemUI(string id)
    {
        ItemMergeUI itemUI;
        if (dictEquipment.TryGetValue(id, out itemUI))
        {
            return itemUI;
        }
        return null;
    }
    public UserEquipment GetItemData(string slotInventoryId)
    {
        ItemMergeUI itemUI;
        if (dictEquipment.TryGetValue(slotInventoryId, out itemUI))
        {
            return itemUI.userEquipment;
        }
        return null;
    }
    private void SetHeighContainer(int itemCount)
    {
        //var sizeOrigin = rectContainer.sizeDelta;
        var height = ((itemCount - 1) / 5 + 1) * 100;
        rectContainer.sizeDelta = new Vector2(490, height + heightItem + heightLine);
        rectEquipments.sizeDelta = new Vector2(490, height);
        //rectItems.sizeDelta = new Vector2(490, heightItem);
    }

    public int GetNumberOfCells(EnhancedScroller scroller) => Mathf.CeilToInt((float) _data.Count / (float) numberOfCellsPerRow);


    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) => 105f;

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        MergeRowCellView cellView = scroller.GetCellView(cellViewPrefab) as MergeRowCellView;


        cellView.name = "Cell " + (dataIndex * numberOfCellsPerRow).ToString() + " to " + ((dataIndex * numberOfCellsPerRow) + numberOfCellsPerRow - 1).ToString();

        // pass in a reference to our data set with the offset for this cell
        var cells = cellView.SetData(ref _data, dataIndex * numberOfCellsPerRow, ClickItemCallBack);
        foreach (var cel in cells)
        {
            dictEquipment[cel.id] = cel;
        }
        //cellView.SetData(_data[dataIndex]);
        return cellView;
    }
}
