using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSystems : MonoBehaviour
{
	private static PersistentSystems instance;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴 금지!
		}
		else
		{
			Destroy(gameObject); // 중복 생성 방지
		}
	}
}
