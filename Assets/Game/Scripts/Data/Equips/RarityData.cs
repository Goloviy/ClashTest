using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RarityData", menuName = "ScriptableObjects/RarityData", order = 1)]

public class RarityData : ScriptableObject
{
    public string title;
    [PreviewField(80, ObjectFieldAlignment.Right)]
    public Sprite border;
    [PreviewField(80, ObjectFieldAlignment.Right)]
    public Sprite borderEquipmentInfo;
    [ColorPalette]
    public Color color;
    public Rarity type;
    public int upgradeLevel = 1;
    [Title("Stats")]
    public float rarityK;
    public int enchantMaxLevel;
    [Title("Merge Require")]
    public int subCount = 2;
    [EnumToggleButtons]
    public MergeType mergeType = MergeType.SAME_ITEM_AND_RARITY;
}
