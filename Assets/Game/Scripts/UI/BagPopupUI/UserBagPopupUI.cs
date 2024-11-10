using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserBagPopupUI : PopupUI
{
    [SerializeField] GearPanelUI gearPanelUI;
    //[SerializeField] InventoryPanelUI inventoryPanelUI;
    [SerializeField] InventoryUI_v2 inventoryPanelUI_v2;
    public override void Open()
    {
        base.Open();
        gearPanelUI.Init();
        inventoryPanelUI_v2.Init();
        //inventoryPanelUI.Init();
    }

}
