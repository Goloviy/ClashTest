using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData 
{
    public PlayerTempData tempData;
    public PlayerSaveData saveData;
    
    public PlayerData()
    {
        tempData = new PlayerTempData();
        saveData = new PlayerSaveData();
    }
    
    public bool RefreshSkillAvailable => tempData.RefreshSkillAvailable;
    public void RefreshSkill() => tempData.RefreshSkill();
    public void LearnSkill(SkillName type)
    {
        tempData.LearnSkill(type);
    }
    public int GetLevelSkill(SkillName type) => tempData.GetLevelSkill(type);
    
    public bool CheckEnoughCurrency(Currency currency, long number)
    {
        //if (currency == Currency.GEM)
        //{
        //    long gem = saveData.Gem;
        //    return gem >= number;
        //}
        //else if (currency == Currency.GOLD)
        //{
        //    long gold = saveData.Gold;
        //    return gold >= number;
        //}
        //else
        //{
            return saveData.GetNumber(currency) >= number;
        //}
    }
    public long GetCurrencyValue(Currency currency)
    {
        return saveData.GetNumber(currency);
    }
    public void AddCurrency(Currency currency, long number)
    {
        //if (currency == Currency.GEM)
        //{
        //    saveData.ModifyGem(number);
        //}
        //else if (currency == Currency.GOLD)
        //{
        //    saveData.ModifyGold(number);
        //}
        //else 
        //{
            saveData.AddCurrency(currency, number);
        //}
    }
    public void Equip(UserEquipment userEquipment)
    {
        saveData.Equip(userEquipment);
    }
    public UserEquipment UnEquip(UserEquipment userEquipment)
    {
        return saveData.UnEquip(userEquipment);
    }

}
