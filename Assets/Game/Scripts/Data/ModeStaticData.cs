using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModeStaticData 
{
    GameModeData[] modes;
    Dictionary<GameMode, GameModeData> dictChapter;
    public ModeStaticData()
    {
        modes = Resources.LoadAll<GameModeData>(StringConst.PATH_GAME_MODE_DATA);
        dictChapter = new Dictionary<GameMode, GameModeData>();
        foreach (var modeData in modes)
        {
            dictChapter.Add(modeData.Mode, modeData);
        }
        Array.Clear(modes, 0, modes.Length);
    }
    public GameModeData GetData(GameMode mode)
    {
        GameModeData data = null;
        if (!dictChapter.TryGetValue(mode, out data))
        {
            DebugCustom.LogError("Level doesn't available :" + mode);
        }
        return data;
    }
}
