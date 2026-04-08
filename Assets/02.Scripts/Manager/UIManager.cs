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

    [Header("Menu UI")]
    public GameObject skillMenuPanel;
    public GameObject optionsPanel;
    public GameObject statPanel;

    [Header("Log")] //활동, 전리품 기록
    public GameObject activityLogPanel;
    public GameObject lootLogPanel;
    public LogManager logManager;

    void Awake()
    {
        Time.timeScale = 1f;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (skillMenuPanel != null) skillMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (statPanel != null) statPanel.SetActive(false);
    }
    void Update()
    {
        if (GameManager.instance != null && goldText != null) //coin
        {
            goldText.text = " " + GameManager.instance.currentGold.ToString("F0");
        }

		//if (Input.GetKeyDown(KeyCode.Escape))   //  ESC키 누르면 옵션창 키는거 일단 꺼둠
		//{
		//	ToggleOptions();
		//}
	}
    public void SkillMenu()
    {
		if (skillMenuPanel != null)
		{
			bool isActive = skillMenuPanel.activeSelf;
			skillMenuPanel.SetActive(!isActive);
		}
	}
    public void ToggleOptions()
    {
        if (optionsPanel != null)
        {
            bool isActive = optionsPanel.activeSelf;
            optionsPanel.SetActive(!isActive);
			// 옵션창이 켜질 때 게임이 멈추도록 설정
			if (!isActive) Time.timeScale = 0f;
			else Time.timeScale = 1f;
		}
	}

    public void CloseOptions()  //  옵션창 닫는 버튼
	{
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false); 
            Time.timeScale = 1f;          
        }
    }

	// 스탯
	public void ToggleStatMenu()
	{
		if (statPanel != null)
		{
			bool isActive = !statPanel.activeSelf;
			statPanel.SetActive(isActive);

			Time.timeScale = 1f;
		}
	}
	public void CloseStatMenu() // 스탯 창 닫는 버튼
	{
		if (statPanel != null)
		{
			statPanel.SetActive(false);
			// 스탯 창 닫을 때 게임이 다시 시작되도록 설정
			Time.timeScale = 1f;
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

        if (GameManager.instance != null)
            GameManager.instance.ResetForNewRun();

        SceneManager.LoadScene("GameScene");
    }
    public void GoToMainMenu() //MainMenu 버튼
    {
        Time.timeScale = 1f;

        if (GameManager.instance != null)
            GameManager.instance.ResetForNewRun();

        SceneManager.LoadScene("StartScene");
    }

    //----활동, 전리품 기록------
    public void ToggleActivityLog()
    {
        if (activityLogPanel != null)
        {
            bool isActive = !activityLogPanel.activeSelf;
            activityLogPanel.SetActive(isActive);
        }
    }

    public void ToggleLootLog()
    {
        if (lootLogPanel != null)
        {
            bool isActive = !lootLogPanel.activeSelf;
            lootLogPanel.SetActive(isActive);
        }
    }

    public void CloseActivityLog()
    {
        if (activityLogPanel != null)
            activityLogPanel.SetActive(false);
    }

    public void CloseLootLog()
    {
        if (lootLogPanel != null)
            lootLogPanel.SetActive(false);
    }
    //----활동, 전리품 기록------
}