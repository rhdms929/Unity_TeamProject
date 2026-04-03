using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
	public static QuestManager Instance;
	public int currentZone = 1;
	public PlayerStats playerStats;
	private QuestData selectedQuest;

	[Header("Quest List")]
	public List<QuestData> questList;

	[Header("Quest UI System")]
	public GameObject questItemPrefab;
	public Transform questContent;

	[Header("Quest Detail Panel")]
	public TextMeshProUGUI detailTitleText;
	public TextMeshProUGUI detailDescText;

	private List<QuestItem> activeQuestUIList = new List<QuestItem>();

	void Awake() { Instance = this; }

	void Start()
	{
		RefreshQuestUI();
	}

	public void OnMonsterKilled(string monsterName)
	{
		bool anyQuestUpdated = false;
		foreach (QuestData quest in questList)
		{
			if (quest.zoneNumber == currentZone && !quest.isCompleted
		   && quest.questType == QuestType.Kill 
		   && monsterName.Contains(quest.targetMonsterName))
			{
				quest.currentCount++;
				anyQuestUpdated = true;
				if (quest.currentCount >= quest.targetCount)
					CompleteQuest(quest);
			}
		}
		if (anyQuestUpdated)
			UpdateAllQuestUI();
	}

	public void RefreshQuestUI()
	{
		foreach (Transform child in questContent)
			Destroy(child.gameObject);

		activeQuestUIList.Clear();

		if (detailTitleText != null) detailTitleText.text = "";
		if (detailDescText != null) detailDescText.text = "퀘스트를 눌러서 확인하세요";

		foreach (QuestData data in questList)
		{
			if (data.zoneNumber == currentZone)
			{
				GameObject obj = Instantiate(questItemPrefab, questContent);
				QuestItem itemScript = obj.GetComponent<QuestItem>();
				if (itemScript != null)
				{
					itemScript.Setup(data.questTitle, data.questDescription, data.currentCount, data.targetCount, data.isCompleted);
					activeQuestUIList.Add(itemScript);
				}
			}
		}
	}

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

	// 경험치 획득 시 호출
	public void OnEXPGained(int amount)
	{
		foreach (QuestData quest in questList)
		{
			if (quest.zoneNumber == currentZone && !quest.isCompleted && quest.questType == QuestType.EXP)
			{
				quest.currentCount += amount;
				if (quest.currentCount >= quest.targetCount)
					CompleteQuest(quest);
			}
		}
		UpdateAllQuestUI();
	}

	// 아이템 획득 시 호출
	public void OnItemGained()
	{
		foreach (QuestData quest in questList)
		{
			if (quest.zoneNumber == currentZone && !quest.isCompleted && quest.questType == QuestType.Item)
			{
				quest.currentCount++;
				if (quest.currentCount >= quest.targetCount)
					CompleteQuest(quest);
			}
		}
		UpdateAllQuestUI();
	}

	public void ShowQuestDetail(string title, string desc, int current, int target, bool isDone)
	{
		selectedQuest = questList.Find(q => q.questTitle == title);
		if (detailTitleText != null) detailTitleText.text = title;
		if (detailDescText != null) detailDescText.text = desc;
	}

	// 완료 버튼 OnClick에 연결
	public void OnClickCompleteButton()
	{
		if (selectedQuest == null)
		{
			Debug.Log("선택된 퀘스트가 없습니다.");
			return;
		}
		if (selectedQuest.isCompleted)
		{
			Debug.Log("이미 완료된 퀘스트입니다.");
			return;
		}

		CompleteQuest(selectedQuest);
		UpdateAllQuestUI();
	}

	void CompleteQuest(QuestData quest)
	{
		quest.isCompleted = true;
		LogManager.Instance.AddActivityLog($"<color=green>[퀘스트 완료]</color> {quest.questTitle}");
		UpdateAllQuestUI();
		CheckZoneProgress();
	}

	public void DropItemCheck()
	{
		if (Random.Range(0, 100) < 20)
		{
			string itemName = "체력 회복 아이템";
			if (LogManager.Instance != null)
				LogManager.Instance.AddActivityLog($"<color=yellow>[아이템 획득]</color> {itemName}을(를) 얻었습니다!");
			OnItemGained();
		}
	}

	void CheckZoneProgress()
	{
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
			Invoke("GoToNextZone", 2f);
	}

	void GoToNextZone()
	{
		currentZone++;
		LogManager.Instance.AddActivityLog($"[구역 해금] {currentZone}구역 임무 시작!");
		RefreshQuestUI();
	}
}