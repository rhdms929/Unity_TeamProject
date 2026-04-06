using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour //골드 표시, bag슬롯들 갱신 기능
{
    [Header("Gold UI Elements")]
    [SerializeField] private TextMeshProUGUI selectedItemPriceText;
    [SerializeField] private TextMeshProUGUI myCurrentGoldText;

    [Header("Inventory Slots")]
    [SerializeField] private List<ItemSlot> inventorySlots = new List<ItemSlot>();

    [Header("Selected Item Info")]
    [SerializeField] private Image selectedItemIcon;
    [SerializeField] private TextMeshProUGUI selectedItemNameText;
    [SerializeField] private TextMeshProUGUI selectedItemLevelText;
    [SerializeField] private TextMeshProUGUI selectedItemCountText;
    [SerializeField] private TextMeshProUGUI selectedItemDescriptionText;

    [Header("Combine UI")]
    [SerializeField] private Button combineButton;
    [SerializeField] private TextMeshProUGUI combineButtonText;

    private InventoryEntry selectedEntry;

    void Start()
    {
        RefreshMyGold();
        RefreshInventoryUI();
        ClearSelectedItemInfo();
    }

    void OnEnable()
    {
        RefreshMyGold();
        RefreshInventoryUI();

        if (selectedItemPriceText != null)
            selectedItemPriceText.text = "0";

        ClearSelectedItemInfo();
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

    public void SelectInventoryItem(InventoryEntry entry)
    {
        selectedEntry = entry;

        if (entry == null || entry.itemData == null)
        {
            ClearSelectedItemInfo();
            return;
        }

        if (selectedItemIcon != null)
        {
            selectedItemIcon.sprite = entry.itemData.icon;
            selectedItemIcon.enabled = true;
        }

        if (selectedItemNameText != null)
            selectedItemNameText.text = entry.itemData.itemName;

        if (selectedItemLevelText != null)
            selectedItemLevelText.text = "Lv." + entry.itemData.itemLevel.ToString();

        if (selectedItemCountText != null)
            selectedItemCountText.text = "x" + entry.count.ToString();

        if (selectedItemDescriptionText != null)
            selectedItemDescriptionText.text = entry.itemData.itemDescription;

        RefreshCombineButton();
    }

    public void ClearSelectedItemInfo()
    {
        selectedEntry = null;

        if (selectedItemIcon != null)
        {
            selectedItemIcon.sprite = null;
            selectedItemIcon.enabled = false;
        }

        if (selectedItemNameText != null)
            selectedItemNameText.text = "";

        if (selectedItemLevelText != null)
            selectedItemLevelText.text = "";

        if (selectedItemCountText != null)
            selectedItemCountText.text = "";

        if (selectedItemDescriptionText != null)
            selectedItemDescriptionText.text = "";

        RefreshCombineButton();
    }

    public InventoryEntry GetSelectedEntry()
    {
        return selectedEntry;
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

        if (selectedEntry != null)
        {
            InventoryEntry refreshedEntry =
                InventoryManager.Instance.items.Find(x => x.itemData == selectedEntry.itemData);

            if (refreshedEntry != null)
                SelectInventoryItem(refreshedEntry);
            else
                ClearSelectedItemInfo();
        }
        else
        {
            RefreshCombineButton();
        }
    }

    public void OnClickCombineButton()
    {
        if (selectedEntry == null || selectedEntry.itemData == null)
            return;

        ItemData currentItem = selectedEntry.itemData;

        if (!currentItem.canCombine)
        {
            if (LogManager.Instance != null)
                LogManager.Instance.AddActivityLog("이 아이템은 합성할 수 없습니다.");
            return;
        }

        if (currentItem.nextItemData == null)
        {
            if (LogManager.Instance != null)
                LogManager.Instance.AddActivityLog("다음 단계 아이템이 없습니다.");
            return;
        }

        if (selectedEntry.count < currentItem.combineNeedCount)
        {
            if (LogManager.Instance != null)
                LogManager.Instance.AddActivityLog(
                    $"{currentItem.itemName} 합성에 필요한 개수가 부족합니다."
                );
            return;
        }

        bool removed = InventoryManager.Instance.RemoveItem(currentItem, currentItem.combineNeedCount);

        if (!removed) return;

        InventoryManager.Instance.AddItem(currentItem.nextItemData, 1, false);

        if (LogManager.Instance != null)
        {
            LogManager.Instance.AddActivityLog(
                $"{currentItem.itemName} {currentItem.combineNeedCount}개를 합성해 {currentItem.nextItemData.itemName} 1개를 획득했습니다."
            );
        }

        InventoryEntry nextEntry =
            InventoryManager.Instance.items.Find(x => x.itemData == currentItem.nextItemData);

        if (nextEntry != null)
            SelectInventoryItem(nextEntry);
        else
            ClearSelectedItemInfo();

        RefreshInventoryUI();
    }

    private void RefreshCombineButton()
    {
        if (combineButton == null) return;

        bool canUseButton = false;

        if (selectedEntry != null && selectedEntry.itemData != null)
        {
            ItemData data = selectedEntry.itemData;

            canUseButton =
                data.canCombine &&
                data.nextItemData != null &&
                selectedEntry.count >= data.combineNeedCount;
        }

        combineButton.interactable = canUseButton;

        if (combineButtonText != null)
        {
            if (selectedEntry != null && selectedEntry.itemData != null)
            {
                combineButtonText.text =
                    "합성 (" + selectedEntry.count + "/" + selectedEntry.itemData.combineNeedCount + ")";
            }
            else
            {
                combineButtonText.text = "합성";
            }
        }
    }
}