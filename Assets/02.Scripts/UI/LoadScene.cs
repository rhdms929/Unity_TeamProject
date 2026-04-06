using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
	public Image loadingBar; // 로딩바 Image 연결

	private IEnumerator Start()
	{
		AsyncOperation op = SceneManager.LoadSceneAsync("GameScene");
		op.allowSceneActivation = false;

		float current = 0f;

		while (!op.isDone)
		{
			float target = Mathf.Clamp01(op.progress / 0.9f);

			current = Mathf.MoveTowards(current, target, Time.deltaTime * 0.5f);
			loadingBar.fillAmount = current;

			if (current >= 1f)
			{
				yield return new WaitForSeconds(0.5f);
				op.allowSceneActivation = true;
			}

			yield return null;
		}
	}
}