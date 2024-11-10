using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertPanel : Singleton<AlertPanel>
{
    [SerializeField] TextMeshProUGUI txtNoticeTitle;
    [SerializeField] TextMeshProUGUI txtNoticeContent;
    [SerializeField] GameObject goNotice;
    [SerializeField] Button btnOk;
    [SerializeField] Button btnAccept;
    [SerializeField] Button btnCancel;
    [SerializeField] Button btnClose;
    const string STRING_NOTICE = "Notice";
    Action actionOk;
    Action actionClose;
    bool canSelect = true;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        btnOk.onClick.AddListener(OnClose);
        btnAccept.onClick.AddListener(OnAccept);
        btnCancel.onClick.AddListener(OnCancel);
        btnClose.onClick.AddListener(OnClose);
    }

    private void OnCancel()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_FAIL);
        if (!canSelect)
        {
            return;
        }
        actionClose?.Invoke();
        CloseNotice();
    }

    private void OnAccept()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);
        if (!canSelect)
        {
            return;
        }
        actionOk?.Invoke();
        CloseNotice();
    }

    private void OnClose()
    {
        if (!canSelect)
        {
            return;
        }
        actionClose?.Invoke();
        CloseNotice();
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

    }
    private void OpenAnim()
    {
        canSelect = false;
        var originScale = Vector3.one;
        this.transform.localScale = originScale * 0.3f;
        goNotice.SetActive(true);
        this.transform.DOScale(originScale, 0.35f).SetUpdate(true).onComplete += () => { canSelect = true; };
    }
    public void ShowNotice(string content, Action actionClose, string title = "")
    {
        OpenAnim();
        this.actionClose = actionClose;
        //SoundController.Instance.PlaySound(SOUND_TYPE.UI_CLICK);
        txtNoticeContent.text = content;
        txtNoticeTitle.text = title.Equals(String.Empty) ? STRING_NOTICE : title;
        btnAccept.gameObject.SetActive(false);
        btnCancel.gameObject.SetActive(false);
        btnOk.gameObject.SetActive(true);
        
    }
    public void ShowNotice(string content, Action actionOk, Action actionClose, string title = "")
    {

        OpenAnim();
        this.actionClose = actionClose;
        this.actionOk = actionOk;
        txtNoticeTitle.text = title.Equals("") ? STRING_NOTICE : title;
        //SoundController.Instance.PlaySound(SOUND_TYPE.UI_CLICK);
        txtNoticeContent.text = content;
        btnAccept.gameObject.SetActive(true);
        btnCancel.gameObject.SetActive(true);
        btnOk.gameObject.SetActive(false);
    }
    public void CloseNotice()
    {
        goNotice.SetActive(false);
        //SoundController.Instance.PlaySound(SOUND_TYPE.UI_CONFIRM);
    }
}
