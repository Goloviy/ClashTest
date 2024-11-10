using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum POPUP_UI_TYPE
{
    NONE,
    HOME

}
public class ManagerUI : Singleton<ManagerUI>
{
    [SerializeField] protected List<BaseUI> popups;
    protected Stack<POPUP_UI_TYPE> popupsActived;
    protected virtual void Awake()
    {
        popups = GetComponentsInChildren<BaseUI>(true).ToList();
        foreach (var popup in popups)
        {
            popup.Init(this);
        }
        popupsActived = new Stack<POPUP_UI_TYPE>();
        OpenDefaultPopup();
    }
    public virtual void OpenDefaultPopup()
    {
        OpenPopup(POPUP_UI_TYPE.HOME, false);

    }
    public virtual void OpenPopup(POPUP_UI_TYPE type, bool isStack)
    {
        if (!isStack)
        {
            HideAllPopup();
        }
        OpenPopup(type);
    }
    protected virtual void HideAllPopup()
    {
        foreach (var popup in popups)
        {
            popup.Close();
        }
        popupsActived.Clear();
    }
    protected virtual void OpenPopup(POPUP_UI_TYPE type)
    {
        popupsActived.Push(type);
        foreach (var popup in popups)
        {
            if (popup.TypePopup == type)
            {
                popup.Open();
            }
        }
    }
}
