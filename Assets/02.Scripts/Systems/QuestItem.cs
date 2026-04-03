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
		if (completedIcon != null) completedIcon.SetActive(isDone);
	}
	public void OnClick()
	{
		QuestManager.Instance.ShowQuestDetail(_title, _desc, _current, _target, _isDone);
	}
}