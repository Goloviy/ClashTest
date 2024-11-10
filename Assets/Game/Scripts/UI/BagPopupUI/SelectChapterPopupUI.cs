using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectChapterPopupUI : PopupUI
{
    [SerializeField] ChapterSlotController slotController;
    [SerializeField] TextMeshProUGUI tmpTitle;
    [SerializeField] TextMeshProUGUI tmpDescriptions;
    [SerializeField] Button btnApply;
    int selectDataIndex = 0;
    private ChapterLevelData[] passChapters;
    protected override void Awake()
    {
        base.Awake();
        slotController.scroller.scrollerSnapped = OnSnapped;
        slotController.scroller.scrollerScrolled = OnScrolled;
        btnApply.onClick.AddListener(OnClickApply);
    }

    
    private void OnClickApply()
    {
        SoundController.Instance.PlaySound(SOUND_TYPE.UI_BUTTON_CLICK);

        EventDispatcher.Instance.PostEvent(EventID.CHANGE_CHAPTER_LEVEL);
        UIManagerHome.Instance.Back();
    }
    private async void Start()
    {
        slotController.Reload(GameData.Instance.staticData.levelsData.Chapters, GameData.Instance.playerData.saveData.BestChapterLevel);
        selectDataIndex = GameDynamicData.SelectChapterLevel - 1;
        await Task.Delay(300);
        if (selectDataIndex < GameData.Instance.staticData.levelsData.Chapters.Length && selectDataIndex >= 0)
        {
            slotController.scroller.JumpToDataIndex(selectDataIndex);
        }
        else
        {
            GameDynamicData.SelectChapterLevel = 1;
            selectDataIndex = 0;
            slotController.scroller.JumpToDataIndex(selectDataIndex);
        }
        //slotController.scroller.snapping = true;
        LoadUIFirst(GameDynamicData.SelectChapterLevel);
    }

    private void LoadUIFirst(int selectChapterLevel)
    {
        var selectChapterData = GameData.Instance.staticData.levelsData.Chapters[selectChapterLevel - 1 < 0 ? 0 : selectChapterLevel - 1];
        //string chapterTitle = I2.Loc.LocalizationManager.GetTranslation(selectChapterData.title);
        tmpTitle.text = selectChapterData.title;
        //string chapterDes = I2.Loc.LocalizationManager.GetTranslation(selectChapterData.desKey);
        tmpDescriptions.text = selectChapterData.des;
    }

    private void OnSnapped(EnhancedScroller scroller, int cellIndex, int dataIndex, EnhancedScrollerCellView cellView)
    {
        if (dataIndex < GameData.Instance.staticData.levelsData.Chapters.Length && dataIndex >= 0)
        {
            selectDataIndex = dataIndex;
            GameDynamicData.SelectChapterLevel = selectDataIndex + 1;
            var selectChapterData = GameData.Instance.staticData.levelsData.Chapters[dataIndex];
            //string chapterTitle = I2.Loc.LocalizationManager.GetTranslation(selectChapterData.title);
            tmpTitle.text = selectChapterData.title;
            //string chapterDes = I2.Loc.LocalizationManager.GetTranslation(selectChapterData.des);
            tmpDescriptions.text = selectChapterData.des;
        }
        else
        {
            tmpTitle.text = String.Empty;
            tmpDescriptions.text = String.Empty;
        }
    }
    private void OnScrolled(EnhancedScroller scroller, Vector2 val, float scrollPosition)
    {
        
    }

}
