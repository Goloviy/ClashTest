using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    protected IMananagerUI manager;
    public PopupType popupType;
    public Transform tfBlackPanel = null;
    public OpenPopupAnim OpenAnim;
    public ClosePopupAnim CloseAnim;

    protected Vector3 originScale;

    protected bool canSelect;

    protected virtual void Awake()
    {
    }
    public virtual void Init(IMananagerUI manager)
    {
        this.manager = manager;
    }
    public virtual void Open()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_OPEN_POPUP);
        originScale = Vector3.one;
        canSelect = false;
        if (tfBlackPanel)
            tfBlackPanel.gameObject.SetActive(true);
        if (OpenAnim == OpenPopupAnim.ZOOM_IN)
        {
            this.transform.localScale = originScale * 0.3f;
            this.gameObject.SetActive(true);
            this.transform.DOScale(originScale, 0.35f).SetUpdate(true).onComplete += () => { canSelect = true; };
        }
        else
        {
            canSelect = true;
            this.gameObject.SetActive(true);
        }
    }
    public virtual void Close()
    {
        if (!canSelect)
        {
            return;
        }
        canSelect = false;
        if (CloseAnim == ClosePopupAnim.ZOOM_OUT)
        {
            this.transform.DOScale(0.3f, 0.35f).SetUpdate(true).onComplete += () =>
            {
                if (tfBlackPanel)
                    tfBlackPanel.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            };

        }
        else
        {
            if (tfBlackPanel)
                tfBlackPanel.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
