using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
	public static ItemTooltip instance; // 핵심: 어디서든 부를 수 있게 함

	public TextMeshProUGUI titleText;
	public TextMeshProUGUI descriptionText;
	private RectTransform rectTransform;

	void Awake()
	{
		instance = this; // 싱글톤 설정
		rectTransform = GetComponent<RectTransform>();
		gameObject.SetActive(false);
	}

	public void Show(string title, string desc, Vector2 position) 
	{
		titleText.text = title;
		descriptionText.text = desc;

		rectTransform.anchoredPosition = position;

		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
