using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaritiesStaticData : MonoBehaviour
{
    List<RarityData> rarities;
    Dictionary<Rarity, RarityData> dictRarity;
    public RaritiesStaticData()
    {
        rarities = Resources.LoadAll<RarityData>(StringConst.PATH_RARITIES_DATA).ToList();
        dictRarity = new Dictionary<Rarity, RarityData>();
        foreach (var rarity in rarities)
        {
            dictRarity.Add(rarity.type, rarity);
        }
    }
    public RarityData GetData(Rarity type)
    {
        RarityData data = null;
        if (!dictRarity.TryGetValue(type, out data))
        {
            DebugCustom.LogError("Rarity Id doesn't available :" + type);
        }
        return data;
    }
}
