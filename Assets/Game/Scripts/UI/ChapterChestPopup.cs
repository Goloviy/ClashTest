using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterChestPopup : MonoBehaviour
{
    [SerializeField] Transform tfContent;
    [SerializeField] ItemChapterChest prefabItem;
    ChapterLevelData chapterLevelData;
    ItemChapterChest[] items;
    
    private void CreateItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            var newItem = Instantiate(prefabItem, tfContent);
            newItem.Init(chapterLevelData.level, chapterLevelData.chapterChests[i], OnChangeChapterReward);
            items[i] = newItem;
        }
    }

    private void OnChangeChapterReward()
    {
        ClearItems();
        GetDataChest();
        CreateItems();
    }
    private void GetDataChest()
    {
        //var idChapterReward = GameData.Instance.playerData.saveData.CollectChapterChestProgress.ToRewardChapter();
        //idChapterReward = idChapterReward > GameData.Instance.playerData.saveData.BestChapterLevel ?
        //    GameData.Instance.playerData.saveData.BestChapterLevel :
        //    idChapterReward;
        //chapterLevelData = GameData.Instance.staticData.GetChapterLevel(idChapterReward);
        chapterLevelData = GameSystem.Instance.GetChapterDataByProgressCollectReward();
        items = new ItemChapterChest[chapterLevelData.chapterChests.Length];

    }
    private void OnEnable()
    {
        GetDataChest();
        CreateItems();
    }
    private void OnDisable()
    {
        ClearItems();
    }
    private void ClearItems()
    {
        if (items != null)
        {
            foreach (var item in items)
            {
                GameObject.Destroy(item.gameObject);
            }
            Array.Clear(items, 0, items.Length);
        }
    }
}
