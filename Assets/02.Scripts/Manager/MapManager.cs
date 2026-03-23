using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
	public void StayInMap1(GameObject uiPanel)
	{
		uiPanel.SetActive(false);
	}

	// Map2 버튼용 (다른 씬으로 이동)
	public void GoToMap2(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
