using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DesignData", menuName = "ScriptableObjects/DesignData", order = 1)]

public class SlotData : ScriptableObject
{
    public string title;
    public ItemType type;
    [PreviewField(80, ObjectFieldAlignment.Right)]
    public Sprite icon;
    public Currency designRequire = Currency.DESIGN_ARMOR;
}
