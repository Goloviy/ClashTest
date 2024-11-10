using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMergeUI : ItemInventory
{
    [SerializeField] GameObject goAlert;
    [SerializeField] GameObject goEquipped;
    [SerializeField] GameObject goSelected;
    public bool CanMerge { get; private set; } = false;
    protected override void LoadItemUI()
    {
        goSelected.SetActive(false);
        base.LoadItemUI();

        bool isEquipped = GameData.Instance.playerData.saveData.IsEquipped(userEquipment);
        goEquipped.SetActive(isEquipped);
        CanMerge = ItemHelper.CheckMergeable(userEquipment);
        goAlert.SetActive(CanMerge);
    }
    protected override void LoadEmptyUI()
    {
        base.LoadEmptyUI();
        goAlert.gameObject.SetActive(false);
        goEquipped.gameObject.SetActive(false);

    }
    protected override void OnClickItem()
    {
        if (userItem == null)
        {
            return;
        }
        else
        {
            OnClick?.Invoke(id);
        }
    }
    public void Select()
    {
        goSelected.SetActive(true);
    }
    public void Unselect()
    {
        goSelected.SetActive(false);
    }
}
