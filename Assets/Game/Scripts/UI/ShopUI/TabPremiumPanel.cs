using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabPremiumPanel : MonoBehaviour
{
    public ItemGemPack prefabPackGem;

    public Transform tfParent;
    GameObject[] elementUIs;

    private void OnEnable()
    {
        ClearItems();
        CreateItems();
    }
    private void CreateItems()
    {
        var items = GameData.Instance.staticData.shopData.PremiumItems;
        elementUIs = new GameObject[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            var newItem = Instantiate(prefabPackGem, tfParent);
            newItem.Init(items[i]);
            elementUIs[i] = newItem.gameObject;
        }
    }
    private void ClearItems()
    {
        if (elementUIs == null)
        {
            return;
        }
        foreach (var element in elementUIs)
        {
            GameObject.Destroy(element);
        }
        Array.Clear(elementUIs, 0, elementUIs.Length);
    }
}
