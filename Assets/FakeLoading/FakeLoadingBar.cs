using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FakeLoadingBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image imgForeGround;
    [SerializeField] TextMeshProUGUI tmpProcessing;

    [Header("Setting")]
    [SerializeField] float timeLoading = 4f;
    float timeRemain;
    [SerializeField] string sceneNameNext = "Home";
    float startFill = 0.15f;
    bool isProcess = false;
    const string STR_PERCENT = "%";
    private void Start()
    {
        timeRemain = timeLoading;
        imgForeGround.fillAmount = startFill;
        isProcess = true;
    }
    private void Update()
    {
        if (!isProcess)
        {
            return;
        }
        if (timeRemain > 0)
        {
            timeRemain -= Time.deltaTime;
            var fillValue = (timeLoading - timeRemain) / timeLoading;
            fillValue = fillValue > startFill ? fillValue : startFill;
            fillValue = Mathf.Clamp01(fillValue);
            imgForeGround.fillAmount = fillValue;
            var percent = Mathf.RoundToInt( fillValue / 0.01f);
            tmpProcessing.text = string.Concat(percent, STR_PERCENT);
        }
        else
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        isProcess = false;
        SceneManager.LoadSceneAsync(sceneNameNext);
    }
}
