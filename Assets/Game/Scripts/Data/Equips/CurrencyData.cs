using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyData", menuName = "ScriptableObjects/CurrencyData", order = 1)]

public class CurrencyData : BaseScriptObject
{
    public string title = "Currency/";
    public Currency currency;
    [PreviewField(80, ObjectFieldAlignment.Right)]
    public Sprite icon;
    public Rarity rarity;
    //public string descriptions;
    public bool isShowInBag = false;
    [ShowIf("isShowInBag", true)]
    [Tooltip("id Model get from ItemBagData-Id")]
    public string modelId;
}
