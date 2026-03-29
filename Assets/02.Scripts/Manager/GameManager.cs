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
		Debug.Log(" " + currentGold);
	}
}