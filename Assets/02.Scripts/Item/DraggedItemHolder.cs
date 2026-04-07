using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggedItemHolder : MonoBehaviour
{
    public static DraggedItemHolder Instance;

    public InventoryEntry DraggingEntry { get; private set; }

    public bool IsDragging
    {
        get { return DraggingEntry != null; }
    }

    private void Awake()
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

    public void StartDrag(InventoryEntry entry)
    {
        DraggingEntry = entry;

        if (entry != null && entry.itemData != null)
            Debug.Log("드래그 시작: " + entry.itemData.itemName);
        else
            Debug.Log("드래그 시작 실패: entry 없음");
    }

    public void EndDrag()
    {
        Debug.Log("드래그 데이터 비움");
        DraggingEntry = null;
    }
}