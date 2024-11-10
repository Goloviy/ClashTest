using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChapterLevelStaticData : MonoBehaviour
{
    ChapterLevelData[] chapters;
    public ChapterLevelData[] Chapters => chapters;
    Dictionary<int, ChapterLevelData> dictChapter;
    public ChapterLevelStaticData()
    {
        chapters = Resources.LoadAll<ChapterLevelData>(StringConst.PATH_CHAPTERS_DATA);
        dictChapter = new Dictionary<int, ChapterLevelData>();
        foreach (var rarity in chapters)
        {
            dictChapter.Add(rarity.level, rarity);
        }
        //Array.Clear(chapters, 0, chapters.Length);
    }
    public ChapterLevelData GetData(int level)
    {
        ChapterLevelData data = null;
        if (!dictChapter.TryGetValue(level, out data))
        {
            DebugCustom.LogError("Level doesn't available :" + level);
        }
        return data;
    }
}
