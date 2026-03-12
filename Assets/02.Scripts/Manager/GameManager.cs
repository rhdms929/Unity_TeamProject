using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public double currentGold = 0;
	//public double goldPerSecond = 1;	//	1√ ∏∂¥Ÿ ∞ÒµÂ∞° 1æø ¡ı∞° (∏∑æ∆µ“)

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

    //void Start()                                                       (∏∑æ∆µ“)
    //{																	 (∏∑æ∆µ“)
    //	StartCoroutine(AutoGoldRoutine());								 (∏∑æ∆µ“)
    //}																	 (∏∑æ∆µ“)
    //																	 (∏∑æ∆µ“)
    //IEnumerator AutoGoldRoutine()										 (∏∑æ∆µ“)
    //{																	 (∏∑æ∆µ“)
    //	while (true)													 (∏∑æ∆µ“)
    //	{																 (∏∑æ∆µ“)
    //		yield return new WaitForSeconds(1f); // 1√ ∏∂¥Ÿ Ω««‡	     (∏∑æ∆µ“)
    //		AddGold(goldPerSecond);										 (∏∑æ∆µ“)
    //	}																 (∏∑æ∆µ“)
    //}																	 (∏∑æ∆µ“)

    public void AddGold(double points)
	{
		currentGold += points;
		Debug.Log(" " + currentGold);
	}
}