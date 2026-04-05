using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//툴팁, 클릭, 사용, 로그
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
        }

        if (currentEntry.count <= 0) return;

        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player == null) return;

        switch (currentEntry.itemData.itemType)
        {
            case ItemData.ItemType.HP_Potion:
            case ItemData.ItemType.HP_Potion_Big:
                player.HealHP(currentEntry.itemData.healAmount);
                break;

            case ItemData.ItemType.MP_Potion:
            case ItemData.ItemType.MP_Potion_Big:
                player.HealMP(currentEntry.itemData.healAmount);
                break;
        }

        bool removed = InventoryManager.Instance.RemoveItem(currentEntry.itemData, 1);

        if (removed && LogManager.Instance != null)
        {
            LogManager.Instance.AddActivityLog(
                $"{currentEntry.itemData.itemName}을(를) 사용했습니다."
            );
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
}