using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameDynamicData.IsSurviveSuccess = false;
        //AdManager.Instance.ShowBanner();
        SoundController.Instance.PlayMusic(MUSIC_BG.MUSIC_BG_2);
        GameDynamicData.IsAvailableRevive = true;
    }

}
