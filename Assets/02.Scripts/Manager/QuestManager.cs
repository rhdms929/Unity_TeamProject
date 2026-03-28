using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
	public static QuestManager Instance;

	public int currentZone = 1;
	public float requiredExpForNextZone = 100f;
	public PlayerStats playerStats;

	[Header("Quest List")]
	public List<QuestData> questList;

	[Header("Quest UI System")]
	public GameObject questItemPrefab; 
	public Transform questContent;     

	// 생성된 UI 항목들을 담아둘 리스트 (나중에 점수 올릴 때 사용)
	private List<QuestItem> activeQuestUIList = new List<QuestItem>();

	void Awake() { Instance = this; }

	void Start()
	{
		RefreshQuestUI();
	}

	// 몬스터가 죽을 때 호출
	public void OnMonsterKilled(string monsterName)
	{
		bool anyQuestUpdated = false;

		foreach (QuestData quest in questList)
		{
			// 1. 현재 구역 퀘스트이고, 아직 미완료이며, 몬스터 이름이 일치하는지 확인
			if (quest.zoneNumber == currentZone && !quest.isCompleted && monsterName.Contains(quest.targetMonsterName))
			{
				quest.currentCount++;
				anyQuestUpdated = true;

				if (quest.currentCount >= quest.targetCount)
				{
					CompleteQuest(quest);
				}
			}
		}

		if (anyQuestUpdated)
		{
			UpdateAllQuestUI();
		}
	}

	// 현재 구역의 모든 퀘스트 UI를 생성 
	public void RefreshQuestUI()
	{
		// 1. 기존 UI 싹 지우기
		foreach (Transform child in questContent)
		{
			Destroy(child.gameObject);
		}
		activeQuestUIList.Clear();

		// 2. 현재 구역에 맞는 퀘스트만 생성
		foreach (QuestData data in questList)
		{
			if (data.zoneNumber == currentZone)
			{
				GameObject obj = Instantiate(questItemPrefab, questContent);
				QuestItem itemScript = obj.GetComponent<QuestItem>();

				if (itemScript != null)
				{
					// QuestItem 스크립트에 데이터 주입
					itemScript.Setup(data.questTitle, data.questDescription, data.currentCount, data.targetCount, data.isCompleted);
					activeQuestUIList.Add(itemScript);
				}
			}
		}
	}

	// 생성된 UI의 텍스트만 실시간으로 업데이트
	void UpdateAllQuestUI()
	{
		int uiIndex = 0;
		foreach (QuestData data in questList)
		{
			if (data.zoneNumber == currentZone && uiIndex < activeQuestUIList.Count)
			{
				activeQuestUIList[uiIndex].Setup(data.questTitle, data.questDescription, data.currentCount, data.targetCount, data.isCompleted);
				uiIndex++;
			}
		}
	}

	void CompleteQuest(QuestData quest)
	{
		quest.isCompleted = true;

		LogManager.Instance.AddActivityLog($"<color=green>[퀘스트 완료]</color> {quest.questTitle}");

		// 구역 내 모든 퀘스트 완료 여부 체크
		CheckZoneProgress();
	}
	public void DropItemCheck()
	{
		// 20% 확률 체크 (0~100 중 20 이하)
		if (Random.Range(0, 100) < 20)
		{
			string itemName = "체력 회복 아이템"; 

			// 활동 기록에 남기기
			if (LogManager.Instance != null)
			{
				LogManager.Instance.AddActivityLog($"<color=yellow>[아이템 획득]</color> {itemName}을(를) 얻었습니다!");
			}

			// 나중에 인벤토리 숫자 올리는 로직 여기서 추가할 예정..
		}
	}

	void CheckZoneProgress()
	{
		// 현재 구역의 퀘스트가 모두 완료되었는지 확인
		bool allDone = true;
		foreach (var q in questList)
		{
			if (q.zoneNumber == currentZone && !q.isCompleted)
			{
				allDone = false;
				break;
			}
		}

		if (allDone)
		{
			if (true)
			{
				Invoke("GoToNextZone", 2f);
			}
		}
	}

	void GoToNextZone()
	{
		currentZone++;
		LogManager.Instance.AddActivityLog($"[구역 해금] {currentZone}구역 임무 시작!");
		RefreshQuestUI(); // 구역이 바뀌었으니 UI 새로고침
	}
}