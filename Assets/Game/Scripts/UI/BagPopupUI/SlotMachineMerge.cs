using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineMerge : MonoBehaviour
{
    [SerializeField] ItemInventory itemSlotUI;
    public UserEquipment userEquipment;
    Button btnThis;
    public string bagSlotId;
    public bool availableItem;
    public SlotMergeType type;
    MergePopupUI mergePopupUI;
    private void Awake()
    {
        btnThis = GetComponent<Button>();
        btnThis.onClick.AddListener(OnClick);
    }
    public void Init( MergePopupUI mergePopupUI , UserEquipment userEquipment, string bagSlotId)
    {
        if (userEquipment != null)
        {
            this.itemSlotUI.gameObject.SetActive(true);
            this.mergePopupUI = mergePopupUI;
            this.bagSlotId = bagSlotId;
            this.userEquipment = userEquipment;
            this.itemSlotUI.Init(userEquipment);
            availableItem = true;
        }
        else
        {
            ClearSlot();
        }

    }
    private void ClearSlot()
    {
        availableItem = false;
        this.userEquipment = null;
        this.itemSlotUI.gameObject.SetActive(false);
    }
    private void OnClick()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (string.IsNullOrEmpty(bagSlotId))
        {
            return;
        }
        mergePopupUI.OnClickSlotMachineMerge(bagSlotId, type);
    }
    public void RemoveItem()
    {
        ClearSlot();
    }
}
