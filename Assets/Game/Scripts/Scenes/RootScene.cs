using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_OPEN_LOADING);
        UpdateLanguage();
        //I2.Loc.LocalizationManager.CurrentLanguageCode = "vi";
        Vibration.Init();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
    private void UpdateLanguage() {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                I2.Loc.LocalizationManager.CurrentLanguageCode = "en";
                break;
            case SystemLanguage.Vietnamese:
                I2.Loc.LocalizationManager.CurrentLanguageCode = "vi";
                break;
            default:
                I2.Loc.LocalizationManager.CurrentLanguageCode = "en";
                break;
        }
    }
}
