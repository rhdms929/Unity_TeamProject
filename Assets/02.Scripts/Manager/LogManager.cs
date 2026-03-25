using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
public class LogManager : MonoBehaviour
{
	public static LogManager Instance;
	public TextMeshProUGUI activityLogText;
    public TextMeshProUGUI lootLogText;
    public int maxLines = 20;

    private Queue<string> activityLines = new Queue<string>();
    private Queue<string> lootLines = new Queue<string>();
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject); 
		}
	}
	public void AddActivityLog(string message)
    {
        AddLine(activityLines, activityLogText, message);
    }

    public void AddLootLog(string message)
    {
        AddLine(lootLines, lootLogText, message);
    }

    void AddLine(Queue<string> queue, TextMeshProUGUI targetText, string message)
    {
        if (targetText == null) return;

        queue.Enqueue(message);

        while (queue.Count > maxLines)
            queue.Dequeue();

        StringBuilder sb = new StringBuilder();

        foreach (string line in queue)
            sb.AppendLine(line);

        targetText.text = sb.ToString();
    }
}