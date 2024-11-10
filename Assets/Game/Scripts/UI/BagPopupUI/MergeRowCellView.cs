using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeRowCellView : EnhancedScrollerCellView
{
    [SerializeField] ItemMergeUI[] rowCellViews;
    /// <summary>
    /// This function just takes the Demo data and displays it
    /// </summary>
    /// <param name="data"></param>
    public ItemMergeUI[] SetData(ref SmallList<UserItem> data, int startingIndex,Action<string> OnClickItem)
    {
        // loop through the sub cells to display their data (or disable them if they are outside the bounds of the data)
        for (var i = 0; i < rowCellViews.Length; i++)
        {
            // if the sub cell is outside the bounds of the data, we pass null to the sub cell
            rowCellViews[i].Init(startingIndex + i < data.Count ? data[startingIndex + i] : null, OnClickItem);
        }
        return rowCellViews;
    }
}
