using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
	public static MapManager Instance;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Map2 버튼용 (다른 씬으로 이동)
	public void GoToMap(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
