using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 드래그 중인 아이템 정보를 임시로 보관하는 싱글톤
public class DraggedItemHolder : MonoBehaviour
{
    public static DraggedItemHolder Instance;
    public InventoryEntry DraggingEntry { get; private set; }

    //드래그 중
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

    // 드래그 시작
    public void StartDrag(InventoryEntry entry)
    {
        DraggingEntry = entry;
    }

    // 드롭 완료
    public void EndDrag()
    {
        DraggingEntry = null;
    }
}
