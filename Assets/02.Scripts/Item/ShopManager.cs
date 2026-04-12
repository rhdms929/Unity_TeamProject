using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShopManager : MonoBehaviour //아이템 구매하면 inventory로 들어감
{
    public static ShopManager Instance;

    private ShopItem selectedItem;
    private ShopItem previousItem;

    void Awake()
    {
        Instance = this;
    }

    public void SelectItem(ShopItem item)
    {
        if (previousItem != null && previousItem != item)
        {
            previousItem.buyButton.SetActive(false);
        }
        selectedItem = item;
        previousItem = item;

        if (item.buyButton != null)
        {
            item.buyButton.SetActive(true);
        }
    }

    public void BuyItem(ShopItem item)
    {
        if (item == null || item.itemData == null) return;
        if (GameManager.instance == null) return;

        int price = item.itemData.buyPrice;

        if (GameManager.instance.currentGold < price)
        {
            if (LogManager.Instance != null)
            {
                LogManager.Instance.AddActivityLog("<color=red>[구매실패]</color> 골드 부족");
            }
            return;
        }

        GameManager.instance.AddGold(-price);

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(item.itemData, 1, false);
        }

        if (LogManager.Instance != null)
        {
            LogManager.Instance.AddActivityLog($"<color=green>[구매]</color> {item.itemData.itemName} 구매");
        }

        item.buyButton.SetActive(false);
        selectedItem = null;

        InventoryUI invUI = FindObjectOfType<InventoryUI>();
        if (invUI != null)
        {
            invUI.RefreshMyGold();
        }
    }
}