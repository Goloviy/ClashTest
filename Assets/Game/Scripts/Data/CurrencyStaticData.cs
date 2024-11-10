using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrencyStaticData 
{
    List<CurrencyData> currencies;
    Dictionary<Currency, CurrencyData> dicts;
    public CurrencyStaticData()
    {
        currencies = Resources.LoadAll<CurrencyData>(StringConst.PATH_CURRENCIES_DATA).ToList();
        dicts = new Dictionary<Currency, CurrencyData>();
        foreach (var equip in currencies)
        {
            dicts.Add(equip.currency, equip);
        }
        DebugCustom.Log("");
    }
    public CurrencyData GetData(Currency currency)
    {
        CurrencyData data = null;
        if (!dicts.TryGetValue(currency, out data))
        {
            DebugCustom.LogError("Item Id doesn't available :" + currency);
        }
        return data;
    }
    //public CurrencyData GetData(string id)
    //{
    //    CurrencyData data = null;
    //    foreach (var item in collection)
    //    {

    //    }
    //    return data;
    //}
}
