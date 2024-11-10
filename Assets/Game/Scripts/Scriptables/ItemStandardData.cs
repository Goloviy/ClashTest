using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemStandardData", menuName = "ScriptableObjects/Shop/ItemStandardData", order = 1)]
public class ItemStandardData : ScriptableObject
{
    public int gradeLevel = 0;
    [PreviewField(50, ObjectFieldAlignment.Left)]
    public Sprite icon;
    public string title;
    public Currency currencyPrice;
    public int priceValue;
    public Currency currencyProduct;
    public int productValue;
}
