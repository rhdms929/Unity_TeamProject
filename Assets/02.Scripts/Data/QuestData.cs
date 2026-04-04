using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType { Kill, EXP, Item }

[System.Serializable] 
public class QuestData
{
	public string questTitle;     
	[TextArea]                     
	public string questDescription; 
	public QuestType questType;
	public int targetCount;      
	public string targetMonsterName; 
	public int zoneNumber;          

	[Header("진행 데이터")]
	public int currentCount;       
	public bool isCompleted;
}
