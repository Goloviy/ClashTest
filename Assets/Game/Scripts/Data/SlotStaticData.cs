using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotStaticData : MonoBehaviour
{
    List<SlotData> designs;
    Dictionary<ItemType, SlotData> dictDesign;
    public SlotStaticData()
    {
        designs = Resources.LoadAll<SlotData>(StringConst.PATH_SLOTS_DATA).ToList();
        dictDesign = new Dictionary<ItemType, SlotData>();
        foreach (var rarity in designs)
        {
            dictDesign.Add(rarity.type, rarity);
        }
    }
    public SlotData GetData(ItemType type)
    {
        SlotData data = null;
        if (!dictDesign.TryGetValue(type, out data))
        {
            DebugCustom.LogError("Design Id doesn't available :" + type);
        }
        return data;
    }
}
