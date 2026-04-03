using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType { Kill, EXP, Item }

[System.Serializable] 
public class QuestData
{
	public string questTitle;      // 퀘스트 제목 
	[TextArea]                     
	public string questDescription; // 퀘스트 설명
	public QuestType questType;
	public int targetCount;        // 목표 마릿수 
	public string targetMonsterName; // 잡아야 할 몬스터 이름 
	public int zoneNumber;          // 존 번호 (0: 첫 번째 존, 1: 두 번째 존, ...)

	[Header("진행 데이터")]
	public int currentCount;       
	public bool isCompleted;
}
