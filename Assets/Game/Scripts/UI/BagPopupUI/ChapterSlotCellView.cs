using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSlotCellView : EnhancedScrollerCellView
{
    /// <summary>
    /// These are the UI elements that will be updated when the data changes
    /// </summary>
    public Image slotImage;

    /// <summary>
    /// This function sets up the data for the cell view
    /// </summary>
    /// <param name="data">The data to use</param>
    public void SetData(ChapterSlotData data)
    {
        // update the cell view's UI
        if (data.sprite == null)
        {
            // this is a blank slot, so set the background color to no alpha
            slotImage.color = new Color(0, 0, 0, 0);
        }
        else
        {
            // this slot has an image so set its sprite
            slotImage.sprite = data.sprite;
            slotImage.color = Color.white;
        }
    }
}
