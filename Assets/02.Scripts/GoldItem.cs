using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldItem : MonoBehaviour
{
	private bool canPickUp = false;

	private void Start()
	{
		Invoke("EnablePickUp", 0.3f); // 0.3초 뒤에 먹을 수 있게 설정
	}
	void EnablePickUp()
	{
		canPickUp = true;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (canPickUp && collision.CompareTag("Player"))
		{
			if (GameManager.instance != null)
			{
				GameManager.instance.AddGold(1);
				Destroy(gameObject);
			}
		}
	}
}
