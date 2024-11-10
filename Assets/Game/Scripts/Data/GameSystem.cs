using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : Singleton<GameSystem>
{
    public EquipmentSystem equipmentSystem;

    private void Awake()
    {
        equipmentSystem = new EquipmentSystem();
    }

    #region SHOP
    public UserEquipment[] OpenGacha(GachaType gachaType, int count)
    {
        UserEquipment[] listCollected = new UserEquipment[count];
        for (int i = 0; i < count; i++)
        {
            var GachaDataBox = GameData.Instance.staticData.gachaData.GetSystemData(gachaType);
            var totalPoint = 0;
            foreach (var rarityGachaData in GachaDataBox.rarityData)
            {
                totalPoint += rarityGachaData.point;
            }
            int rdPoint = UnityEngine.Random.Range(0, totalPoint);
            RarityGachaData itemGachaSystem = null;
            foreach (var itemSystemData in GachaDataBox.rarityData)
            {
                rdPoint -= itemSystemData.point;
                if (rdPoint < 0)
                {
                    itemGachaSystem = itemSystemData;
                    break;
                }
            }
            if (itemGachaSystem != null)
            {
                DebugCustom.Log("Open Gacha Success :" + gachaType);
                DebugCustom.Log("Collect Rarity :" + itemGachaSystem.rarity);
                var collectRarity = itemGachaSystem.rarity;
                bool isCollectSuperItem = false;
                if (GachaDataBox.isDropSuperItem && collectRarity >= GachaDataBox.rarityDropSuper)
                {
                    totalPoint = GachaDataBox.pointNormal + GachaDataBox.pointSuper;
                    rdPoint = UnityEngine.Random.Range(0, totalPoint);
                    if (rdPoint - GachaDataBox.pointNormal < 0)
                        isCollectSuperItem = false;
                    else
                        isCollectSuperItem = true;
                }
                EquipmentData[] listRandom = isCollectSuperItem ? GachaDataBox.superEquipments : GachaDataBox.equipments;
                var rdItem = UnityEngine.Random.Range(0, listRandom.Length);
                var CollectEquipment = listRandom[rdItem];
                var newUserEquipment = new UserEquipment(CollectEquipment.id, collectRarity, 1);
                listCollected[i] = newUserEquipment;
                GameData.Instance.playerData.saveData.AddEquipmentToBag(CollectEquipment.id, collectRarity);
            }
            else
            {
                DebugCustom.LogError("Gacha data error : Check rarity data point");
            }
        }
        GameDynamicData.cachedUserEquipmentCollected = listCollected;
        return listCollected;
    }

    public ChapterLevelData GetChapterDataByProgressCollectReward()
    {
        var idChapterReward = GameData.Instance.playerData.saveData.CollectChapterChestProgress.ToRewardChapter();
        idChapterReward = idChapterReward > GameData.Instance.playerData.saveData.BestChapterLevel ?
            GameData.Instance.playerData.saveData.BestChapterLevel :
            idChapterReward;
        var chapterLevelData = GameData.Instance.staticData.GetChapterLevel(idChapterReward);
        return chapterLevelData;
    }

    #endregion SHOP

    #region GAME MODE
    public void StartPlayMode(GameMode mode, int level)
    {
        switch (mode)
        {
            case GameMode.NONE:
                break;
            case GameMode.CAMPAIGN:
                PlayCampaign(level);
                break;
            case GameMode.DAILY_QUEST:
                PlayChallenge(level);
                break;
            case GameMode.CHALLENGES:
                break;
            default:
                break;
        }

    }
    private void PlayChallenge(int level)
    {
        var playerStamina = GameData.Instance.playerData.GetCurrencyValue(Currency.STAMINA);
        var modeData = GameData.Instance.staticData.modeData.GetData(GameMode.CHALLENGES);
        int requireStamina = modeData.StaminaRequire;
        //if (playerStamina >= requireStamina)
        //{
        //    GameData.Instance.playerData.AddCurrency(Currency.STAMINA, -requireStamina);
        //    if (GameData.Instance.playerData.saveData.ChapterLevel <= GameConfigData.Instance.MaxChapter)
        //    {
        //        GameData.Instance.playerData.tempData.Clear();
        //        SceneManager.LoadSceneAsync("GamePlay");
        //    }
        //}
    }
    private void PlayCampaign(int level)
    {

        var playerStamina = GameData.Instance.playerData.GetCurrencyValue(Currency.STAMINA);
        var modeData = GameData.Instance.staticData.modeData.GetData(GameMode.CAMPAIGN);
        int requireStamina = modeData.StaminaRequire;
        if (playerStamina >= requireStamina)
        {
            if (level <= GameConfigData.Instance.MaxChapter)
            {
                GameDynamicData.CurGameMode = GameMode.CAMPAIGN;
                GameDynamicData.SelectChapterLevel = level;
                GameData.Instance.playerData.AddCurrency(Currency.STAMINA, -requireStamina);
                GameData.Instance.playerData.tempData.Clear();
                SceneManager.LoadSceneAsync("GamePlay");
            }
        }

    }
    
    #endregion GAME MODE
}
