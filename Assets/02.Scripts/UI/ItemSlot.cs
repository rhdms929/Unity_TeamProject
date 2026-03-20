using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	// 아이템 종류 선택
	public enum ItemType { HP_Potion, MP_Potion }
	public ItemType type;
	public float healAmount = 50f; // 회복 수치

	public string itemName;
    [TextArea] public string itemDescription;
	// 각 슬롯마다 위치 설정
	public Vector2 tooltipOffset = new Vector2(-100f, 200f);

	public void OnPointerClick(PointerEventData eventData)
	{
		// 씬에서 플레이어 상태 스크립트를 찾아 회복 실행
		PlayerStatus player = FindObjectOfType<PlayerStatus>();

		if (player != null)
		{
			if (type == ItemType.HP_Potion)
				player.HealHP(healAmount);
			else if (type == ItemType.MP_Potion)
				player.HealMP(healAmount);
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
