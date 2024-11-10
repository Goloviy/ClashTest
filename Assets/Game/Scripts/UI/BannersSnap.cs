using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannersSnap : MonoBehaviour
{
    [SerializeField] int maxBanner = 3;
    float itemW = -500f;

    [SerializeField] Button btnLeft;
    [SerializeField] Button btnRight;
    [SerializeField] Transform tfContent;
    int curBanner = 0;
    private void Start()
    {
        itemW = -1 * Screen.currentResolution.width;
    }
    private void OnEnable()
    {
        btnLeft.onClick.AddListener(OnClickLeft);
        btnRight.onClick.AddListener(OnClickRight);
    }

    private void OnClickRight()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (curBanner < maxBanner - 1)
        {
            curBanner++;
        }
        UpdateScroll();
    }
    void UpdateScroll()
    {
        if (tfContent.transform is RectTransform rtf)
        {
           var newPos = rtf.position;
            newPos.x = curBanner * itemW;
            rtf.position = newPos;
        }
    }
    private void OnClickLeft()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        if (curBanner > 0)
        {
            curBanner--;
        }
        UpdateScroll();
    }

    private void OnDisable()
    {
        btnLeft.onClick.RemoveListener(OnClickLeft);
        btnRight.onClick.RemoveListener(OnClickRight);
    }
}
