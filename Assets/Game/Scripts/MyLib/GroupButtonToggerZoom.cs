using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ButtonTogger
{
    public Sprite spr;
    public Color colorText;

    public float btnWidth;
    public float btnHeight;

    public float iconWidth;
    public float iconHeight;
}

public class GroupButtonToggerZoom : MonoBehaviour
{
    [SerializeField] ButtonTogger btnHighlight;
    [SerializeField] ButtonTogger btnNormal;
    ButtonZoom[] buttons;
    [SerializeField] int[] indexLocks;
    [SerializeField] int indexDefaul = 2;
    int lastIndexHighlight = -1;
    bool isDisableClick = false;
    private void Awake()
    {
        lastIndexHighlight = indexDefaul;
        buttons = GetComponentsInChildren<ButtonZoom>();
    }
    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].index = index;
            buttons[i].onClick.AddListener(delegate { OnSelectButton(index); });
        }
        OnSelectButton(indexDefaul);
    }

    private void OnSelectButton(int index)
    {
        foreach (var _indexLock in indexLocks)
        {
            if (index == _indexLock)
            {
                DebugCustom.Log("Lock Button");
                return;
            }
        }
        ChangeButtonStatus(index);
    }
    private async void ChangeButtonStatus(int indexSelected)
    {
        if (isDisableClick)
        {
            return;
        }
        isDisableClick = true;
        var lastIndexHL = lastIndexHighlight;
        await ZoomOut(indexSelected, lastIndexHL);
        await ZoomIn(indexSelected, lastIndexHL);
        lastIndexHighlight = indexSelected;
        isDisableClick = false;
    }
    private async Task ZoomOut(int indexSelected, int lastIndexHL)
    {
        foreach (var button in buttons)
        {
            //unhighlight
            if (button.transform is RectTransform rtf)
            {
                if (lastIndexHL == button.index)
                {
                    //smootZoomOut
                    float deltaWidth = btnHighlight.btnWidth - btnNormal.btnWidth;
                    float deltaHeight = btnHighlight.btnHeight - btnNormal.btnHeight;
                    int frame = 8;
                    for (int i = 4; i <= frame; i++)
                    {
                        await Task.Delay(10);
                        float nextWidth = btnHighlight.btnWidth - deltaWidth / frame * i;
                        float nextHeight = btnHighlight.btnHeight - deltaHeight / frame * i;
                        //button.ChangeButtonContent(btnHighlight);
                        rtf.sizeDelta = new Vector2(nextWidth, nextHeight);

                    }
                    button.ChangeButtonContent(btnNormal);
                    rtf.sizeDelta = new Vector2(btnNormal.btnWidth, btnNormal.btnHeight);

                }

            }
        }

    }
    private async Task ZoomIn(int indexSelected, int lastIndexHL)
    {
        foreach (var button in buttons)
        {
            if (button.index == indexSelected)
            {
                //highlight button
                if (button.transform is RectTransform rtf)
                {
                    //smoothZoomIn
                    float deltaWidth = btnHighlight.btnWidth - btnNormal.btnWidth;
                    float deltaHeight = btnHighlight.btnHeight - btnNormal.btnHeight;
                    int frame = 8;
                    for (int i = 4; i <= frame; i++)
                    {
                        await Task.Delay(10);
                        float nextWidth = btnNormal.btnWidth + deltaWidth / frame * i;
                        float nextHeight = btnNormal.btnHeight + deltaHeight / frame * i;
                        //button.ChangeButtonContent(btnHighlight);
                        rtf.sizeDelta = new Vector2(nextWidth, nextHeight);
                    }

                    button.ChangeButtonContent(btnHighlight);
                    rtf.sizeDelta = new Vector2(btnHighlight.btnWidth, btnHighlight.btnHeight);
                }
            }
        }
    }
}
