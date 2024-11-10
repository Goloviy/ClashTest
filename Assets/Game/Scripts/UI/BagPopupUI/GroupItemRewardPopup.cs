using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupItemRewardPopup : PopupUI
{
    [SerializeField] GachaRewardItemMini prefabItem;
    [SerializeField] Transform tfParent;
    [SerializeField] Button btnClose;
    Tuple<Sprite, Sprite, string>[] groupRewardPopupData;
    GameObject[] items;
    
    protected override void Awake()
    {
        base.Awake();
        btnClose.onClick.AddListener(OnClose);
    }
    public override void Open()
    {
        groupRewardPopupData = GameDynamicData.groupRewardPopupEquipData;
        items = new GameObject[groupRewardPopupData.Length];
        base.Open();
        InitUI();
    }
    private void InitUI()
    {
        int i = 0;
        foreach (var itemData in groupRewardPopupData)
        {
            var newElement = Instantiate(prefabItem, tfParent);
            newElement.Init(itemData.Item1, itemData.Item2, itemData.Item3, null);
            items[i++] = newElement.gameObject;
        }
    }
    private void ClearItem()
    {
        foreach (var item in items)
        {
            GameObject.Destroy(item);
        }
        Array.Clear(items, 0, items.Length);
    }
    private void OnClose()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        ClearItem();
        UIManagerHome.Instance.Open(PopupType.CAMPAIGN);
    }
}
