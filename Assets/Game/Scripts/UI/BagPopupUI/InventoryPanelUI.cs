using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField] ItemInventory prefabItem;
    [SerializeField] RectTransform rectContainer;
    [SerializeField] RectTransform rectEquipments;
    [SerializeField] RectTransform rectItems;
    SmallList<ItemInventory> listItemUI;
    SmallList<ItemInventory> listDesign;
    float heightItem = 300;
    float heightLine = 100;
    [SerializeField] Button btnSort;
    [SerializeField] Button btnMerge;
    SortInventoryType sortType = SortInventoryType.NONE;
    private void Awake()
    {
        listItemUI = new SmallList<ItemInventory>();
        listDesign = new SmallList<ItemInventory>();
        btnMerge.onClick.AddListener(OnClickMerge);
        btnSort.onClick.AddListener(OnClickSort);
    }

    private void OnClickSort()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

    }

    private void OnClickMerge()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        UIManagerHome.Instance.Open(PopupType.MERGE);
    }

    public void Init()
    {
        sortType = SortInventoryType.ENCHANT_LEVEL;
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
        var pieces = CreatePiecesData();
        SetHeighContainer(equipments.Count);
        InitEquipments(equipments);
        InitItems(pieces);
    }
    private List<Tuple<Sprite,Sprite,int>> CreatePiecesData()
    {
        List<Tuple<Sprite, Sprite, int>> list = new List<Tuple<Sprite, Sprite, int>>();
        var pieceA = CreateData(Currency.DESIGN_ARMOR);
        var pieceBe = CreateData(Currency.DESIGN_BELT);
        var pieceBo = CreateData(Currency.DESIGN_BOOTS);
        var pieceG = CreateData(Currency.DESIGN_GLOVES);
        var pieceH = CreateData(Currency.DESIGN_HELMET);
        var pieceW = CreateData(Currency.DESIGN_WEAPON);
        if (pieceA.Item3 > 0)
        {
            list.Add(pieceA);
        }
        if (pieceBe.Item3 > 0)
        {
            list.Add(pieceBe);
        }
        if (pieceBo.Item3 > 0)
        {
            list.Add(pieceBo);
        }
        if (pieceG.Item3 > 0)
        {
            list.Add(pieceG);
        }
        if (pieceH.Item3 > 0)
        {
            list.Add(pieceH);
        }
        if (pieceW.Item3 > 0)
        {
            list.Add(pieceW);
        }
        return list;

        Tuple<Sprite, Sprite, int> CreateData(Currency currency)
        {
            var value = GameData.Instance.playerData.GetCurrencyValue(currency);
            var itemData = GameData.Instance.staticData.GetCurrencyData(currency);
            var rarityData = GameData.Instance.staticData.GetRarity(itemData.rarity);
            return new Tuple<Sprite, Sprite, int>(itemData.icon, rarityData.border, (int)value);
        }
    }
    private void InitEquipments(List<UserEquipment> listData)
    {
        // reuser item

        int i = 0;
        foreach (var userEquipment in listData)
        {
            if (listItemUI.Count > i)
            {
                listItemUI[i].gameObject.SetActive(true);
                listItemUI[i].Init(userEquipment);
            }
            else
            {
                var newItemUI = Instantiate(prefabItem, rectEquipments.transform);
                newItemUI.Init(userEquipment);
                listItemUI.Add(newItemUI);
            }
            i++;
        }
        //hide item 
        if (listItemUI.Count > listData.Count)
        {
            int countDelta = listItemUI.Count - listData.Count;
            for (int j = listItemUI.Count - countDelta; j < listItemUI.Count; j++)
            {
                listItemUI[j].gameObject.SetActive(false);
            }
        }

    }
    private void InitItems(List<Tuple<Sprite, Sprite, int>> items)
    {
        //create and reuser item

        int i = 0;
        foreach (var item in items)
        {
            if (listDesign.Count > i)
            {
                listDesign[i].gameObject.SetActive(true);
                listDesign[i].Init(item);
            }
            else
            {
                var newItem = Instantiate(prefabItem, rectItems.transform);
                newItem.Init(item);
                listDesign.Add(newItem);
            }
            i++;
        }
        //hide item 
        if (listDesign.Count > items.Count)
        {
            int countDelta = listDesign.Count - items.Count;
            for (int j = listDesign.Count - countDelta; j < listDesign.Count; j++)
            {
                listDesign[j].gameObject.SetActive(false);
            }
        }
    }
    private void SetHeighContainer(int itemCount)
    {
        //var sizeOrigin = rectContainer.sizeDelta;
        var height = ((itemCount - 1) / 5 + 1) * 100;
        rectContainer.sizeDelta = new Vector2(500, height + heightItem + heightLine);
        rectEquipments.sizeDelta = new Vector2(500, height);
        rectItems.sizeDelta = new Vector2(500, heightItem);
    }

}
