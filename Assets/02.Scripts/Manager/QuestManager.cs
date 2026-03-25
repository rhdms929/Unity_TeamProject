using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
	public static QuestManager Instance; // 어디서든 접근 가능하게

	[Header("Quest UI")]
	public TextMeshProUGUI titleText;
	public TextMeshProUGUI detailText;
	public TextMeshProUGUI statusText;

	[Header("Quest List")]
	public List<QuestData> questList; // 여기서 퀘스트들을 순서대로 등록
	private int currentQuestIndex = 0;
	private int currentKillCount = 0;

	void Awake() { Instance = this; }
	void Start()
	{
		UpdateQuestDisplay(); // 게임 시작 시 첫 번째 퀘스트 출력
	}

	// 몬스터가 죽을 때 호출할 함수
	public void OnMonsterKilled(string monsterName)
	{
		if (currentQuestIndex >= questList.Count) return;

		QuestData currentQuest = questList[currentQuestIndex];

		// 현재 퀘스트의 목표 몬스터인지 확인
		if (monsterName.Contains(currentQuest.targetMonsterName))
		{
			currentKillCount++;
			UpdateQuestDisplay();

			if (currentKillCount >= currentQuest.targetCount)
			{
				CompleteQuest();
			}
		}
	}

	void UpdateQuestDisplay()
	{
		QuestData q = questList[currentQuestIndex];
		titleText.text = q.questTitle;
		detailText.text = $"- {q.questDescription} ({currentKillCount}/{q.targetCount})";
		statusText.text = "- 완료여부 : [ <color=red>미완료</color> ]";
	}

	void CompleteQuest()
	{
		statusText.text = "- 완료여부 : [ <color=green>완료</color> ]";

		// 현재 완료한 퀘스트 데이터를 가져오기
		QuestData currentQuest = questList[currentQuestIndex];

		// 활동 기록
		LogManager log = Object.FindAnyObjectByType<LogManager>();
		if (log != null)
		{
			log.AddActivityLog($"<color=green>[퀘스트 완료]</color> {currentQuest.questTitle} 다음 임무를 확인하세요.");
		}

		Invoke("NextQuest", 2f);
	}

	void NextQuest()
	{
		currentQuestIndex++;
		currentKillCount = 0;

		if (currentQuestIndex < questList.Count)
		{
			UpdateQuestDisplay();
		}
		else
		{
			titleText.text = "모든 퀘스트 완료!";
		}
	}
}
