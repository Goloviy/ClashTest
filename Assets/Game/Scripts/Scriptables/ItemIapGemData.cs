using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemIapGemData", menuName = "ScriptableObjects/Shop/ItemIapGemData", order = 1)]

public class ItemIapGemData : ScriptableObject
{
    public string title;
    public int gradeLevel = 0;
    public string productId;
    [PreviewField(50, ObjectFieldAlignment.Left)]
    public Sprite spr;
    public int gem;
    
}
