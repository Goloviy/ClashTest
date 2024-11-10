using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
[CreateAssetMenu(fileName = "GachaEquipmentSystemData", menuName = "ScriptableObjects/Shop/GachaEquipmentSystemData", order = 1)]

public class GachaEquipmentSystemData : ScriptableObject
{
    public GachaType gachaType;
    public Rarity rarityMax = Rarity.Rare;
    public RarityGachaData[] rarityData;
    public int pointNormal;
    public EquipmentData[] equipments;
    public bool isDropSuperItem;
    [ShowIf("isDropSuperItem", true)]
    public Rarity rarityDropSuper;
    [ShowIf("isDropSuperItem", true)]
    public int pointSuper;
    [ShowIf("isDropSuperItem", true)]
    public EquipmentData[] superEquipments;

}
[System.Serializable]
public class RarityGachaData 
{
    public int point = 1;
    public Rarity rarity;
}