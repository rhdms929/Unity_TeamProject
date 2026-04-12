using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 하나의 정보 담는 코드 (ScriptableObject)
[CreateAssetMenu(fileName = "NewItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite icon;
    public int itemLevel = 1;

    [Header("분류")]
    public ItemCategory itemCategory;       // 포션/장비
    public bool isStackable = true;         // 중첩 가능 여부

    [Header("상점 / 합성")]
    public int buyPrice = 10;
    public bool canCombine = false;         // 합성 가능 여부
    public int combineNeedCount = 3;        // 합성에 필요한 개수
    public ItemData nextItemData;           // 합성 결과 아이템

    [Header("포션 아이템")]
    public PotionType potionType = PotionType.None;
    public float effectValue = 0f;          // HP/MP 회복량 수치
    public bool canBottomBar = false;       // 단축바 등록 가능 여부

    [Header("장비 아이템")]
    public EquipmentSlotType equipmentSlotType = EquipmentSlotType.None;
    public int bonusAttack = 0;
    public int bonusDefense = 0;
    public int bonusHp = 0;
    public int bonusMp = 0;
}

public enum ItemCategory
{
    Potion,
    Equipment,
}

public enum PotionType
{
    None,
    HealHP,
    HealMP
}

public enum EquipmentSlotType
{
    None,
    Head,
    Chest,
    Pant,
    Boot,
    Ring
}
