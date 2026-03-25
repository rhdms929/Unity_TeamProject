using UnityEngine;
using TMPro;

public class QuestItem : MonoBehaviour
{
	[Header("UI 연결")]
	public TextMeshProUGUI titleText;
	public TextMeshProUGUI descText;
	public TextMeshProUGUI statusText;

	public void Setup(string title, string desc, int current, int target, bool isDone)
	{
		if (titleText != null) titleText.text = title;
		if (descText != null) descText.text = desc;

		if (statusText != null)
		{
			statusText.text = isDone ? "<color=green>[완료]</color>" : "<color=red>[진행 중]</color>";
		}
	}
}