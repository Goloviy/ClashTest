using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemBag", menuName = "ScriptableObjects/ItemBag", order = 1)]

public class ItemBagData : BaseScriptObject
{
    [BoxGroup("Information")]
    public string title;
    [BoxGroup("Information")]

    [PreviewField(80, ObjectFieldAlignment.Right)]
    public Sprite[] spriteIcons;
    [BoxGroup("Information")]
    [TextArea]
    public string descriptions;
    [BoxGroup("Information")]
    [EnumToggleButtons]
    public ItemType slot;
    [BoxGroup("Price")]
    public Currency currency;
    [BoxGroup("Price")]
    public int basePrice;
}
