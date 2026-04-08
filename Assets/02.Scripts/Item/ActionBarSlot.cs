using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ActionBarSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;

    [Header("등록된 아이템")]
    [SerializeField] private ItemData assignedItem;

    [Header("Player")]
    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        RefreshUI();
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("ActionBarSlot OnDrop");

        if (DraggedItemHolder.Instance == null) return;
        if (!DraggedItemHolder.Instance.IsDragging) return;

        InventoryEntry draggedEntry = DraggedItemHolder.Instance.DraggingEntry;
        if (draggedEntry == null || draggedEntry.itemData == null) return;
        if (!ItemHelper.CanAssignToActionBar(draggedEntry.itemData)) return;

        ItemData draggedItem = draggedEntry.itemData;

        ActionBarSlot[] allSlots = FindObjectsOfType<ActionBarSlot>(true);
        foreach (ActionBarSlot slot in allSlots)
        {
            if (slot == null) continue;
            if (slot == this) continue;

            if (slot.assignedItem == draggedItem)
            {
                slot.assignedItem = null;
                slot.RefreshUI();
                Debug.Log("중복 슬롯 비우기: " + draggedItem.itemName);
            }
        }

        assignedItem = draggedItem;
        Debug.Log("등록 완료: " + assignedItem.itemName);

        RefreshUI();
        DraggedItemHolder.Instance.EndDrag();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UseAssignedItem();
    }

    public void UseAssignedItem()
    {
        if (assignedItem == null) return;
        if (InventoryManager.Instance == null) return;
        if (playerStats == null) return;

        ItemData useItem = assignedItem;

        int count = InventoryManager.Instance.GetItemCount(useItem);
        if (count <= 0)
        {
            ClearAssignedItem();

            if (LogManager.Instance != null)
            {
                LogManager.Instance.AddActivityLog("<color=red>[사용 실패]</color> 등록된 아이템이 없습니다.");
            }

            return;
        }

        bool removed = InventoryManager.Instance.RemoveItem(useItem, 1);
        if (!removed) return;

        if (useItem.consumableEffectType == ConsumableEffectType.HealHP)
        {
            playerStats.HealHP(useItem.effectValue);
        }
        else if (useItem.consumableEffectType == ConsumableEffectType.HealMP)
        {
            playerStats.HealMP(useItem.effectValue);
        }

        if (LogManager.Instance != null)
        {
            LogManager.Instance.AddActivityLog(
                $"<color=green>[사용]</color> {useItem.itemName} 1개 사용"
            );
        }

        int remainCount = InventoryManager.Instance.GetItemCount(useItem);
        if (remainCount <= 0)
        {
            ClearAssignedItem();
        }
        else
        {
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        if (assignedItem != null && InventoryManager.Instance != null)
        {
            int currentCount = InventoryManager.Instance.GetItemCount(assignedItem);
            if (currentCount <= 0)
            {
                assignedItem = null;
            }
        }

        if (assignedItem == null)
        {
            if (iconImage != null)
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }

            if (countText != null)
                countText.text = "";

            return;
        }

        if (iconImage != null)
        {
            iconImage.sprite = assignedItem.icon;
            iconImage.enabled = true;
        }

        if (countText != null)
        {
            int count = 0;

            if (InventoryManager.Instance != null)
                count = InventoryManager.Instance.GetItemCount(assignedItem);

            if (count <= 0)
            {
                assignedItem = null;

                if (iconImage != null)
                {
                    iconImage.sprite = null;
                    iconImage.enabled = false;
                }

                countText.text = "";
            }
            else
            {
                countText.text = count.ToString();
            }
        }
    }

    public void ClearAssignedItem()
    {
        assignedItem = null;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (countText != null)
            countText.text = "";
    }
}