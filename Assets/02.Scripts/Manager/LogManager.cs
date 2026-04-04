using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;
public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [Header("Text")]
    public TextMeshProUGUI activityLogText;
    public TextMeshProUGUI lootLogText;

    [Header("Scroll")]
    public ScrollRect activityScrollRect;
    public ScrollRect lootScrollRect;

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
    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    void Start()
    {
        StartCoroutine(SetStartScrollPosition());
    }

    IEnumerator SetStartScrollPosition()
    {
        yield return null;

        Canvas.ForceUpdateCanvases();

        if (activityScrollRect != null)
            activityScrollRect.verticalNormalizedPosition = 1f;

        if (lootScrollRect != null)
            lootScrollRect.verticalNormalizedPosition = 1f;
    }

    public void AddActivityLog(string message)
    {
        AddLine(activityLines, activityLogText, activityScrollRect, message);
    }

    public void AddLootLog(string message)
    {
        AddLine(lootLines, lootLogText, lootScrollRect, message);
    }

    void AddLine(Queue<string> queue, TextMeshProUGUI targetText, ScrollRect targetScrollRect, string message)
    {
        if (targetText == null) return;

        queue.Enqueue(message);

        while (queue.Count > maxLines)
            queue.Dequeue();

        StringBuilder sb = new StringBuilder();

        foreach (string line in queue)
            sb.AppendLine(line);

        targetText.text = sb.ToString();

        StartCoroutine(RefreshScroll(targetScrollRect));
    }

    IEnumerator RefreshScroll(ScrollRect targetScrollRect)
    {
        yield return null;
        Canvas.ForceUpdateCanvases();

        if (targetScrollRect != null)
        {
            // 아래로 쌓이는 방식이면 0f
            targetScrollRect.verticalNormalizedPosition = 0f;
        }
    }
}