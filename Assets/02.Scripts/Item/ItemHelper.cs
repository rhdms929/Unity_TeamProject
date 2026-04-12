using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 관련 조건 판별을 모아둔 유틸 클래스
// MonoBehaviour 없이 어디서든 정적으로 호출 가능
public static class ItemHelper
{
    // 소비 아이템인지 확인
    public static bool IsConsumable(ItemData data)
    {
        if (data == null)
        {
            return false;
        }

        if (data.itemCategory != ItemCategory.Potion)
        {
            return false;
        }
        return true;
    }

    //장비 아이템인지 확인
    public static bool IsEquipment(ItemData data)
    {
        if (data == null)
        {
            return false;
        }

        if (data.itemCategory != ItemCategory.Equipment)
        {
            return false;
        }
        return true;
    }

    //바텀바에 등록 가능한 아이템인지 확인
    public static bool CanBottomBar(ItemData data)
    {
        if (data == null)
        {
            return false;
        }

        if (data.itemCategory != ItemCategory.Potion)
        {
            return false;
        }

        if (data.canBottomBar == false)
        {
            return false;
        }
        return true;
    }

    // 현재 장비 슬롯 타입과 아이템 슬롯 타입이 맞는지 확인
    public static bool IsMatchingEquipmentSlot(ItemData data, EquipmentSlotType slotType)
    {
        if (data == null)
        {
            return false;
        }

        if (data.itemCategory != ItemCategory.Equipment)
        {
            return false;
        }

        if (data.equipmentSlotType != slotType)
        {
            return false;
        }
        return true;
    }
}
