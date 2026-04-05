using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class InventoryUI : MonoBehaviour //골드 표시, bag슬롯들 갱신 기능
{
    [Header("Gold UI Elements")]
    [SerializeField] private TextMeshProUGUI selectedItemPriceText;
    [SerializeField] private TextMeshProUGUI myCurrentGoldText;

    [Header("Inventory Slots")]
    [SerializeField] private List<ItemSlot> inventorySlots = new List<ItemSlot>();

    void Start()
    {
        RefreshMyGold();
        RefreshInventoryUI();
    }

    void OnEnable()
    {
        RefreshMyGold();
        RefreshInventoryUI();

        if (selectedItemPriceText != null)
            selectedItemPriceText.text = "0";
    }

    public void RefreshMyGold()
    {
        if (GameManager.instance != null && myCurrentGoldText != null)
        {
            myCurrentGoldText.text = ((int)GameManager.instance.currentGold).ToString();
        }
    }

    public void OnItemSelected(int price)
    {
        if (selectedItemPriceText != null)
        {
            selectedItemPriceText.text = price.ToString();
        }
    }

    public void RefreshInventoryUI()
    {
        if (inventorySlots == null || inventorySlots.Count == 0) return;

        foreach (var slot in inventorySlots)
        {
            if (slot != null)
                slot.ClearSlot();
        }

        if (InventoryManager.Instance == null) return;

        for (int i = 0; i < InventoryManager.Instance.items.Count; i++)
        {
            if (i >= inventorySlots.Count) break;

            if (inventorySlots[i] != null)
                inventorySlots[i].SetSlot(InventoryManager.Instance.items[i]);
        }
    }
}