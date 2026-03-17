using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    private IEnumerator Start()
    {
        Debug.Log("LoadScene 시작");

        yield return new WaitForSeconds(2f); // 테스트용

        AsyncOperation op = SceneManager.LoadSceneAsync("GameScene");

        while (!op.isDone)
        {
            yield return null;
        }
    }
}