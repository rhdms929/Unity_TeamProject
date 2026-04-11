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
    public GameObject statPanel;
	public GameObject questPanel;
	public GameObject zonePanel;

	[Header("Log")] //활동, 전리품 기록
    public GameObject activityLogPanel;
    public GameObject lootLogPanel;
    public LogManager logManager;

    void Awake()
    {
        Time.timeScale = 1f;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (skillMenuPanel != null) skillMenuPanel.SetActive(false);
        if (statPanel != null) statPanel.SetActive(false);
		if (questPanel != null) questPanel.SetActive(false);
		if (zonePanel != null) zonePanel.SetActive(false);
	}
    void Update()
    {
        if (GameManager.instance != null && goldText != null) //coin
        {
            goldText.text = " " + GameManager.instance.currentGold.ToString("F0");
        }
	}
    
    // 스킬
    public void SkillMenu()
    {
		if (skillMenuPanel != null)
		{
			bool isActive = skillMenuPanel.activeSelf;
			skillMenuPanel.SetActive(!isActive);
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

	// 퀘스트 
	public void ToggleQuestMenu()
	{
		if (questPanel != null)
		{
			questPanel.SetActive(!questPanel.activeSelf);
		}
	}

	// 존(맵) 
	public void ToggleZoneMenu()
	{
		if (zonePanel != null)
		{
			zonePanel.SetActive(!zonePanel.activeSelf);
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