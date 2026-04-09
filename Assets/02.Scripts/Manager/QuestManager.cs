using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
	public static QuestManager Instance;

	public int currentZone = 1;
	public PlayerStats playerStats;

	[Header("Quest List")]
	public List<QuestData> questList;

	[Header("Quest UI System")]
	public GameObject questItemPrefab;
	public Transform questContent;

	[Header("Quest Detail Panel")]
	public TextMeshProUGUI detailTitleText;
	public TextMeshProUGUI detailDescText;

	private QuestData selectedQuest;
	private List<QuestItem> activeQuestUIList = new List<QuestItem>();

	void Awake() { Instance = this; }

	void Start()
	{
		RefreshQuestUI();
	}

	// 몬스터 처치 시 호출 - Kill 타입 퀘스트 카운트 갱신
	public void OnMonsterKilled(string monsterName)
	{
		bool anyUpdated = false;

		foreach (QuestData quest in questList)
		{
			if (quest.zoneNumber != currentZone) continue;
			if (quest.isCompleted) continue;
			if (quest.questType != QuestType.Kill) continue;
			if (!monsterName.Contains(quest.targetMonsterName)) continue;

			quest.currentCount++;
			anyUpdated = true;

			if (quest.currentCount >= quest.targetCount)
				CompleteQuest(quest);
		}
		if (anyUpdated)
			UpdateAllQuestUI();
	}

	// 경험치 획득 시 호출 - EXP 타입 퀘스트 카운트 갱신
	public void OnEXPGained(int amount)
	{
		foreach (QuestData quest in questList)
		{
			if (quest.zoneNumber != currentZone) continue;
			if (quest.isCompleted) continue;
			if (quest.questType != QuestType.EXP) continue;

			quest.currentCount += amount;

			if (quest.currentCount >= quest.targetCount)
				CompleteQuest(quest);
		}
		UpdateAllQuestUI();
	}
	// 아이템 획득 시 호출 - Item 타입 퀘스트 카운트 갱신
	public void OnItemGained(string itemName)
	{
		foreach (QuestData quest in questList)
		{
			if (quest.zoneNumber != currentZone) continue;
			if (quest.isCompleted) continue;
			if (quest.questType != QuestType.Item) continue;

			// 중요: 지금 먹은 아이템 이름(itemName)에 목표 이름(targetItemName)이 포함되어 있는지 확인!
			if (!string.IsNullOrEmpty(quest.targetItemName) && !itemName.Contains(quest.targetItemName))
				continue;

			quest.currentCount++;

			if (quest.currentCount >= quest.targetCount)
				CompleteQuest(quest);
		}
		UpdateAllQuestUI();
	}
	// 현재 구역 퀘스트 UI 전체 재생성
	public void RefreshQuestUI()
	{
		foreach (Transform child in questContent)
			Destroy(child.gameObject);

		activeQuestUIList.Clear();

		if (detailTitleText != null) detailTitleText.text = "";
		if (detailDescText != null) detailDescText.text = "퀘스트를 눌러서 확인하세요";

		foreach (QuestData data in questList)
		{
			if (data.zoneNumber != currentZone) continue;

			GameObject obj = Instantiate(questItemPrefab, questContent);
			QuestItem itemScript = obj.GetComponent<QuestItem>();

			if (itemScript != null)
			{
				itemScript.Setup(data.questTitle, data.questDescription, data.currentCount, data.targetCount, data.isCompleted);
				activeQuestUIList.Add(itemScript);
			}
		}
	}
	// 생성된 UI 텍스트만 갱신
	void UpdateAllQuestUI()
	{
		int uiIndex = 0;
		foreach (QuestData data in questList)
		{
			if (data.zoneNumber != currentZone) continue;
			if (uiIndex >= activeQuestUIList.Count) break;

			activeQuestUIList[uiIndex].Setup(data.questTitle, data.questDescription, data.currentCount, data.targetCount, data.isCompleted);
			uiIndex++;
		}
	}

	// 퀘스트 클릭 시 하단 패널에 상세 정보 표시
	public void ShowQuestDetail(string title, string desc, int current, int target, bool isDone)
	{
		selectedQuest = questList.Find(q => q.questTitle == title);

		if (detailTitleText != null) detailTitleText.text = title;
		if (detailDescText != null) detailDescText.text = desc;
	}


	void CompleteQuest(QuestData quest)
	{
		quest.isCompleted = true;
		LogManager.Instance.AddActivityLog($"<color=green>[퀘스트 완료]</color> {quest.questTitle}");
		UpdateAllQuestUI();
	}
	
}