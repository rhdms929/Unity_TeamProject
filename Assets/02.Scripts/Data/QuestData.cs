using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class QuestData
{
	public string questTitle;      // 퀘스트 제목 
	[TextArea]                     // 인스펙터에서 글쓰기 편하게 창을 넓혀줌
	public string questDescription; // 퀘스트 설명
	public int targetCount;        // 목표 마릿수 
	public string targetMonsterName; // 잡아야 할 몬스터 이름 
}
