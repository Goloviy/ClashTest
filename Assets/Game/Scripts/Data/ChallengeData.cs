using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ChallengeData", menuName = "ScriptableObjects/Game/ChallengeData", order = 1)]

public class ChallengeData : BaseScriptObject
{
    public int Level = 1;
    [PreviewField( alignment: ObjectFieldAlignment.Left)]
    public Sprite Icon;
    public string Title = "";
    public int RequireUnlockChapterLv = 2;
    public ChallengeLevel Difficult = ChallengeLevel.HARD;
    public CurrencyRewardItem[] CurrencyRewards; 
}
