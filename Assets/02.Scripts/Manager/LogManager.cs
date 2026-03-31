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

    [Header("ScrollRect")]
    public ScrollRect activityScrollRect;
    public ScrollRect lootScrollRect;

    [Header("Content Rect")]
    public RectTransform activityContent;
    public RectTransform lootContent;

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

    void Start()
    {
        StartCoroutine(InitScrollPosition());
    }

    IEnumerator InitScrollPosition()
    {
        yield return null;
        Canvas.ForceUpdateCanvases();

        if (activityContent != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(activityContent);

        if (lootContent != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(lootContent);

        // 처음에는 맨 위
        if (activityScrollRect != null)
            activityScrollRect.verticalNormalizedPosition = 1f;

        if (lootScrollRect != null)
            lootScrollRect.verticalNormalizedPosition = 1f;
    }

    public void AddActivityLog(string message)
    {
        AddLine(activityLines, activityLogText, activityScrollRect, activityContent, message);
    }

    public void AddLootLog(string message)
    {
        AddLine(lootLines, lootLogText, lootScrollRect, lootContent, message);
    }

    void AddLine(
        Queue<string> queue,
        TextMeshProUGUI targetText,
        ScrollRect targetScrollRect,
        RectTransform contentRect,
        string message)
    {
        if (targetText == null) return;

        queue.Enqueue(message);

        while (queue.Count > maxLines)
            queue.Dequeue();

        StringBuilder sb = new StringBuilder();

        foreach (string line in queue)
        {
            sb.AppendLine(line);
        }

        targetText.text = sb.ToString();

        StartCoroutine(RefreshScroll(targetScrollRect, contentRect));
    }
    IEnumerator RefreshScroll(ScrollRect targetScrollRect, RectTransform contentRect)
    {
        yield return null;
        Canvas.ForceUpdateCanvases();

        if (contentRect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        yield return null;

        if (targetScrollRect != null)
        {
            // 아래로 쌓이고 최신이 아래 보이게
            targetScrollRect.verticalNormalizedPosition = 0f;
        }
    }
}