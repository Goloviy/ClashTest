using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem 
{
    public void EnchantEquipment(UserEquipment userEquipment, bool isAuto)
    {
        var equipmentData = GameData.Instance.staticData.GetEquipmentData(userEquipment.itemId);
        bool isEnoughGold = false;
        bool isEnoughDesign = false;
        bool isMaxLevel = true;
        do
        {
            int nextLevel = userEquipment.enchantLevel + 1;
            int requireGold = ItemHelper.CalculateEnchantRequireGold(nextLevel, equipmentData.slot);
            int requireDesign = ItemHelper.CalculateEnchantRequireDesign(nextLevel, equipmentData.slot);
            long pGold = GameData.Instance.playerData.GetCurrencyValue( Currency.GOLD);
            long pDesign = GameData.Instance.playerData.GetCurrencyValue(equipmentData.slot.ToCurrency());

            var rarityData = GameData.Instance.staticData.GetRarity(userEquipment.rarity);
            int levelMax = rarityData.enchantMaxLevel;
            int curLevelEnchant = userEquipment.enchantLevel;
            isEnoughGold = pGold >= requireGold;
            isEnoughDesign = pDesign >= requireDesign;
            isMaxLevel = curLevelEnchant >= levelMax;
            if (isEnoughGold && isEnoughDesign && !isMaxLevel)
            {
                GameData.Instance.playerData.AddCurrency(Currency.GOLD, -requireGold);
                GameData.Instance.playerData.AddCurrency(equipmentData.slot.ToCurrency(), -requireDesign);
                //var itemDesign = GameData.Instance.staticData.items.GetDataItemDesign(equipmentData.slot);
                //GameData.Instance.playerData.saveData.RemoveDesignInBag(equipmentData.slot, requireDesign);
                GameData.Instance.playerData.saveData.IsDirty = true;
                userEquipment.enchantLevel++;

            }
            else
            {
                DebugCustom.Log("End Auto Upgrade Level");
                break;
            }
        } while (isAuto);
        //tinh toan lai equipments stat
        CalculateEquipmentStats();
        EventDispatcher.Instance.PostEvent(EventID.ENCHANT_LEVEL_EQUIPMENT);
    }

    public void CalculateEquipmentStats()
    {
        List<GradeSkillData> listGradeSkill = new List<GradeSkillData>();
        var baseStat = GameData.Instance.playerData.tempData.baseStat;
        var equipStat = GameData.Instance.playerData.tempData.equipStat;
        var buffStat = GameData.Instance.playerData.tempData.buffStat;
        //List<SkillName> passiveSkills = GameData.Instance.playerData.tempData.equipSkills;
        List<SkillName> gradeSkills = GameData.Instance.playerData.tempData.equipBonusSkills;
        GameData.Instance.playerData.tempData.equipSkills = new List<SkillName>();
        //passiveSkills.Clear();
        gradeSkills.Clear();
        equipStat.atk = 0;
        equipStat.hp = 0;
        equipStat.def = 0;
        equipStat.critRate = 0;
        equipStat.critDamage = 0;
        equipStat.moveSpeed = 0;
        buffStat.atk = 0;
        buffStat.hp = 0;
        buffStat.def = 0;
        buffStat.critDamage = 0;
        buffStat.critRate = 0;
        buffStat.moveSpeed = 0;
        //apply equip stat
        foreach (var equipped in GameData.Instance.playerData.saveData.ListEquipped)
        {
            var equipmentData = GameData.Instance.staticData.GetEquipmentData(equipped.itemId) as EquipmentData;
            var rarityData = GameData.Instance.staticData.GetRarity(equipped.rarity);
            var equipAtk = ItemHelper.CalculateStat(equipmentData.atkBase, rarityData.rarityK, equipped.enchantLevel, equipmentData.slot);
            var equipHp = ItemHelper.CalculateStat(equipmentData.hpBase, rarityData.rarityK, equipped.enchantLevel, equipmentData.slot);
            //var equipDef = ItemHelper.CalculateStat(equipmentData.defBase, rarityData.rarityK, equipped.enchantLevel, equipmentData.slot);
            //var equipCritRate = ItemHelper.CalculateStat(equipmentData.critRateBase, rarityData.rarityK, equipped.enchantLevel, equipmentData.slot);
            //var equipCritDamage = ItemHelper.CalculateStat(equipmentData.critDamageBase, rarityData.rarityK, equipped.enchantLevel, equipmentData.slot);
            equipStat.atk += equipAtk;
            equipStat.hp += equipHp;
            //equipStat.def += equipDef;
            //equipStat.critRate += equipCritRate;
            //equipStat.critDamage += equipCritDamage;
            foreach (var gradeSkill in equipmentData.gradeSkills)
            {
                if ((int)gradeSkill.rarity <= (int)equipped.rarity)
                {
                    listGradeSkill.Add(gradeSkill);
                }
            }
            if (equipmentData.passiveSkill != SkillName.NONE)
            {
                GameData.Instance.playerData.tempData.equipSkills.Add(equipmentData.passiveSkill);
            }
            //passiveSkills.Add(equipmentData.passiveSkill);
        }
        //apply skill buff stat
        foreach (var gradeSkill in listGradeSkill)
        {
            if (gradeSkill.gradeSkill == GradeSkill.INCREASE_ATK)
            {
                buffStat.atk += Mathf.RoundToInt((equipStat.atk + baseStat.atk) * (gradeSkill.value / 100f));
            }
            else if (gradeSkill.gradeSkill == GradeSkill.INCREASE_HP)
            {
                buffStat.hp += Mathf.RoundToInt((equipStat.hp + baseStat.hp) * (gradeSkill.value / 100f));
            }
            else if (gradeSkill.gradeSkill == GradeSkill.INCREASE_DEFENSE)
            {
                buffStat.def += Mathf.RoundToInt((equipStat.def + baseStat.def) * (gradeSkill.value / 100f));
            }
            else if (gradeSkill.gradeSkill == GradeSkill.INCREASE_CRIT_RATE)
            {
                buffStat.critRate += Mathf.RoundToInt(equipStat.critRate + baseStat.critRate + gradeSkill.value / 100f);
            }
            else if (gradeSkill.gradeSkill == GradeSkill.INCREASE_CRIT_DAMAGE)
            {
                buffStat.critDamage += Mathf.RoundToInt(equipStat.critDamage + baseStat.critDamage + gradeSkill.value / 100f);
            }
            else if (gradeSkill.gradeSkill == GradeSkill.INCREASE_MOVE_SPEED)
            {
                buffStat.moveSpeed += Mathf.RoundToInt((equipStat.moveSpeed + baseStat.moveSpeed) * (gradeSkill.value / 100f));
            }
            else if (gradeSkill.gradeSkill == GradeSkill.LEARN_ONE_SKILL && gradeSkill.bonusSkill != SkillName.NONE)
            {
                gradeSkills.Add(gradeSkill.bonusSkill);
            }
        }
    }
}
