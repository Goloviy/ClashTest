using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentsStaticData 
{
    List<EquipmentData> equips;
    Dictionary<string, EquipmentData> dictEquip;
    public EquipmentsStaticData()
    {
        equips = Resources.LoadAll<EquipmentData>(StringConst.PATH_ITEMS_DATA).ToList();
        dictEquip = new Dictionary<string, EquipmentData>();
        foreach (var equip in equips)
        {
            dictEquip.Add(equip.id, equip);
        }
    }
    public EquipmentData GetData(string id)
    {
        EquipmentData data = null;
        if (!dictEquip.TryGetValue(id, out data))
        {
            DebugCustom.LogError("Equipment Id doesn't available :" + id);
        }
        return data;
    }
}
