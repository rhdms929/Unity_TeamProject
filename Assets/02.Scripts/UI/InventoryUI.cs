using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	[Header("Gold UI Elements")]
	[SerializeField] private TextMeshProUGUI selectedItemPriceText; 
	[SerializeField] private TextMeshProUGUI myCurrentGoldText;      

	void Start()
	{
		RefreshMyGold();
	}
	void OnEnable()
	{
		// 가방 창이 켜질 때마다 최신 골드로 업데이트
		RefreshMyGold();

		// 아이템 가격은 0으로 초기화
		if (selectedItemPriceText != null)
			selectedItemPriceText.text = "0";
	}

	public void RefreshMyGold()
	{
		if (GameManager.instance != null && myCurrentGoldText != null)
		{
			myCurrentGoldText.text = ((int)GameManager.instance.currentGold).ToString();
		}
	}

	public void OnItemSelected(int price)
	{
		if (selectedItemPriceText != null)
		{
			selectedItemPriceText.text = price.ToString();
		}
	}
}
