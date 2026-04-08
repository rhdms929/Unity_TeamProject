using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite icon;
    public int itemLevel = 1;

    [Header("공통 분류")]
    public ItemCategory itemCategory;
    public bool isStackable = true;

    [Header("상점 / 합성 공통")]
    public int buyPrice = 10;
    public bool canCombine = false;
    public int combineNeedCount = 3;
    public ItemData nextItemData;

    [Header("소비 아이템 정보")]
    public ConsumableEffectType consumableEffectType = ConsumableEffectType.None;
    public float effectValue = 0f;
    public bool canAssignToActionBar = false;

    [Header("장비 아이템 정보")]
    public EquipmentSlotType equipmentSlotType = EquipmentSlotType.None;
    public int bonusAttack = 0;
    public int bonusDefense = 0;
    public int bonusHp = 0;
    public int bonusMp = 0;
}

public enum ItemCategory
{
    Consumable,
    Equipment,
    Material
}

public enum ConsumableEffectType
{
    None,
    HealHP,
    HealMP
}

public enum EquipmentSlotType
{
    None,
    Head,
    Tabard
}