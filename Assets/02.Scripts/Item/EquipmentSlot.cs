using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Header("ΩΩ∑‘ ≈∏¿‘")]
    [SerializeField] private EquipmentSlotType slotType;

    [Header("UI")]
    [SerializeField] private Image iconImage;

    [Header("«ˆ¿Á ¿Â¬¯ æ∆¿Ã≈€")]
    [SerializeField] private ItemData equippedItem;
    public void OnDrop(PointerEventData eventData)
    {
        if (DraggedItemHolder.Instance == null) return;
        if (!DraggedItemHolder.Instance.IsDragging) return;

        InventoryEntry draggedEntry = DraggedItemHolder.Instance.DraggingEntry;
        if (draggedEntry == null || draggedEntry.itemData == null) return;

        ItemData newItem = draggedEntry.itemData;

        if (!ItemHelper.IsMatchingEquipmentSlot(newItem, slotType))
            return;

        Equip(newItem);
        DraggedItemHolder.Instance.EndDrag();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Unequip();
    }
    private void Equip(ItemData newItem)
    {
        if (InventoryManager.Instance == null || newItem == null) return;

        if (equippedItem != null)
        {
            InventoryManager.Instance.AddItem(equippedItem, 1, false);
        }

        bool removed = InventoryManager.Instance.RemoveItem(newItem, 1);
        if (!removed) return;

        equippedItem = newItem;
        RefreshUI();

        if (LogManager.Instance != null)
        {
            LogManager.Instance.AddActivityLog(
                $"<color=green>[¿Â¬¯]</color> {equippedItem.itemName} ¿Â¬¯"
            );
        }
        InventoryManager.Instance.RefreshUI();
    }
    private void Unequip()
    {
        if (equippedItem == null) return;
        if (InventoryManager.Instance == null) return;

        InventoryManager.Instance.AddItem(equippedItem, 1, false);

        if (LogManager.Instance != null)
        {
            LogManager.Instance.AddActivityLog(
                $"<color=yellow>[«ÿ¡¶]</color> {equippedItem.itemName} «ÿ¡¶"
            );
        }
        equippedItem = null;
        RefreshUI();
        InventoryManager.Instance.RefreshUI();
    }

    public void RefreshUI()
    {
        if (iconImage == null) return;

        if (equippedItem == null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }
        else
        {
            iconImage.sprite = equippedItem.icon;
            iconImage.enabled = true;
        }
    }
    public ItemData GetEquippedItem()
    {
        return equippedItem;
    }
}