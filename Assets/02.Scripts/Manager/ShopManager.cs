using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
	public static ShopManager Instance;

	private ShopItem selectedItem;
	private ShopItem previousItem;

	void Awake() { Instance = this; }

	public void SelectItem(ShopItem item)
	{
		// 이전 선택 아이템 Buy 버튼 숨기기
		if (previousItem != null && previousItem != item)
			previousItem.buyButton.SetActive(false);

		selectedItem = item;
		previousItem = item;

		// Buy 버튼 보여주기
		if (item.buyButton != null)
			item.buyButton.SetActive(true);
	}

	public void BuyItem(ShopItem item)
	{
		if (item.itemData == null) return;

		int price = item.itemData.buyPrice;

		if (GameManager.instance.currentGold < price)
		{
			LogManager.Instance.AddActivityLog("<color=red>[구매 실패]</color> 골드가 부족합니다!");
			return;
		}

		GameManager.instance.AddGold(-price);

		if (item.targetSlot != null)
			item.targetSlot.AddItem(1);

		LogManager.Instance.AddActivityLog($"<color=yellow>[구매]</color> {item.itemData.itemName}을(를) 구매했습니다!");

		item.buyButton.SetActive(false);
		selectedItem = null;
	}
}
