using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeStaticData 
{
    ChallengeData[] challenges;
    Dictionary<ChallengeLevel, ChallengeData> dictChapter;
    public ChallengeStaticData()
    {
        challenges = Resources.LoadAll<ChallengeData>(StringConst.PATH_GAME_MODE_DATA);
        dictChapter = new Dictionary<ChallengeLevel, ChallengeData>();
        foreach (var modeData in challenges)
        {
            dictChapter.Add(modeData.Difficult, modeData);
        }
        Array.Clear(challenges, 0, challenges.Length);
    }
    public ChallengeData GetData(ChallengeLevel mode)
    {
        ChallengeData data = null;
        if (!dictChapter.TryGetValue(mode, out data))
        {
            DebugCustom.LogError("Level doesn't available :" + mode);
        }
        return data;
    }
}
