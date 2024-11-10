using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSlotController : MonoBehaviour, IEnhancedScrollerDelegate
{
    private SmallList<ChapterSlotData> _data;
    /// <summary>
    /// The slot cell view prefab to use in the scroller
    /// </summary>
    public EnhancedScrollerCellView slotCellViewPrefab;

    /// <summary>
    /// The scroller that will display the slot cells
    /// </summary>
    public EnhancedScroller scroller;
    int totalChapter = 1;
    [SerializeField] float sizeElement = 500;
    void Awake()
    {
        // create a new data list for the slots
        _data = new SmallList<ChapterSlotData>();
    }

    void Start()
    {
        // set this controller as the scroller's delegate
        scroller.Delegate = this;
    }

    public void Reload(ChapterLevelData[] chapterData, int bestChapterLevel = 1)
    {
        // reset the data list
        _data.Clear();
        totalChapter = bestChapterLevel;
        int i = 0;
        // at the sprites from the demo script to this scroller's data cells
        foreach (var chapter in chapterData)
        {
            if (i < bestChapterLevel)
            {
                _data.Add(new ChapterSlotData() { sprite = chapter.icon, level = chapter.level });
                i++;
            }
            else
                break;

        }

        // reload the scroller
        scroller.ReloadData();
    }

    #region EnhancedScroller Callbacks

    /// <summary>
    /// This callback tells the scroller how many slot cells to expect
    /// </summary>
    /// <param name="scroller">The scroller requesting the number of cells</param>
    /// <returns>The number of cells</returns>
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return totalChapter;
    }

    /// <summary>
    /// This callback tells the scroller what size each cell is.
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell size</param>
    /// <param name="dataIndex">The index of the data list</param>
    /// <returns>The size of the cell (Height for vertical scrollers, Width for Horizontal scrollers)</returns>
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return sizeElement;
    }

    /// <summary>
    /// This callback gets the cell to be displayed by the scroller
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell</param>
    /// <param name="dataIndex">The index of the data list</param>
    /// <param name="cellIndex">The cell index (This will be different from dataindex if looping is involved)</param>
    /// <returns>The cell to display</returns>
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // get the cell view from the scroller, recycling if possible
        ChapterSlotCellView cellView = scroller.GetCellView(slotCellViewPrefab) as ChapterSlotCellView;

        // set the data for the cell
        cellView.SetData(_data[dataIndex]);

        // return the cell view to the scroller
        return cellView;
    }

    #endregion
}

public class ChapterSlotData
{
    /// <summary>
    /// The preloaded sprite for the slot cell. 
    /// We could have loaded the sprite while scrolling,
    /// but since there are so few slot cell types, we'll
    /// just preload them to speed up the in-game processing.
    /// </summary>
    public Sprite sprite;
    public int level;
}