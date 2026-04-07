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

    [Header("Tooltip")]
    public Vector2 tooltipOffset = new Vector2(-100f, 200f);

    private InventoryEntry currentEntry;

    public void SetSlot(InventoryEntry entry)
    {
        currentEntry = entry;

        if (currentEntry != null && currentEntry.itemData != null)
        {
            if (iconImage != null)
            {
                iconImage.sprite = currentEntry.itemData.icon;
                iconImage.enabled = true;
            }

            UpdateUI();
        }
        else
        {
            ClearSlot();
        }
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

    void UpdateUI()
    {
        if (countText != null)
        {
            if (currentEntry != null)
                countText.text = currentEntry.count.ToString();
            else
                countText.text = "";
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentEntry == null || currentEntry.itemData == null) return;

        if (ItemTooltip.instance != null)
        {
            ItemTooltip.instance.Show(
                currentEntry.itemData.itemName,
                currentEntry.itemData.itemDescription,
                tooltipOffset
            );
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemTooltip.instance != null)
            ItemTooltip.instance.Hide();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("ItemSlot OnBeginDrag");

        if (currentEntry == null || currentEntry.itemData == null)
        {
            Debug.Log("드래그 실패: currentEntry 없음");
            return;
        }

        if (!IsPotion(currentEntry.itemData))
        {
            Debug.Log("드래그 실패: 포션 아님");
            return;
        }

        if (DraggedItemHolder.Instance != null)
        {
            DraggedItemHolder.Instance.StartDrag(currentEntry);
        }
        else
        {
            Debug.Log("DraggedItemHolder.Instance 없음");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 지금은 마우스 따라다니는 이미지 없으니까 비워둬도 됨
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("ItemSlot OnEndDrag");
        // 여기서 EndDrag() 하면 OnDrop 전에 데이터가 사라질 수 있어서 비우지 않음
    }

    private bool IsPotion(ItemData data)
    {
        if (data == null) return false;

        return data.itemType == ItemData.ItemType.HP_Potion ||
               data.itemType == ItemData.ItemType.MP_Potion ||
               data.itemType == ItemData.ItemType.HP_Potion_Big ||
               data.itemType == ItemData.ItemType.MP_Potion_Big;
    }
}