using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GearPanelUI : MonoBehaviour
{
    //List<EquipmentData> equipmentDatas;
    SlotGearUI[] slots;
    [SerializeField] TextMeshProUGUI tmpAttack;
    [SerializeField] TextMeshProUGUI tmpHitPoint;
    MainCharacterGearUI mainCharacterGear;
    private void Awake()
    {
        slots = this.transform.GetComponentsInChildren<SlotGearUI>();
        mainCharacterGear = GetComponentInChildren<MainCharacterGearUI>();
    }
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.CHANGE_EQUIPMENT, OnChangeEquipment);
        EventDispatcher.Instance.RegisterListener(EventID.ENCHANT_LEVEL_EQUIPMENT, OnChangeLevelEquipment);
        EventDispatcher.Instance.RegisterListener(EventID.MERGE_EQUIPMENT, OnMergeEquipment);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.CHANGE_EQUIPMENT, OnChangeEquipment);
        EventDispatcher.Instance.RemoveListener(EventID.ENCHANT_LEVEL_EQUIPMENT, OnChangeLevelEquipment);
        EventDispatcher.Instance.RemoveListener(EventID.MERGE_EQUIPMENT, OnMergeEquipment);

    }

    private void OnMergeEquipment(Component arg1, object arg2)
    {
        //mainCharacterGear.UpdateWeapon();
        UpdateStatUI();
        UpdateGearUI();
    }

    private void OnChangeLevelEquipment(Component arg1, object arg2)
    {
        //mainCharacterGear.UpdateWeapon();
        UpdateStatUI();
    }
    private void OnChangeEquipment(Component arg1, object arg2)
    {
        //mainCharacterGear.UpdateWeapon();
        UpdateStatUI();
        UpdateGearUI();
    }
    public void Init()
    {
        //mainCharacterGear.UpdateWeapon();
        UpdateStatUI();
        UpdateGearUI();
    }
    private void UpdateStatUI()
    {
        var playerStat = GameData.Instance.playerData.tempData.PlayerTotalStat;
        tmpAttack.text = playerStat.atk.ToShortString();
        tmpHitPoint.text = playerStat.hp.ToShortString();
    }
    private void UpdateGearUI()
    {
        var userEquipments = GameData.Instance.playerData.saveData.ListEquipped;

        foreach (var slot in slots)
        {
            bool isEquip = false;
            foreach (var userEquipment in userEquipments)
            {
                var equipData = GameData.Instance.staticData.GetEquipmentData(userEquipment.itemId) as EquipmentData;
                if (slot.type == equipData.slot)
                {
                    slot.Init(userEquipment);
                    isEquip = true;
                }
            }
            if (!isEquip)
            {
                slot.Init(null);
            }
        }
    }
}
