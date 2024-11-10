using EnhancedUI.EnhancedScroller;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour
{
    public string id;
    [SerializeField] protected TextMeshProUGUI tmpEnchantLevel;
    [SerializeField] protected TextMeshProUGUI tmpCount;
    [SerializeField] protected Image imgDesign;
    [SerializeField] protected Image imgIcon;
    [SerializeField] protected Image imgbg;
    [SerializeField] protected Button btn;
    [SerializeField] protected Sprite sprDefault;
    [SerializeField] protected Transform[] stars;
    public ItemType designType;
    protected ItemBagData itemData;
    public UserItem userItem;
    public UserEquipment userEquipment;

    protected const string LEVEL = "Lv.";
    protected const string COUNT = "x ";
    protected Action<string> OnClick;
    protected void Awake()
    {
        btn.onClick.AddListener(OnClickItem);
        AssignNewUID();
    }
    protected void AssignNewUID()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public void Init(UserItem _userItem, Action<string> OnClick = null)
    {
        imgIcon.enabled = true;
        imgbg.enabled = true;
        if (_userItem != null)
        {
            if (_userItem is UserEquipment _userEquipment)
                userEquipment = _userEquipment;
            else
                userEquipment = null;
            userItem = _userItem;

            itemData = GameData.Instance.staticData.GetEquipmentData(_userItem.itemId);
            this.OnClick = OnClick;
            LoadItemUI();
        }
        else
        {
            this.userItem = null;
            this.userEquipment = null;
            LoadEmptyUI();
        }
    }
    public void Init(Tuple<Sprite, Sprite, int> data)
    {
        userEquipment = null;
        imgIcon.overrideSprite = data.Item1;
        imgbg.overrideSprite = data.Item2;
        tmpCount.gameObject.SetActive(true);
        tmpCount.text = data.Item3.ToString();
        imgDesign.gameObject.SetActive(false);
        UpdateEnchantUI();
        UpdateUpgradeUI();
    }
    protected void UpdateUpgradeUI()
    {
        if (userEquipment == null)
        {
            foreach (var star in stars)
            {
                star.gameObject.SetActive(false);
            }
        }
        else
        {
            var rarityData = GameData.Instance.staticData.GetRarity(userEquipment.rarity);
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(i < rarityData.upgradeLevel);
            }
        }
    }
    protected void UpdateEnchantUI()
    {
        if (userEquipment == null)
        {

            tmpEnchantLevel.gameObject.SetActive(false);
        }
        else
        {

            tmpEnchantLevel.gameObject.SetActive(true);
            //level enchant
            StringBuilder builder = new StringBuilder();
            builder.Append(LEVEL);
            builder.Append(userEquipment.enchantLevel);
            tmpEnchantLevel.text = builder.ToString();
            tmpEnchantLevel.gameObject.SetActive(userEquipment != null);
        }


    }
    protected void UpdateNumberItemUI()
    {
        if (userEquipment != null || userItem == null)
        {
            tmpCount.gameObject.SetActive(false);
            return;
        }
        tmpCount.gameObject.SetActive(true);
        //count item
        StringBuilder builder = new StringBuilder();
        builder.Clear();
        builder.Append(COUNT);
        builder.Append(userItem.count);
        tmpCount.text = builder.ToString();
    }
    protected virtual void LoadEmptyUI()
    {
        imgIcon.enabled = false;
        imgbg.enabled = true;
        imgbg.overrideSprite = GameData.Instance.staticData.GetRarity(Rarity.Common).border;
        imgDesign.gameObject.SetActive(false);
        UpdateUpgradeUI();
        UpdateEnchantUI();
        UpdateNumberItemUI();
    }
    
    protected virtual void LoadItemUI()
    {
        if (userEquipment != null)
        {
            //load design icon
            var designData = GameData.Instance.staticData.GetSlot(itemData.slot);
            imgDesign.gameObject.SetActive(true);
            imgDesign.overrideSprite = designData.icon;
        }
        else
        {
            imgDesign.gameObject.SetActive(false);

        }
        int index = 0;
        if (itemData.spriteIcons.Length > (int)userItem.rarity)
            index = (int)userItem.rarity;
        imgIcon.overrideSprite = itemData.spriteIcons[index];
        RarityData rarityData = GameData.Instance.staticData.GetRarity(userItem.rarity);
        //imgbg.color = rarityData.color;
        imgbg.overrideSprite = rarityData.border;

        UpdateEnchantUI();
        UpdateUpgradeUI();
        UpdateNumberItemUI();
    }
    protected virtual void OnClickItem()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (userItem == null)
        {
            return;
        }
        else
        {
            if (userEquipment != null)
            {
                GameDynamicData.SelectedEquipment = userEquipment;
                GameDynamicData.SelectedGearCanEquip = true;
                UIManagerHome.Instance.Open(PopupType.EQUIP_INFOR, true);
            }
            else
            {
                GameDynamicData.SelectedItem = userItem;
                UIManagerHome.Instance.Open(PopupType.ITEM_INFOR, true);
            }
        }
    }
}
