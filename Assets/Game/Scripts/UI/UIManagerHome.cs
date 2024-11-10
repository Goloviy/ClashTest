using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManagerHome : Singleton<UIManagerHome>
{
    Dictionary<PopupType, PopupUI> dicts;
    PopupType curPopup;
    PopupType lastPopup = PopupType.NONE;
    PopupType defaulPopup = PopupType.HOME;
    PopupType campaignPopup = PopupType.CAMPAIGN;
    private void Awake()
    {
        dicts = new Dictionary<PopupType, PopupUI>();
        List<PopupUI> popups = GetComponentsInChildren<PopupUI>(true).ToList();
        foreach (var popup in popups)
        {
            dicts.Add(popup.popupType, popup);
        }

    }
    private void Start()
    {
        Open(defaulPopup);
        Open(campaignPopup);
    }
    public void Open(PopupType type, bool isOverride = false)
    {
        if (curPopup == type)
        {
            return;
        }
        lastPopup = curPopup;
        curPopup = type;
        PopupUI popupUI;
        if (dicts.TryGetValue(curPopup, out popupUI))
            popupUI.Open();
        if (dicts.TryGetValue(lastPopup, out popupUI) && !isOverride)
            popupUI.Close();
    }
    public void Back()
    {
        if (lastPopup == PopupType.NONE)
        {
            return;
        }
        var temp = lastPopup;
        lastPopup = curPopup;
        curPopup = temp;
        PopupUI popupUI;
        if (dicts.TryGetValue(curPopup, out popupUI))
            popupUI.Open();
        if (dicts.TryGetValue(lastPopup, out popupUI))
            popupUI.Close();
    }
}
