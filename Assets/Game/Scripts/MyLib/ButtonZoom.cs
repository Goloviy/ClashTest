using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonZoom : Button
{
    Image _image;
    Transform tfIcon;
    TextMeshProUGUI tmpContent;
    public int index;
    protected override void Awake()
    {
        _image = GetComponent<Image>();
        try
        {
            tfIcon = this.transform.GetChild(0).GetComponentInChildren<Transform>();
            tmpContent = this.transform.GetComponentInChildren<TextMeshProUGUI>();
        }
        catch { }
    }
    public void ChangeButtonContent(ButtonTogger buttonTogger)
    {
        if (_image)
        {
            _image.overrideSprite = buttonTogger.spr;
        }
        if (buttonTogger.iconWidth > 0 && buttonTogger.iconHeight > 0)
        {
            if (tfIcon != null && tfIcon is RectTransform rtf)
            {
                rtf.sizeDelta = new Vector2(buttonTogger.iconWidth, buttonTogger.iconHeight);
            }
        }
        if (buttonTogger.colorText != new Color(0, 0, 0, 0))
        {
            if (tmpContent)
            {
                tmpContent.color = buttonTogger.colorText;
            }
        }
    }
}
