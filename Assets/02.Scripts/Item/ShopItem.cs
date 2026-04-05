using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour //아이템 정보
{
	[Header("아이템 정보")]
	public ItemData itemData;

	[Header("UI 연결")]
	public TextMeshProUGUI itemNameText;
	public TextMeshProUGUI priceText;
	public GameObject buyButton; // Buy 버튼 오브젝트

    void Start()
    {
        if (itemData != null)
        {
            if (itemNameText != null)
                itemNameText.text = itemData.itemName;

            if (priceText != null)
                priceText.text = itemData.buyPrice.ToString();
        }

        if (buyButton != null)
            buyButton.SetActive(false);
    }

    // 아이템 클릭 시 호출
    public void OnClickItem()
	{
        if (ShopManager.Instance != null)
            ShopManager.Instance.SelectItem(this);
    }

	// Buy 버튼 클릭 시 호출
	public void OnClickBuy()
	{
        if (ShopManager.Instance != null)
            ShopManager.Instance.BuyItem(this);
    }
}
