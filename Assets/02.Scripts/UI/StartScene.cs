using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScene : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LoadScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("게임 종료!");
    }
}