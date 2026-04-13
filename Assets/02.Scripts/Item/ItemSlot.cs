using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//툴팁, 클릭, 사용, 로그
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("UI")]
    public Image iconImage;
    public TextMeshProUGUI countText;

    private InventoryEntry currentEntry;

    public void SetSlot(InventoryEntry entry)
    {
        currentEntry = entry;

        if (currentEntry == null || currentEntry.itemData == null)
        {
            ClearSlot();
            return;
        }
        RefreshIcon();
        RefreshCountText();
    }

    public void ClearSlot()
    {
        currentEntry = null;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (countText != null)
            countText.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentEntry == null || currentEntry.itemData == null) return;

        if (ItemTooltip.instance != null)
        {
            ItemTooltip.instance.Show(currentEntry.itemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemTooltip.instance != null)
        {
            ItemTooltip.instance.Clear();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentEntry == null || currentEntry.itemData == null) return;

        InventoryUI invUI = FindObjectOfType<InventoryUI>();
        if (invUI != null)
        {
            invUI.OnItemSelected(currentEntry.itemData.buyPrice);
            invUI.SelectInventoryItem(currentEntry);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentEntry == null || currentEntry.itemData == null) return;

        if (DraggedItemHolder.Instance != null)
        {
            DraggedItemHolder.Instance.StartDrag(currentEntry);
        }
    }
   
    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
    private void RefreshIcon()
    {
        if (iconImage == null) return;

        iconImage.sprite = currentEntry.itemData.icon;
        iconImage.enabled = true;
    }

    private void RefreshCountText()
    {
        if (countText == null) return;

        if (currentEntry == null)
        {
            countText.text = "";
            return;
        }
        countText.text = currentEntry.count.ToString();
    }
}