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

    [Header("상점 정보")]
    public int buyPrice = 10;

    [Header("포션 정보")]
    public float healAmount = 50f;

    public enum ItemType
    {
        HP_Potion,
        MP_Potion,
        HP_Potion_Big,
        MP_Potion_Big
    }

    public ItemType itemType;
}
