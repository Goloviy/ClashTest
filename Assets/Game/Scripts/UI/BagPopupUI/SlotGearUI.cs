using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotGearUI : MonoBehaviour
{
    public ItemType type;
    public ItemInventory itemUI;
    public bool isEmpty = true;
    UserEquipment userEquipment;
    Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
    public void Init(UserEquipment userEquipment)
    {
        this.userEquipment = userEquipment;
        itemUI.Init(userEquipment);
        isEmpty = userEquipment == null;

        itemUI.gameObject.SetActive(userEquipment != null);

    }
    protected virtual void OnClick()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (isEmpty)
        {
            return;
        }
        else
        {
            if (userEquipment != null)
            {
                GameDynamicData.SelectedEquipment = userEquipment;
                GameDynamicData.SelectedGearCanEquip = false;
                UIManagerHome.Instance.Open(PopupType.EQUIP_INFOR, true);
            }
        }
    }
}
