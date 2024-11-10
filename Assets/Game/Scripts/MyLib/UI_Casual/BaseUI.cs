using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    POPUP_UI_TYPE popupType;
    public void Init(ManagerUI manager)
    {

    }
    public POPUP_UI_TYPE TypePopup => popupType;
    public void Close()
    {

    }
    public void Open()
    {

    }
}
