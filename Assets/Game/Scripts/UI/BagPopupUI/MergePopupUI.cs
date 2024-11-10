using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MergePopupUI : PopupUI
{
    [SerializeField] MergeInventoryPanelUI inventoryPanelUI;
    [SerializeField] MergeSuccessPopupUI mergeSuccessPopupUI;
    [SerializeField] SlotMachineMerge slotTargetEquipment;
    [SerializeField] SlotMachineMerge slotMachineMergeMain;
    [SerializeField] SlotMachineMerge slotMachineMergeSub;
    [SerializeField] SlotMachineMerge slotMachineMergeSub2;
    [SerializeField] Button btnBack;
    [SerializeField] Button btnMerge;
    [SerializeField] Button btnAutoMerge;

    MergeData curMergeData;
    List<string> listSelectedId;
    UserEquipment userEquipTarget;
    protected override void Awake()
    {
        listSelectedId = new List<string>();
        base.Awake();
        slotTargetEquipment.Init(this, null, string.Empty);
        slotMachineMergeMain.Init(this, null, string.Empty);
        slotMachineMergeSub.Init(this, null, string.Empty);
        slotMachineMergeSub2.Init(this, null, string.Empty);
        ClearMachineUI();
        btnBack.onClick.AddListener(OnClickBack);
        btnMerge.onClick.AddListener(OnClickMerge);
        btnAutoMerge.onClick.AddListener(OnClickAutoMerge);
        mergeSuccessPopupUI.gameObject.SetActive(false);
    }

    private void OnClickMerge()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        List<UserEquipment> removeList = new List<UserEquipment>();
        bool removeAllSuccess = true;
        bool haveItemEquipped = false;
        if (listSelectedId.Count >= curMergeData.subCount + 1)
        {
            DeleteItemsFromMachine();
            //create new Item
            if (removeAllSuccess)
            {
                //merge success
                //add new item and clear UI
                var newUserEquip = GameData.Instance.playerData.saveData.AddItemToBag(userEquipTarget) as UserEquipment;
                if (haveItemEquipped)
                    GameData.Instance.playerData.Equip(newUserEquip);
                var itemUI = inventoryPanelUI.GetItemData(listSelectedId[0]);
                mergeSuccessPopupUI.Open(itemUI, userEquipTarget);
                ClearMachineUI();
            }
            else
            {
                //merge fail
                //add old item, which was deleted
                DebugCustom.LogError("Something went wrong :" + String.Join(", ", listSelectedId.ToArray()));
                ReturnEquipmentHasDeleted();
                ClearMachineUI();
            }
            ItemHelper.IsInventoryChanged = true;
            ClearMachineUI();
            InitInventory();
        }

        #region local func
        void ReturnEquipmentHasDeleted()
        {
            foreach (var userEquipment in removeList)
            {
                GameData.Instance.playerData.saveData.AddItemToBag(userEquipment);
            }
        }
        void DeleteItemsFromMachine()
        {
            foreach (var id in listSelectedId)
            {
                var userEquipment = inventoryPanelUI.GetItemData(id);
                if (userEquipment != null)
                {
                    var isEquipped = GameData.Instance.playerData.saveData.IsEquipped(userEquipment);
                    if (isEquipped)
                    {
                        haveItemEquipped = true;
                        var itemUnequip = GameData.Instance.playerData.UnEquip(userEquipment);
                        if (itemUnequip == null)
                        {
                            removeAllSuccess = false;
                            break;
                        }
                    }
                    var itemRemove = GameData.Instance.playerData.saveData.RemoveEquipmentInBag(userEquipment);
                    if (itemRemove == null)
                    {
                        removeAllSuccess = false;
                        break;
                    }
                    else
                        removeList.Add(itemRemove);

                }
                else
                {
                    removeAllSuccess = false;
                    break;
                }
            }
        }
        #endregion
    }
    private void OnClickAutoMerge()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        List<UserEquipment> listEquipment = new List<UserEquipment>();
        foreach (var userEquipments in ItemHelper.dictMergeableGroup.Values)
        {
            bool isGroupHaveEquipped = false;
            UserEquipment userEquipmentEquipped = null;
            UserEquipment userEquipmentRemove = null;
            int mergeCount = userEquipments.Count / 3;
            int removeCount = mergeCount * 3;
            for (int i = 0; i < userEquipments.Count; i++)
            {
                var userEquipment = userEquipments[i];
                
                if (i < removeCount)
                {
                    if (GameData.Instance.playerData.saveData.ListEquipped.Contains(userEquipment))
                    {
                        //neu la mon do dang mac
                        isGroupHaveEquipped = true;
                        userEquipmentEquipped = userEquipment;
                    }
                    else
                    {
                        //doi voi mon do duoi tay nai . Recycle no , sau do xoa di
                        var eData = GameData.Instance.staticData.GetEquipmentData(userEquipment.itemId);
                        var resource = ItemHelper.DownAllLevelEnchant(userEquipment.enchantLevel, eData.slot);
                        //var itemDesign = GameData.Instance.staticData.items.GetDataItemDesign(eData.slot);
                        var currencyDesign = eData.slot.ToCurrency();
                        //add resource recycle
                        //GameData.Instance.playerData.saveData.ModifyGold(resource.gold);
                        GameData.Instance.playerData.AddCurrency(currencyDesign, resource.countDesign);
                        GameData.Instance.playerData.AddCurrency(Currency.GOLD, resource.gold);
                        //GameData.Instance.playerData.saveData.AddItemToBag(new UserEquipment(itemDesign.id, Rarity.Common, resource.countDesign));
                        //xoa equipment
                        userEquipmentRemove = userEquipment;
                        GameData.Instance.playerData.saveData.RemoveEquipmentInBag(userEquipment);
                    }
                }
            }

            var targetRarity = (Rarity)((int)userEquipmentRemove.rarity + 1);
            var newUserEquipment = new UserEquipment(userEquipmentRemove.itemId, targetRarity, 1);

            for (int i = 0; i < mergeCount; i++)
            {
                if (isGroupHaveEquipped)
                {
                    
                    //neu mon do` dang deo tren nguoi . Nang cap rarity giu nguyen level enchant
                    //1 loai equipment chi co' toi da 1 mon deo tren nguoi
                    userEquipmentEquipped.rarity = targetRarity;
                    GameData.Instance.playerData.saveData.IsDirty = true;
                    isGroupHaveEquipped = false;
                }
                else
                {
                    GameData.Instance.playerData.saveData.AddItemToBag(newUserEquipment);
                    listEquipment.Add(newUserEquipment);
                }
            }

        }
        mergeSuccessPopupUI.Open(listEquipment.ToArray());
        ItemHelper.IsInventoryChanged = true;
        ClearMachineUI();
        InitInventory();
    }
    private void OnClickBack()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        ClearMachineUI();
        UIManagerHome.Instance.Back();
    }

    public override void Open()
    {
        base.Open();
        InitInventory();
    }

    private void InitInventory()
    {
        inventoryPanelUI.Init(ClickItemInventoryCallBack);

    }
    private void ClickItemInventoryCallBack(string slotInventoryId)
    {
        //check slot in bag available item id
        var selectedItemUI = inventoryPanelUI.GetItemUI(slotInventoryId);
        if (!selectedItemUI)
        {
            DebugCustom.Log("Bag-slot id not available item :" + slotInventoryId);
            return;
        }
        //check can upgrade rarity
        if (selectedItemUI.userEquipment.rarity == Rarity.Mythical)
        {
            DebugCustom.Log("Highest rarity" );
            return;
        }
        //check slot and setup machine data, machine ui
        var slotMachineEmpty = GetSlotEmpty();
        if (slotMachineEmpty == null)
        {
            DebugCustom.Log("Full or null slot machine");
            return;
        }
        if (slotMachineEmpty != null && slotMachineEmpty.type == SlotMergeType.MAIN)
        {
            CreateMachineMergeData(selectedItemUI.userEquipment);
            SetupMachineUI(inventoryPanelUI.GetItemData(slotInventoryId));
        }
        //check item was selected
        if (listSelectedId.Contains(slotInventoryId))
        {
            DebugCustom.Log("Item is Selected");
            return;
        }
        //check condition merge by rarity
        var selectedItemData = inventoryPanelUI.GetItemData(slotInventoryId);
        if (curMergeData.mergeType == MergeType.SAME_ITEM_AND_RARITY)
        {
            if (curMergeData.itemMainID != selectedItemData.itemId || curMergeData.itemMainRarity != selectedItemData.rarity)
            {
                DebugCustom.Log("Item need same item, rarity");
                return;
            }
        }
        else if (curMergeData.mergeType == MergeType.ONLY_SAME_RARITY)
        {
            if (curMergeData.itemMainRarity != selectedItemData.rarity)
            {
                DebugCustom.Log("Item need same rarity");
                return;
            }
        }
        //setup UI Bag , Slot Machine UI
        if (selectedItemUI)
        {
            listSelectedId.Add(slotInventoryId);
            inventoryPanelUI.SelectItem(slotInventoryId);
            slotMachineEmpty.Init(this, selectedItemUI.userEquipment, selectedItemUI.id);
            slotTargetEquipment.Init(this, userEquipTarget, string.Empty);
            CheckMergeCondition();
        }

    }
    

    private void SetupMachineUI(UserEquipment userEquipment)
    {
        listSelectedId.Clear();
        SetupUIMachine();
    }
    private void CreateMachineMergeData(UserEquipment userEquipment)
    {
        var rarityData = GameData.Instance.staticData.GetRarity(userEquipment.rarity);
        curMergeData = new MergeData()
        {
            subCount = rarityData.subCount,
            mergeType = rarityData.mergeType,
            itemMainID = userEquipment.itemId,
            itemMainRarity = userEquipment.rarity,
        };

        userEquipTarget = new UserEquipment(userEquipment.itemId, (Rarity)((int)userEquipment.rarity + 1), userEquipment.enchantLevel);

    }
    private void SetupUIMachine()
    {
        
        slotMachineMergeSub.gameObject.SetActive(curMergeData.subCount >= 1);
        slotMachineMergeSub2.gameObject.SetActive(curMergeData.subCount >= 2);

    }

    public void OnClickSlotMachineMerge(string id, SlotMergeType type)
    {
        //remove equip
        if (type == SlotMergeType.NONE)
        {
            return;
        }
        else if (type == SlotMergeType.MAIN)
        {
            ClearMachineUI();
        }
        else
        {
            if (type == SlotMergeType.SUB_1)
                slotMachineMergeSub.RemoveItem();
            else
                slotMachineMergeSub2.RemoveItem();
            inventoryPanelUI.RemoveSelectedItem(id);
            listSelectedId.Remove(id);
            CheckMergeCondition();
        }
    }
    private void ClearMachineUI()
    {
        //if (slotMachineMergeMain.availableItem)
            inventoryPanelUI.RemoveSelectedItem(slotMachineMergeMain.bagSlotId);
        //if (slotMachineMergeSub.availableItem)
            inventoryPanelUI.RemoveSelectedItem(slotMachineMergeSub.bagSlotId);
        //if (slotMachineMergeSub2.availableItem)
            inventoryPanelUI.RemoveSelectedItem(slotMachineMergeSub2.bagSlotId);
        slotMachineMergeMain.RemoveItem();
        slotMachineMergeSub.RemoveItem();
        slotMachineMergeSub2.RemoveItem();
        slotTargetEquipment.RemoveItem();
        slotMachineMergeSub.gameObject.SetActive(false);
        slotMachineMergeSub2.gameObject.SetActive(false);
        listSelectedId.Clear();
    }

    private SlotMachineMerge GetSlotEmpty()
    {
        if (!slotMachineMergeMain.availableItem)
        {
            return slotMachineMergeMain;
        }
        else if (!slotMachineMergeSub.availableItem && curMergeData.subCount > 0)
        {
            DebugCustom.Log("Slot Sub 1");
            return slotMachineMergeSub;
        }
        else if (!slotMachineMergeSub2.availableItem && curMergeData.subCount == 2)
        {
            DebugCustom.Log("Slot Sub 2");
            return slotMachineMergeSub2;
        }
        return null;
    }

    private void CheckMergeCondition()
    {
        btnMerge.gameObject.SetActive(listSelectedId.Count >= curMergeData.subCount + 1);
    }
}
public struct MergeData
{
    public int subCount;
    public MergeType mergeType;
    public string itemMainID;
    public Rarity itemMainRarity;
}