using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanUpLevel 
{
    int CurrentExp { get; }
    int CurrentLevel { get; }
    int NextLevelExp { get; }
}
