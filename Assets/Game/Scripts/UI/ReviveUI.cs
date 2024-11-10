using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviveUI : PopupUI
{
    int timeWait = 5;
    [SerializeField] TextMeshProUGUI tmpCounter;
    [SerializeField] Image imgFill;
    [SerializeField] Button btnPlayAds;
    bool isEndCounter = false;
    float valueFillStart = 0f;
    float valueFillEnd = 1f;
    Coroutine corouTimer;
    protected override void Awake()
    {
        base.Awake();
        btnPlayAds.onClick.AddListener(OnClickPlayAds);
    }
    private void OnEnable()
    {
        isEndCounter = false;
        btnPlayAds.gameObject.SetActive(true);
    }
    private void OnClickPlayAds()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        if (GameDynamicData.IsAvailableRevive)
        {
            isEndCounter = true;
            ViewAdsSuccess(1);
        }
        //ViewAdsSuccess();
    }

    private void ViewAdsSuccess(int point)
    {
        GameDynamicData.IsAvailableRevive = false;
        EventDispatcher.Instance.PostEvent(EventID.CHARACTER_REVIVE);
        Time.timeScale = 1;
        //StopCoroutine(corouTimer);
        Close();
    }
    private void ViewAdsFail()
    {
        Time.timeScale = 1;
        Close();
        EventDispatcher.Instance.PostEvent(EventID.GAME_OVER);
    }
    public override void Close()
    {
        base.Close();
        isEndCounter = true;
    }
    public override void Open()
    {
        base.Open();
        isEndCounter = false;
        imgFill.fillAmount = valueFillStart;
        tmpCounter.text = 3.ToString();
        CounterTime();
        imgFill.gameObject.SetActive(false);
        //corouTimer = StartCoroutine(CorouTimer());
    }

    IEnumerator CorouTimer()
    {
        int counter = timeWait;
        tmpCounter.text = "3";
        for (int i = 0; i < timeWait; i++)
        {
            counter--;
            if (counter <= 3 || counter >= 0)
            {
                tmpCounter.text = counter.ToString();
            }
            yield return new WaitForSeconds(1);
        }
        manager.Open(PopupType.EndGamePlayScene);
    }

    private async void CounterTime()
    {
        timeWait *= 1000;
        int currCounter = timeWait;
        int timePerFrame = 25;
        int totalFrame = Mathf.RoundToInt(timeWait / timePerFrame);
        for (int i = 0; i < totalFrame; i++)
        {
            await Task.Delay(timePerFrame);
            if (isEndCounter)
                return;
            currCounter-= timePerFrame;
            imgFill.fillAmount = (timeWait - currCounter)/ (float) timeWait;
            tmpCounter.text = (currCounter/1000).ToString();
        }
        imgFill.fillAmount = valueFillEnd;
        tmpCounter.text = 0.ToString();
        //open popup game over
        await Task.Delay(timePerFrame);
        if (isEndCounter)
            return;
        manager.Open(PopupType.EndGamePlayScene);

    }

    
}
