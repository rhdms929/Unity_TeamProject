using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InventoryEntry //인벤토리 한칸 데이터임
{
    public ItemData itemData;
    public int count;

    public InventoryEntry(ItemData data, int amount)
    {
        itemData = data;
        count = amount;
    }
}