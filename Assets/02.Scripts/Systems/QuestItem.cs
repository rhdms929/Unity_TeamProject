using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestItem : MonoBehaviour
{
	[Header("UI 연결")]
	public TextMeshProUGUI titleText;
	public GameObject completedIcon;

	// 내부 데이터 보관
	private string _title;
	private string _desc;
	private int _current;
	private int _target;
	private bool _isDone;

	public void Setup(string title, string desc, int current, int target, bool isDone)
	{
		_title = title;
		_desc = desc;
		_current = current;
		_target = target;
		_isDone = isDone;

		if (titleText != null) titleText.text = title;

		// 완료 여부에 따라 아이콘 표시
		if (completedIcon != null) completedIcon.SetActive(isDone);
	}

	// 퀘스트 항목 클릭 시 상세 정보 표시
	public void OnClick()
	{
		QuestManager.Instance.ShowQuestDetail(_title, _desc, _current, _target, _isDone);
	}
}