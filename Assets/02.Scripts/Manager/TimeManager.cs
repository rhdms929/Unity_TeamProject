using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float elapsedTime = 0f;
    private bool isTimerRunning = true;

	void Update()
	{
		if (isTimerRunning)
		{
			elapsedTime += Time.deltaTime;
			UpdateTimerUI();
		}
	}

	void UpdateTimerUI()
	{
		// 시간을 분과 초로 계산
		int minutes = Mathf.FloorToInt(elapsedTime / 60f);
		int seconds = Mathf.FloorToInt(elapsedTime % 60f);

		if (timeText != null)
		{
			timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
		}
	}

	// 게임 오버 시 타이머를 멈추는 함수
	public void StopTimer()
	{
		isTimerRunning = false;
	}
}
