using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
	[Header("Basic Info")]
	public string itemName;       // 아이템 이름
	public Sprite itemIcon;       // 가방에 표시될 아이콘
	[TextArea]
	public string description;    // 아이템 설명

	[Header("Price Info")]
	public int buyPrice;          // 상점 구매가
	public int sellPrice;         // 상점 판매가

	[Header("Item Type")]
	public ItemType itemType;     // 소모품, 장비 등 구분

	public enum ItemType
	{
		Consumable, // 소모품 (포션 등)
		Equipment,  // 장비 (검, 갑옷 등)
		Etc         // 기타 (재료 등)
	}
}
