using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ItemHelper
{
    public static bool IsConsumable(ItemData data)
    {
        return data != null && data.itemCategory == ItemCategory.Consumable;
    }

    public static bool IsEquipment(ItemData data)
    {
        return data != null && data.itemCategory == ItemCategory.Equipment;
    }

    public static bool CanAssignToActionBar(ItemData data)
    {
        return data != null &&
               data.itemCategory == ItemCategory.Consumable &&
               data.canAssignToActionBar;
    }

    public static bool IsMatchingEquipmentSlot(ItemData data, EquipmentSlotType slotType)
    {
        return data != null &&
               data.itemCategory == ItemCategory.Equipment &&
               data.equipmentSlotType == slotType;
    }
}