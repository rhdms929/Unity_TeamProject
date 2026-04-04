using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public double currentGold = 0;
	public Transform goldIcon;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

    public void AddGold(double points)
	{
		currentGold += points;
		Debug.Log("현재 골드: " + currentGold);

		// 가방 UI가 씬에 있다면, 글자를 바로 바꿔줌
		InventoryUI invUI = FindObjectOfType<InventoryUI>();
		if (invUI != null)
		{
			invUI.RefreshMyGold();
		}
	}
    public void ResetForNewRun()
    {
        currentGold = 0;
        goldIcon = null;
    }
}