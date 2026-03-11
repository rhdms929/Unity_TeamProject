using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public double currentGold = 0;
	public double goldPerSecond = 1;	//	1초마다 골드가 1씩 증가

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

	void Start()
	{
		StartCoroutine(AutoGoldRoutine());
	}

	IEnumerator AutoGoldRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f); // 1초마다 실행
			AddGold(goldPerSecond);
		}
	}

	public void AddGold(double points)
	{
		currentGold += points;
		Debug.Log(" " + currentGold);
	}
}