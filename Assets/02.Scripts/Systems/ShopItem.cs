using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
	[Header("아이템 정보")]
	public ItemData itemData;
	public ItemSlot targetSlot; // 구매 시 추가될 인벤토리 슬롯

	[Header("UI 연결")]
	public TextMeshProUGUI itemNameText;
	public TextMeshProUGUI priceText;
	public GameObject buyButton; // Buy 버튼 오브젝트

	void Start()
	{
		if (itemData != null)
		{
			if (itemNameText != null) itemNameText.text = itemData.itemName;
			if (priceText != null) priceText.text = itemData.buyPrice.ToString();
		}
		if (buyButton != null) buyButton.SetActive(false);
	}

	// 아이템 클릭 시 호출
	public void OnClickItem()
	{
		ShopManager.Instance.SelectItem(this);
	}

	// Buy 버튼 클릭 시 호출
	public void OnClickBuy()
	{
		ShopManager.Instance.BuyItem(this);
	}
}
