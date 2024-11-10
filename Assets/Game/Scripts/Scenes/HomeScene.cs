using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundController.Instance.PlayMusic(MUSIC_BG.MUSIC_BG_1);
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        GameDynamicData.PlayerPlayGame = false;
        GameSystem.Instance.equipmentSystem.CalculateEquipmentStats();
        //AdManager.Instance.HideBanner();
    }

    //private void OnApplicationPause(bool pauseStatus)
    //{
    //    if (!pauseStatus)
    //    {
    //        AppOpenAdManager.Instance.ShowAdIfAvailable();
    //    }
    //}
}