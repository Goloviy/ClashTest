using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsStaticData 
{
    List<ItemBagData> items;
    Dictionary<string, ItemBagData> dictItem;
    //List<ItemBagData> designs;
    List<EquipmentData> equipments;
    public ItemsStaticData()
    {
        items = Resources.LoadAll<ItemBagData>(StringConst.PATH_ITEMS_DATA).ToList();
        dictItem = new Dictionary<string, ItemBagData>();
        //designs = new List<ItemBagData>();
        equipments = new List<EquipmentData>();
        foreach (var item in items)
        {
            dictItem.Add(item.id, item);

            if (item is EquipmentData equipment)
            {
                equipments.Add(equipment);
            }
            //else
            //{
            //    designs.Add(item);
            //}
        }
    }
    public ItemBagData GetData(string id)
    {
        ItemBagData data = null;
        if (!dictItem.TryGetValue(id, out data))
        {
            DebugCustom.LogError("Item Id doesn't available :" + id);
        }
        return data;
    }
    //public ItemBagData GetDataItemDesign(ItemType designType)
    //{
    //    foreach (var design in designs)
    //    {
    //        if (design.slot == designType)
    //            return design;
    //    }
    //    return null;
    //}
    //public ItemBagData GetRandomEquipment()
    //{
    //    var rd = Random.Range(0, items.Count);
    //    return items[rd];
    //}
    //public ItemBagData GetRandomItemDesign()
    //{
    //    return designs[Random.Range(0, designs.Count)];
    //}
    public EquipmentData GetRandomEquipment(bool containSuperItem = false)
    {
        EquipmentData equipment;
        do
        {
            equipment = equipments[Random.Range(0, equipments.Count)];
        } while (equipment.isSuper && !containSuperItem);
        return equipment;
    }
}
