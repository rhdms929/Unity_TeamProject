using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[Header("Item Configuration")]
	public ItemData itemData;
	// 아이템 종류 선택
	public enum ItemType { HP_Potion, MP_Potion }
	public ItemType type;
	public float healAmount = 50f; // 회복 수치

	public string itemName;
    [TextArea] public string itemDescription;
	// 각 슬롯마다 위치 설정
	public Vector2 tooltipOffset = new Vector2(-100f, 200f);

	[Header("아이템 획득")]
	public TextMeshProUGUI countText; 
	public int itemCount = 0;
	void Start()
	{
		UpdateUI();
	}

	// 아이템 개수를 하나 늘리고 UI를 갱신하는 함수
	public void AddItem(int amount = 1)
	{
		itemCount += amount;
		UpdateUI();

		// 활동 기록에 로그 남기
		if (LogManager.Instance != null)
		{
			LogManager.Instance.AddActivityLog($"<color=yellow>[아이템 획득]</color> {itemName}을 얻었습니다)");
		}
	}

	void UpdateUI()
	{
		if (countText != null)
			countText.text = itemCount.ToString();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (itemData != null)
		{
			InventoryUI invUI = FindObjectOfType<InventoryUI>();
			if (invUI != null)
			{
				invUI.OnItemSelected(itemData.buyPrice);
			}
		}
		if (itemCount <= 0) return;
		// 씬에서 플레이어 상태 스크립트를 찾아 회복 실행
		PlayerStats player = FindObjectOfType<PlayerStats>();

		if (player != null)
		{
			if (type == ItemType.HP_Potion)
				player.HealHP(healAmount);
			else if (type == ItemType.MP_Potion)
				player.HealMP(healAmount);

			itemCount--; 
			UpdateUI();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{

		if (ItemTooltip.instance != null)
		{
			ItemTooltip.instance.Show(itemName, itemDescription, tooltipOffset);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (ItemTooltip.instance != null)
			ItemTooltip.instance.Hide();
	}
}
