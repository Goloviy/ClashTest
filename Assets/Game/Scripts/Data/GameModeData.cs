using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ModeData", menuName = "ScriptableObjects/Game/ModeData", order = 1)]

public class GameModeData : BaseScriptObject
{
    public string Title = "";
    public int StaminaRequire = 5;
    public GameMode Mode;

}
