using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [Header("Gold UI")]
    public TextMeshProUGUI goldText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
    void Update()
    {
        if (GameManager.instance != null && goldText != null) //coin
        {
            goldText.text = " " + GameManager.instance.currentGold.ToString("F0");
        }
    }
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }
    public void RetryGame() //retry 버튼
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu() //MainMenu 버튼
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }
}